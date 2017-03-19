using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManagement : MonoBehaviour {

    private static ScreenManagement instance;
    public static ScreenManagement Instance { get { return instance; } }

    private void Start() {
        instance = this;
    }

	public void ShowCharacterView() {
        CharacterView.Instance.UpdateInfo();
        CharacterView.Instance.viewObject.SetActive(true);
        VillageView.Instance.viewObject.SetActive(false);
    }

    public void CloseCharacterView() {
        CharacterView.Instance.viewObject.SetActive(false);
        VillageView.Instance.viewObject.SetActive(true);
    }
}
