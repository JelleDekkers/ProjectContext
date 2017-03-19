using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseInfoPanel : MonoBehaviour {

    private static HouseInfoPanel instance;
    public static HouseInfoPanel Instance { get { return instance; } }

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Text occupantText;
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private Image questionMark;

    public bool IsActive { get; private set; }

    private void Awake() {
        instance = this;
    }

    public void ShowInfo(House house) {
        CharactersData character = CharactersDatabase.Instance.Data.dataArray[house.CharacterIndex];
        if (house.CharacterIndex == Player.Instance.CharacterID)
            occupantText.text = "Jij";
        else
            occupantText.text = character.Name;
        portrait.sprite = CharacterSprites.Instance.Portraits[house.CharacterIndex];
        panel.SetActive(true);
        IsActive = true;
    }

    public void Close() {
        panel.SetActive(false);
        IsActive = false;
    }
}
