using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private string clientCode = "";
    private string serverCode = "";
    private string playerName;

    private int serversFoundCount { get { return UDPServerDiscovery.foundLocalServers.Count; } }
    private string[] genders = new string[] { "Man", "Vrouw" };
    private int playerGender = 0;

    private void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        GamePrefs.LoadData();
    }

    private void Start() {
        serverCode = GamePrefs.ServerCode;
        clientCode = GamePrefs.ServerCode;
        playerName = GamePrefs.Name;
        playerGender = GamePrefs.Gender;
    }

    private void OnGUI() {
        //network info:
        GUI.Label(new Rect(10, 10, 1000, 20), "Network reachability: " + Application.internetReachability);

        serverCode = GUI.TextField(new Rect(220, 30, 100, 40), serverCode);
        if (GUI.Button(new Rect(10, 30, 200, 40), "Host Server")) 
            HostGame(serverCode);

        clientCode = GUI.TextField(new Rect(220, 80, 100, 40), clientCode);
        if (GUI.Button(new Rect(10, 80, 200, 40), "Connect to server"))
            GetServersAndConnect();

        GUI.Label(new Rect(220, 130, 100, 40), "Servers found: " + serversFoundCount);

        // player info:
        GUI.Label(new Rect(10, 180, 300, 20), "Enter your name: ");
        playerName = GUI.TextField(new Rect(10, 200, 100, 40), playerName);
        playerGender = GUI.Toolbar(new Rect(120, 200, 100, 40), playerGender, genders);
        if (GUI.Button(new Rect(10, 250, 100, 40), "Save"))
            SavePlayerInfo();

        if (GUI.Button(new Rect(10, 350, 200, 40), "Delete PlayerPrefs"))
            PlayerPrefs.DeleteAll();
        if (GUI.Button(new Rect(10, 400, 200, 40), "Delete Player Data"))
            Player.DeleteData();
        if (GUI.Button(new Rect(10, 450, 200, 40), "Delete Server Data"))
            ServerData.DeleteData();
    }

    public void HostGame(string code) {
        //if(code == "") {
        //    print("Please enter a code");
        //    return;
        //}

        LoadingViewManager.Instance.Show("Starting Server");

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
            GamePrefs.SaveServerCode(serverCode);
            NetworkManager.serverCode = serverCode;
            NetworkManager.CreateServer();
            SceneManager.LoadScene("game");
        } else {
            PopupManager.Instance.ShowPopup("Error", "Connection Error, please enable Wifi to play");
        }
    }

    private void GetServersAndConnect() {
        LoadingViewManager.Instance.Show("Joining Server");
        UDPServerDiscovery.SearchForServers();
        UDPServerDiscovery.OnFinishedLookingForServers += LoadingViewManager.Instance.Hide;
        UDPServerDiscovery.OnFinishedLookingForServers += ConnectToServerByCode;
    }

    public void ConnectToServerByCode() {
        //if (clientCode == "") {
        //    print("Please enter a code");
        //    return;
        //}
        GamePrefs.SaveServerCode(clientCode);
        foreach (GameServer server in UDPServerDiscovery.foundLocalServers) {
            if (server.Code.ToLower() == clientCode.ToLower()) {
                UDPServerDiscovery.OnFinishedLookingForServers -= LoadingViewManager.Instance.Hide;
                UDPServerDiscovery.OnFinishedLookingForServers -= ConnectToServerByCode;
                NetworkManager.ConnectToServer(server.IpAddress);
                SceneManager.LoadScene("game");
                return;
            }
        }
        UDPServerDiscovery.OnFinishedLookingForServers -= LoadingViewManager.Instance.Hide;
        UDPServerDiscovery.OnFinishedLookingForServers -= ConnectToServerByCode;
        PopupManager.Instance.ShowPopup("Error", "No server found with corresponding code");
    }

    private void SavePlayerInfo() {
        if(!Common.IsValidName(playerName)) {
            PopupManager.Instance.ShowPopup("Error", "Please enter a valid name");
            return;
        }

        GamePrefs.SaveName(playerName);
        GamePrefs.SaveGender((Gender)playerGender);
    }
}
