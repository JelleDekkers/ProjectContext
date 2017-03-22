using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectHouse : MonoBehaviour {

    private Vector2 mousePos;
    private Collider2D hitCol;

	void Update () {
        if (GameVersion.Instance.Version == Version.Teacher)
            return;

        if (HouseInfoPanel.Instance.IsActive)
            return;

        if (Input.GetMouseButtonDown(0)) {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hitCol = Physics2D.OverlapPoint(mousePos);
            if (hitCol != null && hitCol.GetComponent<House>().enabled) {
                House selectedHouse = hitCol.GetComponent<House>();
                VillageView.Instance.SelectHouse(selectedHouse);
            }
        }
    }
}
