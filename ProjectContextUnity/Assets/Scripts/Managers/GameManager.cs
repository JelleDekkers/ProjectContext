using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState {
    ServerStart = -1, //waiting for students to all join the game, triggering next day distributes characters to all connected players
    Day0 = 0, // introduction with character and game mechanics / waiting for teacher to trigger day 1
    Day1 = 1,
    Day2 = 2,
    Day3 = 3,
    Day4 = 4,
    Day5 = 5,
    Day6 = 6 // liberation, character post game
}

public class GameManager : MonoBehaviour {

    public Player player;
    public ServerData server;
    public bool ShowDebugOptions = false;

    [SerializeField]
    private Canvas studentCanvas;
    [SerializeField]
    private Canvas teacherCanvas;

    private void Start() {
        if (!Network.isServer) {
            player = new Player();
            if (Player.FileExists() == false) {
                player.Name = GamePrefs.Name;
                player.SaveData();
            } else {
                player = player.LoadData();
            }
        }

        if(Network.isServer) {
            server = new ServerData();
            if (ServerData.FileExists() == false) {
                server.GameState = -1;
                server.players = new List<PlayerData>();
            } else {
                server = server.LoadData();
            }
            server.Code = GamePrefs.ServerCode;
            server.SaveData();
        }

        if (Network.isServer) {
            NetworkManager.OnPlayerDisconnectedFromServer += RefreshPlayerList;
        } else {
            NetworkManager.OnConnectedToServerEvent += OnConnectedToServer;
            if (player.CharacterID == -1)
                LoadingViewManager.Instance.Show("Wachten op docent");

            if (player.CharacterID != -1) { 
                SetVillageHouses(player.allPlayerChars);
                VillageView.Instance.SetupCharacterButton(player.CharacterID);
                FlowchartHandler.Instance.SetFlowChart(player.CharacterID);
            }
        }

        if(GameVersion.Instance.Version == Version.Teacher) {
            teacherCanvas.gameObject.SetActive(true);
            studentCanvas.gameObject.SetActive(false);
        } else {
            teacherCanvas.gameObject.SetActive(false);
            studentCanvas.gameObject.SetActive(true);
        }
    }

    private void OnGUI() {
        ShowDebugOptions = GUI.Toggle(new Rect(10, Screen.height - 50, 100, 100), ShowDebugOptions, " Debug Options");

        if (!ShowDebugOptions)
            return;

        GUI.Label(new Rect(10, 10, 1000, 20), "Is host: " + NetworkManager.IsHost);
        GUI.Label(new Rect(10, 20, 1000, 20), "Server Code: " + NetworkManager.serverCode);
        GUI.Label(new Rect(10, 30, 1000, 20), "connections: " + Network.connections.Length);

        if (Network.isServer)
            HostGUI();
        else
            ClientGUI();
    }

    string msg = "";
    private void HostGUI() {
        GUI.Label(new Rect(10, 50, 1000, 20), "Game State (Host): " + server.GameState);

        if (GUI.Button(new Rect(10, 70, 300, 40), "Stop Hosting Server"))
            Quit();
        if (GUI.Button(new Rect(10, 120, 300, 40), "Trigger Next Day"))
            TriggerNextDay();

        // player list:
        for (int i = 0; i < server.players.Count; i++) {
            GUI.Label(new Rect(10, 180 + (i * 10), 20, 20), i + ": ");
            GUI.Label(new Rect(40, 180 + (i * 10), 100, 20), ((ConnectionStatus)server.players[i].ConnectionStatus).ToString());
            GUI.Label(new Rect(100, 180 + (i * 10), 300, 20), " name: "+ server.players[i].Name);
            GUI.Label(new Rect(200, 180 + (i * 10), 300, 20), " char id: " + server.players[i].CharID.ToString());
            GUI.Label(new Rect(300, 180 + (i * 10), 300, 20), " ip: " + server.players[i].IpAddress);

            //if(GUI.Button(new Rect(250, 180 + (i * 10), 300, 20), "Randomize Character")) 
            //    NetworkManager.networkView.RPC("AssignCharId", Network.connections[i], GetRandomCharId());
            // if gamestate == -1 && charId == -1, show button to give player a character voor studenten die er de eerste dag niet waren
        }
    }

    private void ClientGUI() {
        GUI.Label(new Rect(10, 60, 1000, 20), "Name: " + player.Name);
        GUI.Label(new Rect(10, 70, 1000, 20), "Gender: " + (Gender)player.Gender);
        GUI.Label(new Rect(10, 80, 1000, 20), "Character ID: " + player.CharacterID);
        GUI.Label(new Rect(10, 90, 1000, 20), "Game ID: " + player.ID);
        if (player.CharacterID == -1)
            GUI.Label(new Rect(10, 100, 1000, 20), "Character Name: " + player.CharacterID);
        else
            GUI.Label(new Rect(10, 100, 1000, 20), "Character Name: " + CharactersDatabase.Instance.Data.dataArray[player.CharacterID].Name);


        if (GUI.Button(new Rect(10, 120, 300, 40), "Disconnect from server"))
            Quit();
    }

    private void OnConnectedToServer() {
        if (!Network.isServer) 
            NetworkManager.networkView.RPC("PlayerConnected", RPCMode.Server, player.ID, player.Name, player.Gender, player.CharacterID);
        
        NetworkManager.OnConnectedToServerEvent -= OnConnectedToServer;
    }

