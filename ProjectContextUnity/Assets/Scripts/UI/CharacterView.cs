using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour {

    private static CharacterView instance;
    public static CharacterView Instance { get { return instance; } }

    public GameObject viewObject;

    private CharactersData character;

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
    }

    public void SetCharacter(CharactersData character) {
        this.character = character;
        characterName.text = character.Name;
        characterInfo.text = character.Backstory;
        characterImage.sprite = CharacterSprites.Instance.Sprites[character.ID];
        UpdateInfo();
    }

    public void UpdateInfo() {
        if (character == null)
            SetCharacter(CharactersDatabase.Instance.Data.dataArray[Player.Instance.CharacterID]);

        //money.text = Player.Instance.Money.ToString();
    }
}
