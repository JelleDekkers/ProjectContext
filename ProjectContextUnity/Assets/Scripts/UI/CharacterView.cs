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
    private GridLayoutGroup moneyGroup;
    [SerializeField]
    private Image moneyIcon;
    [SerializeField]
    private GridLayoutGroup healthGroup;
    [SerializeField]
    private Image healthIcon;

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

        // health:
        Image[] icons = healthGroup.transform.GetComponentsInChildren<Image>();
        foreach (Image s in icons)
            Destroy(s.gameObject);

        for(int i = 0; i < Player.Instance.Health; i++) {
            Image img = Instantiate(healthIcon);
            img.transform.SetParent(healthGroup.transform);
        }

        // money:
        icons = moneyGroup.transform.GetComponentsInChildren<Image>();
        foreach (Image s in icons)
            Destroy(s.gameObject);

        for (int i = 0; i < Player.Instance.Money; i++) {
            Image img = Instantiate(moneyIcon);
            img.transform.SetParent(moneyGroup.transform);
        }

    }
}
