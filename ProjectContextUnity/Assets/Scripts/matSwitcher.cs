using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matSwitcher : MonoBehaviour {

    private SpriteRenderer[] houses;

    public Material outlineMat;

    private void Start() {
        houses = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer r in houses)
            ApplyOutlineMaterial(r);
    }

    private void ApplyOutlineMaterial(SpriteRenderer rend) {
        rend.material = outlineMat;
    }
}
