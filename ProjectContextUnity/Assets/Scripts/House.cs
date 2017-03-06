using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public int OccupantPlayerIndex;
    public string Straat;
    public int HouseNumber;
    
    public void SetOccupant(int playerIndex) {
        OccupantPlayerIndex = playerIndex;
    }

    public string GetOccupantCharacterName() {
        return "Lambertus";
    }

    public string GetOccupantPlayerName() {
        return "Bob";
    }
}
