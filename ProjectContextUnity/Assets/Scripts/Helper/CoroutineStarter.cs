
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CoroutineStarter : MonoBehaviour{
    private static CoroutineStarter _instance;

    public static CoroutineStarter Instance {
        get {
            if (_instance == null) {
                if (!Application.isPlaying)
                    return null;

                _instance = GameObject.FindObjectOfType<CoroutineStarter>();
                if (_instance == null) {
                    GameObject go = new GameObject("CoroutineStarter");
                    GameObject.DontDestroyOnLoad(go);
                    _instance = go.AddComponent<CoroutineStarter>();
                }
            }
            return _instance;
        }
    }
}