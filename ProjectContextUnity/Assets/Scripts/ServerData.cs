using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds Server data and players
/// </summary
[System.Serializable]
public class ServerData {

    public string Code;
    public int GameState;// = -1;
    public List<PlayerData> players; // = new List<PlayerData>();

    private static string fileName = "serverData";
    private static string filePath = "/" + fileName + ".gd";

    public void SaveData() {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + filePath);
        formatter.Serialize(file, this);
        file.Close();
        Debug.Log("Done saving");
    }

    public ServerData LoadData() {
        if (FileExists()) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filePath, FileMode.Open);
            ServerData data = (ServerData)formatter.Deserialize(file);
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
