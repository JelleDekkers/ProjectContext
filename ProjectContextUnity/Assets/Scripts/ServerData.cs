using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerData {

    public static string ServerCode;
    public static int GameState = -1;

    private const string serverCodeKey = "serverCode";
    private const string gameStateKey = "stateKey";

    public static void LoadData() {
        ServerCode = PlayerPrefs.GetString(serverCodeKey);
        if(PlayerPrefs.HasKey(gameStateKey))
            GameState = PlayerPrefs.GetInt(gameStateKey);
    }

    public static void SaveServerCode(string code) {
        PlayerPrefs.SetString(serverCodeKey, code);
    }

    public static void SaveGameState(int state) {
        PlayerPrefs.SetInt(gameStateKey, state);
    }
}
