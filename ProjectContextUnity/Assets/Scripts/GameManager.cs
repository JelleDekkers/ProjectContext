using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
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
    [SerializeField]
    public List<PlayerData> allPlayers = new List<PlayerData>();

    private void Start() {
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

    private void HostGUI() {
        GUI.Label(new Rect(10, 50, 1000, 20), "Game State (Host): " + ServerData.GameState);

        if (GUI.Button(new Rect(10, 70, 300, 40), "Stop Hosting Server"))
            Quit();
        if (GUI.Button(new Rect(10, 120, 300, 40), "Trigger Next Day"))
            TriggerNextDay();

        // player list:
        for (int i = 0; i < allPlayers.Count; i++) {
            GUI.Label(new Rect(10, 180 + (i * 10), 20, 20), i + ": ");
            GUI.Label(new Rect(40, 180 + (i * 10), 300, 20), " name: "+ allPlayers[i].Name);
            GUI.Label(new Rect(120, 180 + (i * 10), 300, 20), " char id: " + allPlayers[i].CharID.ToString());
            GUI.Label(new Rect(200, 180 + (i * 10), 300, 20), " ip: " + allPlayers[i].IpAddress);

            //if(GUI.Button(new Rect(250, 180 + (i * 10), 300, 20), "Randomize Character")) 
            //    NetworkManager.networkView.RPC("AssignCharId", Network.connections[i], GetRandomCharId());
            // if gamestate == -1 && charId == -1, show button to give player a character voor studenten die er de eerste dag niet waren
        }
    }

    private void ClientGUI() {
        GUI.Label(new Rect(10, 60, 1000, 20), "Name: " + Player.Name);
        GUI.Label(new Rect(10, 70, 1000, 20), "Gender: " + (Gender)Player.Gender);
        GUI.Label(new Rect(10, 80, 1000, 20), "Character ID: " + Player.CharacterID);
        GUI.Label(new Rect(10, 90, 1000, 20), "Character Name: " + CharactersSheet.dataArray[Player.CharacterID].Name);


        if (GUI.Button(new Rect(10, 120, 300, 40), "Disconnect from server"))
            Quit();
    }

    private void OnConnectedToServer() {
        if (!Network.isServer) 
            NetworkManager.networkView.RPC("AddNewPlayer", RPCMode.Server, Player.ID, Player.Name, Player.CharacterID);
        
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
            allPlayers.Clear();

        // all clients need to resend their information again
        for (int i = 0; i < allPlayers.Count; i++) {
            PlayerData player = allPlayers[i];
            foreach (NetworkPlayer net in Network.connections) {
                if (player.IpAddress == net.ipAddress)
                    continue;
                else
                    allPlayers.RemoveAt(i);
            }
        }
    }

    private void OnDestroy() {
        print("object destroyed");
    }

    [RPC]
    private void AddNewPlayer(int id, string name, int charID) {
        string ipAdress = Network.connections[Network.connections.Length - 1].ipAddress;
        PlayerData p = new PlayerData(ipAdress, id, name, charID);
        allPlayers.Add(p);
    }

    private void TriggerNextDay() {
        if(ServerData.GameState == (int)GameState.Day5) {
            print("final state reached"); // show game over/scores
            return;
        }

        if (ServerData.GameState == (int)GameState.ServerStart) {
            DistributeCharsAmongstPlayers();
            NetworkManager.networkView.RPC("NextDayRPC", RPCMode.Others, ServerData.GameState++);
        } else {
            NetworkManager.networkView.RPC("NextDayRPC", RPCMode.All, ServerData.GameState++);
        }

        ServerData.GameState++;
        ServerData.SaveGameState(ServerData.GameState);
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
        }
    }

    // voor individuele studenten die geen karakter hebben gekregen op dag 0
    private int GetRandomCharId() {
        int rndId = Random.Range(0, CharactersSheet.dataArray.Length);
        return rndId;
    }

    [RPC]
    private void SetPlayerId(int id) {
        Player.ID = id;
        Player.SaveID(id);
    }

    [RPC]
    private void NextDayRPC(int newDayIndex) {
        //trigger new day on clients
    }

    [RPC]
    private void AssignCharId(int charId) {
        Player.CharacterID = charId;
        Player.SaveCharID(charId);
        // show character information screen
    }
}
