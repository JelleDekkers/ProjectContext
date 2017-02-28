using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Player {

    public static string ServerCode;
    public static int ID = -1;
    public static string Name;
    public static int CharacterID = -1;
    public static int Gender;

    private const string serverCodeKey = "serverCode";
    private const string idKey = "id";
    private const string nameKey = "name";
    private const string genderKey = "gender";
    private const string charKey = "charID";

    public static void LoadData() {
        ServerCode = PlayerPrefs.GetString(serverCodeKey);
        Name = PlayerPrefs.GetString(nameKey);
        Gender = PlayerPrefs.GetInt(genderKey);

        if (PlayerPrefs.HasKey(idKey))
            ID = PlayerPrefs.GetInt(idKey);
        if (PlayerPrefs.HasKey(charKey))
            CharacterID = PlayerPrefs.GetInt(charKey);
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

    public static void SaveGender(Gender gender) {
        PlayerPrefs.SetInt(genderKey, (int)gender);
    }
}
