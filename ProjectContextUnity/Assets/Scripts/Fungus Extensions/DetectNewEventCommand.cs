using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Zuiderhuizen", "Detect New Event", "Detects a new event, and stops the flowchart")]
public class DetectNewEventCommand : Command {

    public override void OnEnter() {
        FlowchartHandler.Instance.NewBlockEvent();
        Continue();
    }

    public override string GetSummary() {
        return "Stops flowchart with new event";
    }
}
