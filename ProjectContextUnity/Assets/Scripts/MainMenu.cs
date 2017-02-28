﻿using UnityEngine;
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
        Player.LoadData();
        ServerData.LoadData();
    }

    private void Start() {
        serverCode = ServerData.ServerCode;
        clientCode = Player.ServerCode;
        playerName = Player.Name;
        playerGender = Player.Gender;
    }

    private void OnGUI() {
        //network info:
        GUI.Label(new Rect(10, 10, 1000, 20), "Network reachability: " + Application.internetReachability);

        serverCode = GUI.TextField(new Rect(320, 30, 100, 40), serverCode);
        if (GUI.Button(new Rect(10, 30, 300, 40), "Host Server")) 
            HostGame(serverCode);

        clientCode = GUI.TextField(new Rect(320, 80, 100, 40), clientCode);
        if (GUI.Button(new Rect(10, 80, 300, 40), "Connect to server"))
            GetServers();

        GUI.Label(new Rect(320, 130, 100, 40), "Servers found: " + serversFoundCount);

        // player info:
        GUI.Label(new Rect(10, 180, 300, 20), "Enter your name: ");
        playerName = GUI.TextField(new Rect(10, 200, 100, 40), playerName);
        playerGender = GUI.Toolbar(new Rect(120, 200, 100, 40), playerGender, genders);
        if (GUI.Button(new Rect(10, 250, 100, 40), "Save"))
            SavePlayerInfo();

        if (GUI.Button(new Rect(10, 350, 200, 40), "Delete PlayerPrefs"))
            PlayerPrefs.DeleteAll();
    }

    public void HostGame(string code) {
        //if(code == "") {
        //    print("Please enter a code");
        //    return;
        //}

        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
            ServerData.SaveServerCode(serverCode);
            NetworkManager.serverCode = serverCode;
            NetworkManager.CreateServer();
            SceneManager.LoadScene("game");
        } else {
            print("Connection Error, please enable Wifi to play.");
        }
    }

    private void GetServers() {
        UDPServerDiscovery.SearchForServers();
        UDPServerDiscovery.OnFinishedLookingForServers += ConnectToServerByCode;
    }

    public void ConnectToServerByCode() {
        //if (clientCode == "") {
        //    print("Please enter a code");
        //    return;
        //}
        Player.SaveServerCode(clientCode);
        foreach (GameServer server in UDPServerDiscovery.foundLocalServers) {
            if (server.Code.ToLower() == clientCode.ToLower()) {
                UDPServerDiscovery.OnFinishedLookingForServers -= ConnectToServerByCode;
                NetworkManager.ConnectToServer(server.IpAddress);
                SceneManager.LoadScene("game");
                return;
            }
        }
        UDPServerDiscovery.OnFinishedLookingForServers -= ConnectToServerByCode;
        print("Error, No server found with corresponding code.");
    }

    private void SavePlayerInfo() {
        if(!Common.IsValidName(playerName)) {
            print("enter a valid name");
            return;
        }

        Player.SaveName(playerName);
        Player.SaveGender((Gender)playerGender);
    }
}
