using UnityEngine;
using UnityEngine.UI;

public class AdMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;

    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
