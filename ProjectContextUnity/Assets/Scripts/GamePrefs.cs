using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores cookies over different sessions to prevent having to re-enter server code everytime
/// </summary>
public static class GamePrefs {

    public static string ServerCode;
    public static string Name;
    public static int Gender;

    private const string serverCodeKey = "serverCode";
    private const string nameKey = "name";
    private const string genderKey = "gender";

    public static void LoadData() {
        ServerCode = PlayerPrefs.GetString(serverCodeKey);
        Name = PlayerPrefs.GetString(nameKey);
        Gender = PlayerPrefs.GetInt(genderKey);
    }

    public static void SaveServerCode(string code) {
        PlayerPrefs.SetString(serverCodeKey, code);
    }

    public static void SaveName(string name) {
        PlayerPrefs.SetString(nameKey, name);
    }

    public static void SaveGender(Gender gender) {
        PlayerPrefs.SetInt(genderKey, (int)gender);
    }
}
