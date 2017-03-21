using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class FlowchartHandler : MonoBehaviour {

    [SerializeField]
    private GameObject sayDialog;
    [SerializeField]
    private GameObject menuDialog;

    private Flowchart flowchart;
    private string nextBlockName = "Start";
    private bool NewEvent = false;

    public int EventCount = -1;

    private static FlowchartHandler instance;
    public static FlowchartHandler Instance { get { return instance; } }

    void Awake() {
        instance = this;
    }

    public void SetFlowChart(int charId) {
        Flowchart[] flowcharts = GetComponentsInChildren<Flowchart>();
        flowchart = flowcharts[charId];
        print(Player.Instance);
        flowchart.SetIntegerVariable("Money", Player.Instance.Money);
        flowchart.SetIntegerVariable("Health", Player.Instance.Health);
        flowchart.SetIntegerVariable("Status", Player.Instance.Status);
        nextBlockName = Player.Instance.CurrentFlowChartBlock;
    }

    public void UpdateVariables() {
        Player.Instance.Money = flowchart.GetIntegerVariable("Money");
        Player.Instance.Health = flowchart.GetIntegerVariable("Health");
        Player.Instance.Status = flowchart.GetIntegerVariable("Status");
        Player.Instance.SaveData();
        VillageView.Instance.UpdateInfo();
        CharacterView.Instance.UpdateInfo();
    }

    public void StartNewEvent() {
        NewEvent = true;
        sayDialog.gameObject.SetActive(true);
        menuDialog.gameObject.SetActive(true);
        flowchart.ExecuteBlock(nextBlockName);
    }

    public void NewBlockEvent() {
        if (NewEvent) {
            NewEvent = false;
        } else {
            nextBlockName = flowchart.GetExecutingBlocks()[1].BlockName;
            Player.Instance.CurrentFlowChartBlock = nextBlockName;
            Player.Instance.SaveData();
            flowchart.StopAllBlocks();
        }
    }
}
