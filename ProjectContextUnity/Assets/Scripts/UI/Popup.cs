using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {

    public Text title;
    public Text info;
    public Button btn;

    public void Init(string title, string info) {
        this.title.text = title.ToString();
        this.info.text = info.ToString();
        btn.onClick.AddListener(() => Close());
    }

    public void Close() {
        Destroy(gameObject);
    }
}
