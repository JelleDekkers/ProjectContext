using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseInfoPanel : MonoBehaviour {

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Text occupantText;
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private Image questionMark;

    public void SetInfo(int charID) {
        CharactersData character = CharactersDatabase.Instance.Data.dataArray[charID];
        occupantText.text = character.Name;
        portrait.sprite = CharacterSprites.Instance.Portraits[charID];
    }

    public void Show() {
        panel.SetActive(true);
    }

    public void Close() {
        panel.SetActive(false);
    }
}
