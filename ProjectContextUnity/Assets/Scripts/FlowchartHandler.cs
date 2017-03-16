using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartHandler : MonoBehaviour {

    [SerializeField]
    private GameObject sayDialog;
    [SerializeField]
    private GameObject menuDialog;

    private Flowchart flowChart;
    private string nextBlockName = "Start";
    private bool NewEvent = false;

    public int EventCount = -1;

    private static FlowchartHandler instance;
    public static FlowchartHandler Instance { get { return instance; } }

    void Awake() {
        instance = this;
        Flowchart[] flowcharts = GetComponentsInChildren<Flowchart>();
        flowChart = flowcharts[0];
        //character = GetComponent<CharacterTest>();
        //flowChart.SetIntegerVariable("Money", character.Money);
    }

    public void UpdateVariables() {
        //character.Money = flowChart.GetIntegerVariable("Money");
        //character.HealthPoints = flowChart.GetIntegerVariable("HealthPoints");
    }

    public void StartNewEvent() {
        NewEvent = true;
        sayDialog.gameObject.SetActive(true);
        menuDialog.gameObject.SetActive(true);
        flowChart.ExecuteBlock(nextBlockName);
    }

    public void NewBlockEvent() {
        if (NewEvent) {
            NewEvent = false;
        } else {
            nextBlockName = flowChart.GetExecutingBlocks()[1].BlockName;
            flowChart.StopAllBlocks();
        }
    }
}
