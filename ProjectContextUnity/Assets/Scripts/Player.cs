﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

/// <summary>
/// Holds player information for gameplay
/// </summary>
[System.Serializable]
public class Player {

    public string ServerCode;
    public int ID = -1;
    public int CharacterID = -1;
    public string Name;
    public int Gender;
    public List<int> allPlayerChars;
    public int GameState = -1;

    private static string fileName = "gameData";
    private static string filePath = "/" + fileName + ".gd";

    public void SaveData() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filePath);
        formatter.Serialize(file, this);
        file.Close();
        Debug.Log("Done saving");
    }

    public Player LoadData() {
        if (FileExists()) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.Open);
            Player data = (Player)formatter.Deserialize(file);
            file.Close();
            return data;
        } else {
            Debug.Log("Trying to load data but no data file found.");
            return null;
        }
    }

    public static void DeleteData() {
        File.Delete(Application.persistentDataPath + filePath);
    }

    public static bool FileExists() {
        return (File.Exists(Application.persistentDataPath + filePath));
    }
}
