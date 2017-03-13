using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageView : MonoBehaviour {

    private static VillageView instance;
    public static VillageView Instance { get { return instance; } }

    public GameObject viewObject;

    [SerializeField]
    private Transform housesParent;
    [SerializeField]
    private Image characterImage;

    private House selectedHouse;

	void Start () {
        instance = this;
	}

    public void SetupCharacterButton(int charId) {
        characterImage.sprite = CharacterSprites.Instance.Portraits[charId];
    }

    public void AssignHouses(List<int> characters) {
        House[] houses = housesParent.GetComponentsInChildren<House>(true);
        for(int i = 0; i < characters.Count; i++) {
            houses[i].GetComponent<House>().enabled = true;
            houses[i].CharacterIndex = characters[i];
            houses[i].SetPlayerIndex();
        }
    }

    public void SelectHouse(House house) {
        selectedHouse = house;
        HouseInfoPanel.Instance.ShowInfo(house);
    }

    public void DeselectHouse() {
        selectedHouse = null;
    }
}
