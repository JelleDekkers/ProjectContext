using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManagement : MonoBehaviour {

    private static ScreenManagement instance;
    public static ScreenManagement Instance { get { return instance; } }

    [SerializeField]
    private GameObject loadingView;

    private void Start() {
        instance = this;
    }

	public void ShowCharacterView() {
        CharacterView.Instance.UpdateInfo();
        CharacterView.Instance.viewObject.SetActive(true);
        VillageManager.Instance.viewObject.SetActive(false);
    }

    public void CloseCharacaterView() {
        CharacterView.Instance.viewObject.SetActive(false);
        VillageManager.Instance.viewObject.SetActive(true);
    }
}
