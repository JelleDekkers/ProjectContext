using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour {

    [SerializeField]
    private GameObject statusObject;
    [SerializeField]
    private Slider statusSlider;

    private static StatusView instance;
    public static StatusView Instance { get { return instance; } }

    private void Start() {
        instance = this;
    }

    public void Show() {
        statusObject.SetActive(true);
        //status normalizen en op slider toepassen
        statusSlider.value = GetNormalizedStatus(Player.Instance.Status);
    }

    public float GetNormalizedStatus(int status) {
        float min = -5;
        float max = 5;
        float normalized = 0.5f;
        normalized = (status - min) / (max - min);
        return normalized;
    }
}
