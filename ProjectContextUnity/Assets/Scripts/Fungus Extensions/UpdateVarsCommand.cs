using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CommandInfo("Zuiderhuizen", "Update Variables", "Updates the variables in the game to sync with flowchart")]
public class UpdateVarsCommand : Command {

    public override void OnEnter() {
        FlowchartHandler.Instance.UpdateVariables();
        Continue();
    }

    public override string GetSummary() {
        return "Update variables in script";
    }
}
