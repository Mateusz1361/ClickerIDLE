using UnityEngine;
using UnityEngine.UI;

public class ViewSelection : MonoBehaviour {
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private GameObject investorsMenu;
    [SerializeField]
    private GameObject tradeMenu;
    [SerializeField]
    private GameObject adMenu;
    [SerializeField]
    private GameObject inventoryMenu;
    [SerializeField]
    private GameObject worldMenu;
    [SerializeField]
    private Button shopMenuButton;
    [SerializeField]
    private Button investorsMenuButton;
    [SerializeField]
    private Button tradeMenuButton;
    [SerializeField]
    private Button adMenuButton;
    [SerializeField]
    private Button inventoryMenuButton;
    [SerializeField]
    private Button worldMenuButton;

    private void Start() {
        SwitchState(null);
        shopMenuButton.onClick.AddListener(() => SwitchState(shopMenu));
        investorsMenuButton.onClick.AddListener(() => SwitchState(investorsMenu));
        tradeMenuButton.onClick.AddListener(() => SwitchState(tradeMenu));
        adMenuButton.onClick.AddListener(() => SwitchState(adMenu));
        inventoryMenuButton.onClick.AddListener(() => SwitchState(inventoryMenu));
        worldMenuButton.onClick.AddListener(() => SwitchState(worldMenu));
    }

    private void SwitchState(GameObject obj) {
        shopMenu.SetActive(obj == shopMenu);
        investorsMenu.SetActive(obj == investorsMenu);
        tradeMenu.SetActive(obj == tradeMenu);
        adMenu.SetActive(obj == adMenu);
        inventoryMenu.SetActive(obj == inventoryMenu);
        worldMenu.SetActive(obj == worldMenu);
    }
}
