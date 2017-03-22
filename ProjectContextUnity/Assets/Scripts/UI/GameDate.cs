using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDate : MonoBehaviour {

    private static GameDate instance;
    public static GameDate Instance { get { return instance; } }

	void Awake () {
        instance = this;
	}
	
    public void SetDate(string date) {
        GetComponent<Text>().text = date;
    }
}
