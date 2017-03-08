using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {
    
    /// <summary>
    /// Index number in character sheet
    /// </summary>
    public int CharacterIndex;

    /// <summary>
    /// Index nr of player living here, will be the same as parent child index
    /// </summary>
    public int PlayerIndex;

    public string StreetName;
    public int HouseNumber;

    private void Start() {
        PlayerIndex = transform.GetSiblingIndex();
    }
    
    public void SetOccupant(int playerIndex) {
        CharacterIndex = playerIndex;
    }

    public string GetOccupantCharacterName() {
        return "Lambertus";
    }

    public string GetOccupantPlayerName() {
        return "Bob";
    }
}
