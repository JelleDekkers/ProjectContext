using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFlowchartForTesting : MonoBehaviour {

	void Awake () {
        FlowchartHandler handler = GetComponent<FlowchartHandler>();
        Player player = new Player();
        player.SetInstance();
        player.Money = 3;
        player.Status = 0;
        handler.SetFlowChart(0);
	}
	
	
}
