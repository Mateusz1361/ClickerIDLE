using UnityEngine;
using UnityEngine.UI;

public class FactoryItem : MonoBehaviour {
    [SerializeField]
    private Image iconOfFactory;
    [SerializeField]
    private Button startProcessButton;
    [SerializeField]
    private Slider progressSlider; 
    [SerializeField] 
    private Transform parentOfPrices;
    [SerializeField]
    private GameObject factoryPricePrefab;
    private InventoryMenu inventoryMenu;
    private float duration = 5.0f;
    private float elapsed = 0.0f;
    private bool isUpdating = false;
    private FactoryResultData factoryResultData;

    private void Awake() {
        startProcessButton.onClick.AddListener(StartFactory);
    }

    public void Init(FactoryItemData factoryItemData,InventoryMenu _inventoryMenu) {
        inventoryMenu = _inventoryMenu;
        progressSlider.value = 0;
        foreach(var price in factoryItemData.price) {
            var prefab = Instantiate(factoryPricePrefab,parentOfPrices);
            var component  = prefab.GetComponent<ShopItemPrice>();
            component.InitPrice(inventoryMenu,null,new() { name = price.name, unlockCount = 0, value = price.value });
            iconOfFactory.sprite = Resources.Load<Sprite>("Images/FactoryButton");
        }
        factoryResultData = factoryItemData.result;
    }

    public void StartFactory() {
        var children = parentOfPrices.GetComponentsInChildren<ShopItemPrice>();
        bool canAfford = true;
        foreach(var price in children) {
            if(price.Value > inventoryMenu.ResourceInstances[price.Name].Count) {
                canAfford = false;
                break;
            }
        }
        if(canAfford) {
            foreach(var price in children) {
                inventoryMenu.ResourceInstances[price.Name].Count -= price.Value;
            }
            isUpdating = true;
            elapsed = 0.0f;
            startProcessButton.interactable = false;
        }
    }

    public void UpdateFactory() {
        if(isUpdating) {
            elapsed += Time.deltaTime;
            progressSlider.value = Mathf.Clamp01(elapsed / duration);
            if(elapsed >= duration) {
                progressSlider.value = 0.0f;
                isUpdating = false;
                startProcessButton.interactable = true;
                inventoryMenu.AddItems(factoryResultData.type,(uint)factoryResultData.value);
            }
        }
    }
}
