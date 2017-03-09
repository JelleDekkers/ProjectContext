﻿using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour {

    private static CharacterView instance;
    public static CharacterView Instance { get { return instance; } }

    private CharactersData character;

    public GameObject viewObject;

    [SerializeField]
    private Text characterName;
    [SerializeField]
    private Text characterInfo;
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private Text money;

    private void Awake() {
        instance = this;
        //SetCharacter()
    }

    public void SetCharacter(CharactersData character) {
        characterName.text = character.Name;
        characterInfo.text = character.Backstory;
        characterImage.sprite = CharacterSprites.Instance.Sprites[character.ID];
        UpdateInfo();
    }

    public void UpdateInfo() {
        //money.text = character.Money.ToString();
    }
}
