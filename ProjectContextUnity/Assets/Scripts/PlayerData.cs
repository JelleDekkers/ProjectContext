[System.Serializable]
public class PlayerData {

    public int ID;
    public string Name;
    public string IpAddress;
    public int CharID;

    public PlayerData(string ipAddress, int id, string name, int charId) {
        ID = id;
        IpAddress = ipAddress;
        Name = name;
        CharID = charId;
    }
}