using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersDatabase : MonoBehaviour {

    private static CharactersDatabase instance;
    public static CharactersDatabase Instance { get { return instance; } }

    public Characters Data;

    private void Start() {
        instance = this;
    }
}
