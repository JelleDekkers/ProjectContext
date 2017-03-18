using UnityEngine;

public enum Version {
    Student,
    Teacher
}

public class GameVersion : MonoBehaviour {

    public Version Version;

    private static GameVersion instance;
    public static GameVersion Instance { get { return instance; } }

    void Awake() {
        instance = this;
    }
}
