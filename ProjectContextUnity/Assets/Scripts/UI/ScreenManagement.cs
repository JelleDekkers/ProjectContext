using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManagement : MonoBehaviour {

    private static ScreenManagement instance;
    public static ScreenManagement Instance { get { return instance; } }

    [SerializeField]
    private GameObject loadingView;
    [SerializeField]
    private GameObject characterView;
    [SerializeField]
    private GameObject villageView;

    private void Start() {
        instance = this;
    }

	public void ShowCharacterView() {
        CharacterView.Instance.UpdateInfo();
        characterView.SetActive(true);
        villageView.SetActive(false);
    }

    public void CloseCharacaterView() {
        villageView.SetActive(true);
        characterView.SetActive(false);
    }
}
