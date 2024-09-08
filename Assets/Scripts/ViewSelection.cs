using UnityEngine;
using UnityEngine.UI;

public class ViewSelection : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
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
    [SerializeField]
    private Button factoryMenuButton;

    public void Init() {
        SwitchState(null);
        shopMenuButton.onClick.AddListener(() =>  SwitchState(referenceHub.shopMenu.gameObject.activeSelf ? null : referenceHub.shopMenu.gameObject));
        investorsMenuButton.onClick.AddListener(() => SwitchState(referenceHub.investorMenu.gameObject.activeSelf ? null : referenceHub.investorMenu.gameObject));
        tradeMenuButton.onClick.AddListener(() => SwitchState(referenceHub.tradeMenu.gameObject.activeSelf ? null : referenceHub.tradeMenu.gameObject));
        adMenuButton.onClick.AddListener(() => SwitchState(referenceHub.adMenu.gameObject.activeSelf ? null : referenceHub.adMenu.gameObject));
        inventoryMenuButton.onClick.AddListener(() => SwitchState(referenceHub.inventoryMenu.gameObject.activeSelf ? null : referenceHub.inventoryMenu.gameObject));
        worldMenuButton.onClick.AddListener(() => SwitchState(referenceHub.worldMenu.gameObject.activeSelf ? null : referenceHub.worldMenu.gameObject));
        factoryMenuButton.onClick.AddListener(() => SwitchState(referenceHub.factoryMenu.gameObject.activeSelf ? null : referenceHub.factoryMenu.gameObject));
        referenceHub.investorMenu.Init();
        referenceHub.shopMenu.Init();
        referenceHub.tradeMenu.Init();
        referenceHub.adMenu.Init();
        referenceHub.inventoryMenu.Init();
        referenceHub.worldMenu.Init();
        referenceHub.factoryMenu.Init();
    }

    private void SwitchState(GameObject obj) {
        referenceHub.shopMenu.gameObject.SetActive(obj == referenceHub.shopMenu.gameObject);
        referenceHub.investorMenu.gameObject.SetActive(obj == referenceHub.investorMenu.gameObject);
        referenceHub.tradeMenu.gameObject.SetActive(obj == referenceHub.tradeMenu.gameObject);
        referenceHub.adMenu.gameObject.SetActive(obj == referenceHub.adMenu.gameObject);
        referenceHub.inventoryMenu.gameObject.SetActive(obj == referenceHub.inventoryMenu.gameObject);
        referenceHub.worldMenu.gameObject.SetActive(obj == referenceHub.worldMenu.gameObject);
        referenceHub.factoryMenu.gameObject.SetActive(obj == referenceHub.factoryMenu.gameObject);
    }
}
