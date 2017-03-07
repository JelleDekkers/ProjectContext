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
    Day5 = 5
}

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Characters CharactersSheet;

    public Player player;
    public ServerData server;

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

        print("serverData:");
        print("code: " + server.Code);
        print("gameState: "+ server.GameState);
        print("players: " + server.players);
        print("players count: " + server.players.Count);

        if (Network.isServer) 
            NetworkManager.OnPlayerDisconnectedFromServer += RefreshPlayerList;
        else 
            NetworkManager.OnConnectedToServerEvent += OnConnectedToServer;
    }

    private void Update() {
        //waiting for connection:
        //if (Network.peerType == NetworkPeerType.Client) {
        //    print("is client");
        //    //NetworkManager.networkView.RPC("StartGame", RPCMode.All);
        //}
    }

    private void OnGUI() {
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
            GUI.Label(new Rect(40, 180 + (i * 10), 300, 20), " name: "+ server.players[i].Name);
            GUI.Label(new Rect(120, 180 + (i * 10), 300, 20), " char id: " + server.players[i].CharID.ToString());
            GUI.Label(new Rect(200, 180 + (i * 10), 300, 20), " ip: " + server.players[i].IpAddress);

            //if(GUI.Button(new Rect(250, 180 + (i * 10), 300, 20), "Randomize Character")) 
            //    NetworkManager.networkView.RPC("AssignCharId", Network.connections[i], GetRandomCharId());
            // if gamestate == -1 && charId == -1, show button to give player a character voor studenten die er de eerste dag niet waren
        }
    }

    private void ClientGUI() {
        GUI.Label(new Rect(10, 60, 1000, 20), "Name: " + player.Name);
        GUI.Label(new Rect(10, 70, 1000, 20), "Gender: " + (Gender)player.Gender);
        GUI.Label(new Rect(10, 80, 1000, 20), "Character ID: " + player.CharacterID);
        if(player.CharacterID == -1)
            GUI.Label(new Rect(10, 90, 1000, 20), "Character Name: " + player.CharacterID);
        else
            GUI.Label(new Rect(10, 90, 1000, 20), "Character Name: " + CharactersSheet.dataArray[player.CharacterID].Name);


        if (GUI.Button(new Rect(10, 120, 300, 40), "Disconnect from server"))
            Quit();
    }

    private void OnConnectedToServer() {
        if (!Network.isServer) 
            NetworkManager.networkView.RPC("AddNewPlayer", RPCMode.Server, player.ID, player.Name, player.CharacterID);
        
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
        if (Network.connections.Length == 0)
            server.players.Clear();

        // all clients need to resend their information again
        for (int i = 0; i < server.players.Count; i++) {
            PlayerData player = server.players[i];
            foreach (NetworkPlayer net in Network.connections) {
                if (player.IpAddress == net.ipAddress)
                    continue;
                else
                    server.players.RemoveAt(i);
            }
        }
    }

    private void OnDestroy() {
        print("object destroyed");
    }

    [RPC]
    private void AddNewPlayer(int id, string name, int charID) {
        string ipAdress = Network.connections[Network.connections.Length - 1].ipAddress;
        PlayerData player = new PlayerData(ipAdress, id, name, charID);
        server.players.Add(player);
    }

    private void TriggerNextDay() {
        if(server.GameState == (int)GameState.Day5) {
            print("final state reached"); // show game over/scores
            return;
        }

        if (server.GameState == (int)GameState.ServerStart) {
            DistributeCharsAmongstPlayers();
            NetworkManager.networkView.RPC("NextDayRPC", RPCMode.Others, server.GameState++);
        } else {
            NetworkManager.networkView.RPC("NextDayRPC", RPCMode.All, server.GameState++);
        }

        server.GameState++;
    }

    private void DistributeCharsAmongstPlayers() {
        List<int> chars = new List<int>();
        foreach (CharactersData data in CharactersSheet.dataArray)
            chars.Add(data.ID);
        
        //semi randomness maken

        for(int i = 0; i < Network.connections.Length; i++) {
            NetworkPlayer player = Network.connections[i];
            int rndIndex = Random.Range(0, chars.Count - 1);
            chars.RemoveAt(rndIndex);
            NetworkManager.networkView.RPC("AssignCharId", player, rndIndex);
            NetworkManager.networkView.RPC("ShowCharacterView", player);
        }
    }

    // voor individuele studenten die geen karakter hebben gekregen op dag 0
    private int GetRandomCharId() {
        int rndId = Random.Range(0, CharactersSheet.dataArray.Length);
        return rndId;
    }

    [RPC]
    private void SetPlayerId(int id) {
        player.ID = id;
    }

    [RPC]
    private void NextDayRPC(int newDayIndex) {
        //trigger new day on clients
    }

    [RPC]
    private void AssignCharId(int charId) {
        player.CharacterID = charId;
        player.SaveData();
        // show character information screen
    }
    
    [RPC]
    private void ShowCharacterView() {
        ScreenManagement.Instance.ShowCharacterView();
    }
}
