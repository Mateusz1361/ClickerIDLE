using UnityEngine;
using UnityEngine.UI;

public class AdMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
