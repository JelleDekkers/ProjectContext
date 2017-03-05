using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprites : MonoBehaviour {

    private static CharacterSprites instance;
    public static CharacterSprites Instance { get { return instance; } }

    public Sprite[] sprites;

    private void Start() {
        instance = this;
    }
}
