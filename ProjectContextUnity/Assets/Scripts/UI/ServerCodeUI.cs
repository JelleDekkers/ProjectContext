using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerCodeUI : MonoBehaviour {

	void Start () {
        transform.GetComponentInChildren<Text>().text = "Server Code: " + NetworkManager.serverCode.ToString();
	}
}
