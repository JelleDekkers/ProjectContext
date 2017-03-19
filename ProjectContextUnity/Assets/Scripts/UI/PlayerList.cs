using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

    [SerializeField]
    private GridLayoutGroup group;
    [SerializeField]
    private PlayerListItem itemPrefab;

    private static PlayerList instance;
    public static PlayerList Instance { get { return instance; } }

    private void Awake() {
        instance = this;
    }

    void Start () {
        group.cellSize = new Vector2(group.GetComponent<RectTransform>().rect.width, group.cellSize.y);
	}
	
	public void UpdateList(List<PlayerData> players) {
        EmptyGroup();

        foreach(PlayerData player in players) {
            PlayerListItem item = Instantiate(itemPrefab);
            item.transform.SetParent(group.transform);
            item.UpdateInfo(player);
        }
    }

    private void EmptyGroup() {
        PlayerListItem[] items = group.GetComponentsInChildren<PlayerListItem>();
        foreach(PlayerListItem item in items) {
            Destroy(item.gameObject);
        }
    }

    public void OnDrag() {
        transform.position = Input.mousePosition;
    }
}
