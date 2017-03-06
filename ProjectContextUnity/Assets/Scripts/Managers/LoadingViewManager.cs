using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingViewManager : MonoBehaviour {

    private static LoadingViewManager instance;
    public static LoadingViewManager Instance { get { return instance; } }

    [SerializeField]
    private GameObject loadingView;
    [SerializeField]
    private Text title;
    [SerializeField]
    private GameObject icon;

    private float iconRotationSpeed = 200;
    private Canvas canvas;

	void Start () {
        instance = this;
        canvas = GetComponent<Canvas>();
	}

    public void Hide() {
        canvas.enabled = false;
    }

    public void Show(string titleText) {
        canvas.enabled = true;
        title.text = titleText;
    }

    private void Update() {
        icon.transform.Rotate(-Vector3.forward * Time.deltaTime * iconRotationSpeed);
    }
}
