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
    private GameObject factoryMenu;
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
        shopMenuButton.onClick.AddListener(() =>  SwitchState(shopMenu.activeSelf?null:shopMenu) );
        investorsMenuButton.onClick.AddListener(() => SwitchState(investorsMenu.activeSelf?null:investorsMenu));
        tradeMenuButton.onClick.AddListener(() => SwitchState(tradeMenu.activeSelf?null:tradeMenu));
        adMenuButton.onClick.AddListener(() => SwitchState(adMenu.activeSelf ? null : adMenu));
        inventoryMenuButton.onClick.AddListener(() => SwitchState(inventoryMenu.activeSelf ? null : inventoryMenu));
        worldMenuButton.onClick.AddListener(() => SwitchState(worldMenu.activeSelf ? null : worldMenu));
        factoryMenuButton.onClick.AddListener(() => SwitchState(factoryMenu.activeSelf ? null : factoryMenu));
        investorsMenu.GetComponent<InvestorMenu>().Init();
        shopMenu.GetComponent<ShopMenu>().Init();
        tradeMenu.GetComponent<TradeMenu>().Init();
        adMenu.GetComponent<AdMenu>().Init();
        inventoryMenu.GetComponent<InventoryMenu>().Init();
        worldMenu.GetComponent<WorldMenu>().Init();
        factoryMenu.GetComponent<FactoryMenu>().Init();
    }

    private void SwitchState(GameObject obj) {
        shopMenu.SetActive(obj == shopMenu);
        investorsMenu.SetActive(obj == investorsMenu);
        tradeMenu.SetActive(obj == tradeMenu);
        adMenu.SetActive(obj == adMenu);
        inventoryMenu.SetActive(obj == inventoryMenu);
        worldMenu.SetActive(obj == worldMenu);
        factoryMenu.SetActive(obj == factoryMenu);
    }
}
