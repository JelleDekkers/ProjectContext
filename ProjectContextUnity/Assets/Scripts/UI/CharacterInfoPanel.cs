using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour {

    private static CharacterInfoPanel instance;
    public static CharacterInfoPanel Instance { get { return instance; } }

    [SerializeField]
    private Text header;
    [SerializeField]
    private Text backStory;

    private void Awake() {
        instance = this;
    }

    public void SetInfo(int charId) {
        CharactersData data = CharactersDatabase.Instance.Data.dataArray[charId];
        header.text = data.Name;
        backStory.text = data.Backstory;
    }
}
