using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Genders {
    Man = 0,
    Woman = 1
}

[System.Serializable]
public static class Player {

    public static string ServerCode;
    public static int ID = -1;
    public static string Name;
    public static int CharacterID = -1;
    public static int Gender;
    public static int GameState = -1;

    private const string serverCodeKey = "serverCode";
    private const string idKey = "id";
    private const string nameKey = "name";
    private const string genderKey = "gender";
    private const string charKey = "charID";
    private const string gameStateKey = "stateKey";

    public static void LoadData() {
        ServerCode = PlayerPrefs.GetString(serverCodeKey);
        Name = PlayerPrefs.GetString(nameKey);
        Gender = PlayerPrefs.GetInt(genderKey);

        if (PlayerPrefs.HasKey(idKey))
            ID = PlayerPrefs.GetInt(idKey);
        if (PlayerPrefs.HasKey(charKey))
            CharacterID = PlayerPrefs.GetInt(charKey);
        if (PlayerPrefs.HasKey(gameStateKey))
            GameState = PlayerPrefs.GetInt(gameStateKey);
    }

    public static void SaveServerCode(string code) {
        PlayerPrefs.SetString(serverCodeKey, code);
    }

    public static void SaveID(int id) {
        PlayerPrefs.SetInt(idKey, id);
    }

    public static void SaveName(string name) {
        PlayerPrefs.SetString(nameKey, name);
    }

    public static void SaveCharID(int id) {
        PlayerPrefs.SetInt(charKey, id);
    }

    public static void SaveGender(Genders gender) {
        PlayerPrefs.SetInt(genderKey, (int)gender);
    }

    public static void SaveGameState(GameState state) {
        PlayerPrefs.SetInt(gameStateKey, (int)state);
    }

    public static void SaveGameState(int state) {
        PlayerPrefs.SetInt(gameStateKey, state);
    }
}
