using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour {

    private static CharacterView instance;
    public static CharacterView Instance { get { return instance; } }

    private CharactersData character;

    [SerializeField]
    private Text characterName;
    [SerializeField]
    private Text characterInfo;
    [SerializeField]
    private Image characterImage;
    [SerializeField]
    private Text money;

    private void Start() {
        instance = this;
        //SetCharacter()
    }

    public void SetCharacter(CharactersData character) {
        characterName.text = character.Name;
        characterInfo.text = character.Backstory;
        characterImage.sprite = CharacterSprites.Instance.sprites[character.ID];
        UpdateInfo();
    }

    public void UpdateInfo() {
        money.text = character.Money.ToString();
    }
}
