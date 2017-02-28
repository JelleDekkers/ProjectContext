using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

public class UdpState {
    public IPEndPoint iPEndPoint;
    public UdpClient updClient;
}

public class GameServer {
    public string Code { get; set; }
    public string IpAddress { get; set; }

    public GameServer(string code, string address) {
        Code = code;
        IpAddress = address;
    }
}

/// <summary>
/// Class for sending and recieving data through wifi.
/// </summary>
public class UDPServerDiscovery{
    public static List<GameServer> foundLocalServers = new List<GameServer>();
    public static Action<string, string> onAddedToList;
    public static Action OnFinishedLookingForServers;

    private const int BROADCAST_PORT = 15000;
    private static int serverPort;
    private static UdpClient broadcastClient; //client
    private static IPEndPoint broadcastEndPoint;
    private static UdpClient listenClient; //server
    public static string serverCode;
    private static readonly UDPServerDiscovery instance = new UDPServerDiscovery();

	private UdpClient udpClient;//voor het closen

    private AsyncCallback listenServerCallback;
    private Stack<GameServer> gameserverStack = new Stack<GameServer>();
	private float listenTimer = 3f;
	
    #region client functions
    public void OpenListeningPort() {
        // open a listening port on a random port to receive a response back from server
        // using 0 doesn't seem to work reliably, so we'll just do it ourselves

        int rndPort = RandomPortNumber();
        try {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, rndPort);
            UdpClient udpClient = new UdpClient(iPEndPoint);
            UdpState udpState = new UdpState();
            udpState.iPEndPoint = iPEndPoint;
            udpState.updClient = udpClient;

            processMainThread = true;
            CoroutineStarter.Instance.StartCoroutine(HandleServerCallbackMainThread());
            listenServerCallback = new AsyncCallback(ListenServerCallbackThreaded);
            udpClient.BeginReceive(listenServerCallback, udpState);
           
            broadcastClient = udpClient;
            broadcastEndPoint = iPEndPoint;
            Debug.Log("Broadcast listener opened on port " + broadcastEndPoint.Port.ToString());
        } catch {
            Debug.LogError("Catch: Something went wrong opening listening port on port nr " + rndPort);
            //PopupManager.ShowErrorPopup("Error", "Something went wrong, please try again");
            //SceneManager.LoadLevel(SceneManager.Menu);
        }
    }

    private void FindServers() {
        // open a broadcast and send own broadcast listener port to the LAN

        foundLocalServers.Clear();

        UdpClient updClient = new UdpClient();
        byte[] sendBytes = BitConverter.GetBytes(broadcastEndPoint.Port);

        // Important!
        // this is disabled by default, so we have to enable it
        updClient.EnableBroadcast = true;

        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT);
        updClient.BeginSend(sendBytes, sendBytes.Length, ipEndPoint, new AsyncCallback(FindServerCallback), updClient);

        Debug.Log("Find server message sent on broadcast listener");
    }

    private void FindServerCallback(IAsyncResult ar) {
        // broadcast has finished, endSend
        UdpClient updClient = (UdpClient)ar.AsyncState;
        int bytesSent = updClient.EndSend(ar);

        Debug.Log("Completed search for servers");

        // close the broadcast client
        updClient.Close();
    }

    //Does not run at main thread.
   public void ListenServerCallbackThreaded(IAsyncResult ar) {
        // server has responded with its game name
        // send this to the stateController
        Debug.Log("ListenServerCallback, bytes received:" + broadcastClient.Available);

        try {
            IPEndPoint ep1 = ((UdpState)(ar.AsyncState)).iPEndPoint;
            byte[] receiveBytes = broadcastClient.EndReceive(ar, ref ep1);
            string receiveData = Encoding.ASCII.GetString(receiveBytes);

            Debug.Log("["+ep1.Address+"] receiveData: " + receiveData);

            gameserverStack.Push(new GameServer(receiveData, ep1.Address.ToString()));
            broadcastClient.BeginReceive(listenServerCallback, ar.AsyncState);
            
        } catch (Exception e) {
            Debug.LogError(e.Message + e.StackTrace);
        }
    }

    //Parallel process running on main-thread handles Unity-objects.
	private bool processMainThread = true;
	
    private IEnumerator HandleServerCallbackMainThread() {
		float timer = 0;
		while(processMainThread && timer < listenTimer) {
			timer += Time.deltaTime;

			while (gameserverStack.Count != 0) { 
				GameServer server = gameserverStack.Pop();
				AddLocalServer(server.Code, server.IpAddress);
			}
            yield return null;
        }

        CloseListeningPort();
		Debug.Log("HandleServerCallbackMainThread Stop");
    }

    private static void AddLocalServer(string name, string address) {
        Debug.Log("Server found: " + name + " address: " + address);
        foundLocalServers.Add(new GameServer(name, address));
        if(onAddedToList != null)
            onAddedToList(name, address);
    }

    private void CloseListeningPort() {
        if(!processMainThread) 
            return;

        //close the broadcast listener, this is needed to start a new search
        try {
            processMainThread = false;
            broadcastClient.Close();
            Debug.Log("Broadcast listener closed");
        } catch {

        }

        if (OnFinishedLookingForServers != null)
            OnFinishedLookingForServers();
    }
    #endregion

    #region server functions
    private void StartServer(string _serverCode) {
        if (!Network.isServer) {
            int maxConnectionTries = 20;
            int rndServerPort;

            for (int i = 0; i < maxConnectionTries; i++) {
                rndServerPort = RandomPortNumber();
                NetworkConnectionError err = Network.InitializeServer(2, NetworkManager.ConnectionPort, !Network.HavePublicAddress()); //NetworkManager.ConnectionPort
                if (err == NetworkConnectionError.NoError) {
                    serverPort = rndServerPort;
                    serverCode = _serverCode;
                    Debug.Log("servercode: " + serverCode);
                    Debug.Log("Server started");
                    return;
                }
            }
            Debug.LogError("Max connections tries reached, all tried ports are in use?");
            //SceneManager.LoadLevel(SceneManager.Menu);
            //PopupManager.ShowErrorPopup("Error", "Something went wrong starting a server, please try again");
        }
        else {
            Debug.LogWarning("Already connected as server");
        }
    }

    private void ListenForClients() {
        try {
            // open a listening port to listen for any clients
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, BROADCAST_PORT);
            UdpClient udpClient = new UdpClient(ipEndPoint);
            listenClient = udpClient;
            UdpState udpState = new UdpState();
            udpState.iPEndPoint = ipEndPoint;
            udpState.updClient = udpClient;
            udpClient.BeginReceive(new AsyncCallback(ListenForClientsCallback), udpState);

            Debug.Log("Server listening port opened");
        } catch {
            //PopupManager.ShowErrorPopup("Error", "Something went wrong, please try again");
            //SceneManager.LoadLevel(SceneManager.Menu);
        }
    }

    private void ListenForClientsCallback(IAsyncResult ar) {
        //Received a broadcast from a client
        Debug.Log("Client message received on server listening port");

        UdpClient udpClient = (UdpClient)((UdpState)(ar.AsyncState)).updClient;
        IPEndPoint ipEndpoint = (IPEndPoint)((UdpState)(ar.AsyncState)).iPEndPoint;
        byte[] receiveBytes = udpClient.EndReceive(ar, ref ipEndpoint);
        int clientPort = BitConverter.ToInt32(receiveBytes, 0);

        Debug.Log("Client is listening for reply on broadcast port " + clientPort.ToString());

        //Send a response back to the client on the port they sent us
        string sentData = serverCode;
        byte[] sendBytes = Encoding.ASCII.GetBytes(sentData);

        UdpClient uc2 = new UdpClient();
        IPEndPoint ep2 = new IPEndPoint(ipEndpoint.Address, clientPort);
        uc2.BeginSend(sendBytes, sendBytes.Length, ep2, new AsyncCallback(RespondClientCallback), uc2);

        // Important!
        // close and re-open the broadcast listening port so that another async operation can start 
        udpClient.Close();
        Debug.Log("server listening port closed");
        ListenForClients();
    }

    private void RespondClientCallback(IAsyncResult ar) {
        //Reply to client has finished
        UdpClient udpClient = (UdpClient)ar.AsyncState;
        int bytesSent = udpClient.EndSend(ar);

        //Close the response port
        udpClient.Close();
    }
    #endregion

    #region static public functions
    public static void HostServer(string serverCode) {
        instance.StartServer(serverCode);
        instance.ListenForClients();
    }

    public static void SearchForServers() {
        instance.OpenListeningPort();
        instance.FindServers();
    }

    public static void Refresh() {
        foundLocalServers.Clear();
        instance.CloseListeningPort();
        SearchForServers();
    }

    public static void StopListeningForClients() {
        listenClient.Close();
        Debug.Log("server listening port closed");
    }

    public static void StopHostingServer() {
        Network.Disconnect();
        StopListeningForClients();
    }
    #endregion
    
    private int RandomPortNumber() {
        System.Random r = new System.Random();
        return r.Next(15000, 40000);
    }
}