    public void Quit() {
        NetworkManager.Quit();
        SceneManager.LoadScene("menu");
    }

    private void RefreshPlayerList() {
        StartCoroutine(RefreshPlayerListCoroutine());
    }

    private IEnumerator RefreshPlayerListCoroutine() {
        float waitTime = 2;
        yield return new WaitForSeconds(waitTime);
        if (Network.connections.Length == 0) {
            foreach (PlayerData player in server.players) {
                if (player.ConnectionStatus == (int)ConnectionStatus.Online) {
                    player.ConnectionStatus = (int)ConnectionStatus.Offline;
                    server.SaveData();
                    PlayerList.Instance.UpdateList(server.players);
                    yield return null;
                }
            }
        }

        // cycle through player data list
        for (int i = 0; i < server.players.Count; i++) {
            PlayerData player = server.players[i];
            //cycle through network players list
            foreach (NetworkPlayer net in Network.connections) {
                if (player.IpAddress == net.ipAddress) {
                    continue;
                } else {
                    server.players[i].ConnectionStatus = (int)ConnectionStatus.Offline;
                }
            }
        }

        PlayerList.Instance.UpdateList(server.players);
    }

    [RPC]
    private void PlayerConnected(int id, string name, int gender, int charID) {
        NetworkPlayer networkPlayer = Network.connections[Network.connections.Length - 1];
        string ipAdress = networkPlayer.ipAddress;

        // first time join:
        if (id == -1) {
            PlayerData player = new PlayerData(id, ipAdress, name, gender, charID);
            player.ConnectionStatus = (int)ConnectionStatus.Online;
            int newId = server.players.Count;
            player.ID = newId;
            server.players.Add(player);
            NetworkManager.networkView.RPC("SetPlayerId", networkPlayer, newId);
        } else {
            PlayerData player = server.players[id];
            player.ConnectionStatus = (int)ConnectionStatus.Online;
            player.Name = name;
        }

        server.SaveData();
        PlayerList.Instance.UpdateList(server.players);
    }

    [RPC]
    private void SetPlayerId(int id) {
        player.ID = id;
        player.SaveData();
    }

    public void TriggerNextDay() {
        if(server.GameState == (int)GameState.Day5) {
            print("final state reached"); // show game over/scores
            return;
        }

        if (server.GameState == (int)GameState.ServerStart) {
            DistributeCharsAmongstPlayers();
        } else {
            NetworkManager.networkView.RPC("NextDayRPC", RPCMode.Others, server.GameState + 1);
        }

        server.GameState++;
        server.SaveData();
        RefreshPlayerList();
    }

    private void DistributeCharsAmongstPlayers() {
        List<int> availableChars = new List<int>();
        List<int> distributedChars = new List<int>();
        foreach (CharactersData c in CharactersDatabase.Instance.Data.dataArray)
            availableChars.Add(c.ID);
       
        // assign character
        for(int i = 0; i < server.players.Count; i++) {
            PlayerData player = server.players[i];
            if (player.ConnectionStatus == (int)ConnectionStatus.Offline)
                continue;

            if(i > availableChars.Count) {
                Debug.LogWarning("more players than characters. These players will all be the final charachter.");
                player.CharID = availableChars.Count - 1;
            } else {
                player.CharID = availableChars[i];
            }

            distributedChars.Add(player.CharID);
        }

        // distribute characters
        for (int i = 0; i < server.players.Count; i++) {
            PlayerData player = server.players[i];
            if (player.ConnectionStatus == (int)ConnectionStatus.Offline)
                continue;

            foreach (NetworkPlayer networkPlayer in Network.connections) {
                if (networkPlayer.ipAddress == player.IpAddress) {
                    NetworkManager.networkView.RPC("AssignCharId", networkPlayer, player.CharID);
                    string convertedList = Common.ConvertToString(distributedChars);
                    NetworkManager.networkView.RPC("SetVillageHouses", networkPlayer, convertedList);
                } else {
                    Debug.LogWarning("No corresponding ip adress found");
                }
            }
        }

        PlayerList.Instance.UpdateList(server.players);
    }

    [RPC]
    private void AssignCharId(int charId) {
        player.CharacterID = charId;
        player.SaveData();
        ScreenManagement.Instance.ShowCharacterView();
        VillageView.Instance.SetupCharacterButton(player.CharacterID);
        CharacterInfoPanel.Instance.SetInfo(charId);
        FlowchartHandler.Instance.SetFlowChart(charId);
        LoadingViewManager.Instance.Hide();
    }

    [RPC]
    private void SetVillageHouses(string chars) {
        List<int> list = Common.ConvertToIntList(chars);
        SetVillageHouses(list);
    }

    private void SetVillageHouses(List<int> list) {
        player.allPlayerChars = list;
        player.SaveData();
        print("players count: " + list.Count);
        VillageView.Instance.AssignHouses(list);
    }

    // voor individuele studenten die geen karakter hebben gekregen op dag 0
    private int GetRandomCharId() {
        int rndId = Random.Range(0, CharactersDatabase.Instance.Data.dataArray.Length);
        return rndId;
    }

    [RPC]
    private void NextDayRPC(int newDayIndex) {
        DailyEventManager.Instance.StartDailyEvent(newDayIndex - 1); // -1 omdat de index van newsEvents begint op 0 maar gameState bij 1
    }
}
