using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTest : MonoBehaviour {

    [SerializeField]
    private Characters data;

    private void Start() {
        foreach(CharactersData d in data.dataArray) {
            print(d.ID + " " + d.Name);
        }
    }
}
