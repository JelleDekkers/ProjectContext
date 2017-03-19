using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour {

    [SerializeField]
    private Image img;
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Image onlineImg;
    [SerializeField]
    private Text charName;

    [SerializeField]
    private Color onlineColor;
    [SerializeField]
    private Color offlineColor;

	public void UpdateInfo(PlayerData data) {
        playerName.text = data.Name;

        if (data.ConnectionStatus == (int)ConnectionStatus.Online)
            onlineImg.color = onlineColor;
        else
            onlineImg.color = offlineColor;

        if (data.CharID != -1) {
            img.gameObject.SetActive(true);
            charName.gameObject.SetActive(true);
            img.sprite = CharacterSprites.Instance.Sprites[data.CharID];
            charName.text = CharactersDatabase.Instance.Data.dataArray[data.CharID].Name;
        } else {
            img.gameObject.SetActive(false);
            charName.gameObject.SetActive(false);
        }
    }
}
