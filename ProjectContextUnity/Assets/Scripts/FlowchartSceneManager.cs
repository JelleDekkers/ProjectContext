using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowchartSceneManager : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
            FlowchartHandler.Instance.StartNewEvent();
        }
	}
}
