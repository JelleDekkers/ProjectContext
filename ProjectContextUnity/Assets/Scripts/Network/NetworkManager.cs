using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum ConnectionState {
    Idle,       //menu state
    Joining,    //Trying to connect to a server
    Connected,  //connected to server/is server
}

public class NetworkManager : MonoBehaviour {

    public static string ConnectionIp = "127.0.0.1";
    public static ushort ConnectionPort = 25000;
    public static NetworkView networkView;
    public static string localIpAddress { get; private set; }
    private static INetwork network;
    public static string serverCode = "";
    public static bool IsHost;
    public static string hostIpAddress;
    public static string hostPort;

    private float timeOutTimer = 0;
    private const float TIME_OUT_TIMER_MAX = 20;

    public static Action OnConnectedToServerEvent;
    public static Action OnPlayerDisconnectedFromServer;

    private static void NetworkSetup() {
        network = new LocalNetworkManager();
    }

    private void Awake() {
        Init();
        if (IsHost) {
            UDPServerDiscovery.HostServer(serverCode);
        } else {
            network.ConnectToServer(hostIpAddress);
        }
    }

    private void Init() {
        networkView = FindObjectOfType(typeof(NetworkView)) as NetworkView;
        NetworkSetup();
        localIpAddress = Network.player.ipAddress.ToString();
    }

    public void SearchForServers() {
        UDPServerDiscovery.SearchForServers();
    }

    private void Update() {
        if (Network.peerType == NetworkPeerType.Disconnected) {
            if (timeOutTimer < TIME_OUT_TIMER_MAX)
                timeOutTimer += Time.deltaTime;
            else {
                Debug.Log("Connection timed out");
                Quit();
                SceneManager.LoadScene("menu");
            }
        }
    }

    private void OnGUI() {
        if (Network.connections.Length >= 1) {
            GUI.Label(new Rect(Screen.width - 100, 10, 100, 20), "Ping: " + Network.GetAveragePing(Network.connections[0]));
        }
    }

    private void OnDestroy() {
        network.OnDestroyEvent();
    }

    #region Connection Events
    //Networking events do not get called with non-monobehaviour scripts
    private void OnConnectedToServer() {
        print("connected to server");
        network.OnConnectedToServer();
        if (OnConnectedToServerEvent != null)
            OnConnectedToServerEvent();
    }

    private void OnPlayerConnected(NetworkPlayer player) {
        print("Player connected");
    }

    private void OnFailedToConnect(NetworkConnectionError error) {
        Debug.Log("Failed to connect to server, error info: " + error);
        network.OnFailedToConnect(error);
        SceneManager.LoadScene("menu");
    }

    private void OnDisconnectedFromServer(NetworkDisconnection info) {
        Debug.Log("OnDisconnectedFromServer()");
        network.OnDisconnectedFromServer(info);
        SceneManager.LoadScene("menu");
    }

    private void OnPlayerDisconnected(NetworkPlayer player) {
        network.OnPlayerDisconnected(player);
        if (Network.isServer)
            OnPlayerDisconnectedFromServer();

        print("Player disconnected: " + player);
    } 

    private void OnServerInitialized() {
        network.OnServerInitialized();
    }
    #endregion Connection Events

    public static void CreateServer() {
        print("Creating server");
        IsHost = true;
    }

    public static void ConnectToServer(string ipAddress) {
        IsHost = false;
        hostIpAddress = ipAddress;
    }

    public static void ConnectToServer() {
        ConnectToServer("");
    }

    public static void Quit() {
        network.OnQuit();
    }

    #region Network RPCs

    [RPC]
    private void Disconnect() {
        network.Disconnect();
    }
    #endregion
}
