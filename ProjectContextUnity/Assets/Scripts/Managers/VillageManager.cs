using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour {

    private static VillageManager instance;
    public static VillageManager Instance { get { return instance; } }

    [SerializeField]
    private Transform housesParent;
    [SerializeField]
    private HouseInfoPanel infoPanel;

	void Start () {
        instance = this;
	}

    public void SetHouses() {
        House[] houses = housesParent.GetComponentsInChildren<House>();
        //foreach player
        //foreach (House h in houses)
        //    h.SetOccupant();
    }

    public void SelectHouse(GameObject house) {
        //infoPanel.SetInfo();
        //house sorting layer hoger dan ui?
    }
}
