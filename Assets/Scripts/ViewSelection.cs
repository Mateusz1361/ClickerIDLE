using UnityEngine;
using UnityEngine.UI;

public class ViewSelection : MonoBehaviour {
    [SerializeField]
    private GameObject shopMenu;
    [SerializeField]
    private GameObject workerMenu;
    [SerializeField]
    private GameObject tradeMenu;
    [SerializeField]
    private GameObject adMenu;
    [SerializeField]
    private GameObject equipmentMenu;
    [SerializeField]
    private Button shopMenuButton;
    [SerializeField]
    private Button workerMenuButton;
    [SerializeField]
    private Button tradeMenuButton;
    [SerializeField]
    private Button adMenuButton;
    [SerializeField]
    private Button equipmentMenuButton;

    private void Start() {
        SwitchState(null);
        shopMenuButton.onClick.AddListener(SwitchToShopMenu);
        workerMenuButton.onClick.AddListener(SwitchToWorkerMenu);
        tradeMenuButton.onClick.AddListener(SwitchToTradeMenu);
        adMenuButton.onClick.AddListener(SwitchToAdMenu);
        equipmentMenuButton.onClick.AddListener(SwitchToEquipmentMenu);
    }

    public void SwitchToShopMenu() {
        SwitchState(shopMenu);
    }

    public void SwitchToWorkerMenu() {
        SwitchState(workerMenu);
    }

    public void SwitchToTradeMenu() {
        SwitchState(tradeMenu);
    }

    public void SwitchToAdMenu() {
        SwitchState(adMenu);
    }

    public void SwitchToEquipmentMenu() {
        SwitchState(equipmentMenu);
    }

    private void SwitchState(GameObject obj) {
        shopMenu.SetActive(obj == shopMenu);
        workerMenu.SetActive(obj == workerMenu);
        tradeMenu.SetActive(obj == tradeMenu);
        adMenu.SetActive(obj == adMenu);
        equipmentMenu.SetActive(obj == equipmentMenu);
    }
}
