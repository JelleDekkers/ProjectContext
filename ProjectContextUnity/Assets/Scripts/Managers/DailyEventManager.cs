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

    public DailyEvent[] Events = new DailyEvent[] {
        new DailyEvent("Duitsland valt aan!",       "14 mei 1940"),
        new DailyEvent("Razzia's naar Joden",       "20 september 1941"),
        new DailyEvent("Avondklok gaat in",         "1 novemeber 1942"),
        new DailyEvent("Hongerwinter slaat toe!",   "2 januari 1943"),
        new DailyEvent("Nederland bevrijdt!",       "14 mei 1945")
    };

    private static DailyEventManager instance;
    public static DailyEventManager Instance { get { return instance; } }
    	
    private void Start() {
        instance = this;
    }

    public void StartDailyEvent(int index) {
        newsPaperCanvas.gameObject.SetActive(true);
        newsText.text = Events[index].Event;
        newsPaperDate.text = Events[index].Date;
    }

    public void CloseNewsPaper() {
        newsPaperCanvas.gameObject.SetActive(false);
        ShowFlowChart();
    }

    private void ShowFlowChart() {
        FlowchartHandler.Instance.StartNewEvent();
    }
}

