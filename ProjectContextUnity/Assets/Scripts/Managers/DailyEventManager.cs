using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyEventManager : MonoBehaviour {

    [SerializeField]
    private Canvas newsPaperCanvas;
    [SerializeField]
    private Text newsText;
    [SerializeField]
    private Text newsPaperDate;

    private static DailyEventManager instance;
    public static DailyEventManager Instance { get { return instance; } }
    	
    private void Start() {
        instance = this;
    }

    public void StartNextEvent() {
        newsPaperCanvas.gameObject.SetActive(true);
        //newsText
        //date
    }

    public void CloseNewsPaper() {
        newsPaperCanvas.gameObject.SetActive(false);
        ShowFlowChart();
    }

    private void ShowFlowChart() {
        FlowchartHandler.Instance.StartNewEvent();
    }
}
