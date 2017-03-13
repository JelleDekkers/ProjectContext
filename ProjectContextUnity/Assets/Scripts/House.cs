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

    public void SetPlayerIndex() {
        PlayerIndex = transform.GetSiblingIndex();
    }

    private void Update() {

    }
}
