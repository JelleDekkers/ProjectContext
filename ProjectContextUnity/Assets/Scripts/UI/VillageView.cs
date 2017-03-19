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
    [SerializeField]
    private GridLayoutGroup healthGroup;
    [SerializeField]
    private Image healthIcon;

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

    public void UpdateInfo() {
        // health:
        Image[] icons = healthGroup.transform.GetComponentsInChildren<Image>();
        foreach (Image s in icons)
            Destroy(s.gameObject);

        for (int i = 0; i < Player.Instance.Health; i++) {
            Image img = Instantiate(healthIcon);
            img.transform.SetParent(healthGroup.transform);
        }
    }
}
