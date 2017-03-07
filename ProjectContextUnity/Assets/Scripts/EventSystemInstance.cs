using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemInstance : MonoBehaviour {

    private static EventSystemInstance instance;
    public static EventSystemInstance Instance { get { return instance; } }

    void Awake() {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
