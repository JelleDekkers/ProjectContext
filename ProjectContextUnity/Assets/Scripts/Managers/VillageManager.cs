using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour {

    private static VillageManager instance;
    public static VillageManager Instance { get { return instance; } }

    public GameObject viewObject;

    [SerializeField]
    private Transform housesParent;
    [SerializeField]
    private HouseInfoPanel infoPanel;

	void Start () {
        instance = this;
	}

    public void AssignHouses(List<int> characters) {
        House[] houses = housesParent.GetComponentsInChildren<House>();
        for(int i = 0; i < characters.Count; i++) {
            houses[i].CharacterIndex = characters[i];
            print("house nr: " + i + " gets: " + characters[i]);
        }
    }

    public void SelectHouse(GameObject house) {
        //infoPanel.SetInfo();
        //house sorting layer hoger dan ui?
    }
}
