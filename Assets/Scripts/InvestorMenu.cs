using UnityEngine;
using UnityEngine.UI;

public class InvestorMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
