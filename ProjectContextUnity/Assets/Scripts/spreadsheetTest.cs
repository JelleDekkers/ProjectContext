using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spreadsheetTest : MonoBehaviour {

    public Characters data;

	void Start () {
        foreach(CharactersData d in data.dataArray) {
            print(d.Name);
        }
	}
	
}
