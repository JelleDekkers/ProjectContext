using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour {

    private static PopupManager instance;
    public static PopupManager Instance { get { return instance; } }

    [SerializeField]
    private Popup popupPrefab;

	void Start () {
        instance = this;
	}
	
    public void ShowPopup(string title, string info) {
        Popup msg = Instantiate(instance.popupPrefab) as Popup;
        msg.Init(title, info);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            ShowPopup("Test", "test");
        }
    }
}
