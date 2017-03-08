public enum ConnectionStatus {
    Online,
    Offline
}

/// <summary>
/// Player Data class that holds client information for the host
/// </summary>
[System.Serializable]
public class PlayerData {
    
    /// <summary>
    /// The ID of the player
    /// </summary>
    public int ID;

    public string IpAddress;

    /// <summary>
    /// The real name of the player
    /// </summary>
    public string Name;

    /// <summary>
    /// The ID of the assigned character in the characters sheet.
    /// </summary>
    public int CharID;

    public int ConnectionStatus;

    public PlayerData(string ipAddress, int id, string name, int charId) {
        ID = id;
        IpAddress = ipAddress;
        Name = name;
        CharID = charId;
    }
}