[System.Serializable]
public class PlayerData {
    
    /// <summary>
    /// The ID of the player
    /// </summary>
    public int ID;
    
    /// <summary>
    /// The real name of the player
    /// </summary>
    public string Name;

    public string IpAddress;

    /// <summary>
    /// The ID of the assigned character in the characters sheet.
    /// </summary>
    public int CharID;

    public PlayerData(string ipAddress, int id, string name, int charId) {
        ID = id;
        IpAddress = ipAddress;
        Name = name;
        CharID = charId;
    }
}