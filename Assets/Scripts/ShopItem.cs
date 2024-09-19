using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopItem : MonoBehaviour {
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Image buyItemIcon;
    [SerializeField]
    private TMP_Text buyItemQuantityText;
    [SerializeField]
    private GameObject shopItemPricePrefab;
    [SerializeField]
    private RectTransform shopItemPriceContent;
    [SerializeField]
    private Button purchaseButton;
    [SerializeField]
    private GameObject unlockMarker;
    [SerializeField]
    private TMP_Text unlockLevelText;
    private WorldLocation worldLocation;
    private InventoryMenu inventoryMenu;
    public SafeUDecimal multiplier = 1;
    [SerializeField]
    private ReferenceHub referenceHub;
    [HideInInspector]
    public List<ShopItemPrice> shopItemsPrices;
    
    private ShopItemData defaultSettings;
    [HideInInspector]
    public new string name;

    private SafeUDecimal cacheMainResourceClickIncrement = 0;
    private SafeUDecimal cacheMainResourceAutoIncrement = 0;

    public SafeUDecimal MainResourceClickIncrement() {
        return cacheMainResourceClickIncrement;
    }

    public SafeUDecimal MainResourceAutoIncrement() {
        return cacheMainResourceAutoIncrement;
    }

    public void RecalculateMainResourceClickIncrement() {
        cacheMainResourceClickIncrement = (ResultType == "Power") ? ResultQuantity * Count * multiplier : 0;
    }

    public void RecalculateMainResourceAutoIncrement() {
        cacheMainResourceAutoIncrement = (ResultType == "Worker") ? ResultQuantity * Count * multiplier : 0;
    }
    
    public string ResultType { get; private set; }

    private ulong _unlockLevel = 0;
    public ulong UnlockLevel {
        get {
            return _unlockLevel;
        }
        private set {
            _unlockLevel = value;
            unlockLevelText.text = $"Unlock at level {_unlockLevel}";
        }
    }

    private void OnEnable() {
        if(worldLocation != null) {
            unlockMarker.SetActive(worldLocation.Level < UnlockLevel);
            purchaseButton.interactable = !unlockMarker.activeSelf;
        }
    }

    public void ResetItem() {
        ResultQuantity = defaultSettings.result.value;
        Count = 0;
        foreach(var price in shopItemsPrices) {
            price.ResetPrice();
        }
        RecalculateMainResourceAutoIncrement();
        RecalculateMainResourceClickIncrement();
        worldLocation.RecalculateMainResourceAutoIncrement();
        worldLocation.RecalculateMainResourceClickIncrement();
    }

    public void InitItem(WorldLocation _worldLocation,ShopItemData data,ReferenceHub _referenceHub) {
        worldLocation = _worldLocation;
        inventoryMenu = _referenceHub.inventoryMenu;
        referenceHub = _referenceHub;
        UnlockLevel = data.unlockLevel;
        name = data.name;
        nameText.text = name;
        ResultType = data.result.type;

        if(ResultType == "Power") {
            buyItemIcon.sprite = Resources.Load<Sprite>("Images/MineButton");
        }
        else if(ResultType == "Worker") {
            buyItemIcon.sprite = Resources.Load<Sprite>("Images/WorkersButton");
        }
        else if (ResultType == "Improvement")
        {
            buyItemIcon.sprite = Resources.Load<Sprite>("Images/ItemIcons/Dynamite");
        }
        else {
            buyItemIcon.sprite = inventoryMenu.ItemTemplates[ResultType].icon;
        }

        ResultQuantity = data.result.value;
        shopItemsPrices = new();
        foreach(var shopItemPriceData in data.price) {
            var prefab = Instantiate(shopItemPricePrefab,shopItemPriceContent);
            var component = prefab.GetComponent<ShopItemPrice>();
            component.InitPrice(inventoryMenu,this,shopItemPriceData);
            shopItemsPrices.Add(component);
        }
        defaultSettings = data;
    }

    private SafeUInteger _resultQuantity = 0;
    public SafeUInteger ResultQuantity {
        get {
            return _resultQuantity;
        }
        set {
            _resultQuantity = value;
            buyItemQuantityText.text = _resultQuantity.ToString();
        }
    }

    public event Action<SafeUInteger> OnCountChange;
    private SafeUInteger _count = 0;
    public SafeUInteger Count {
        get {
            return _count;
        }
        set {
            _count = value;
            OnCountChange?.Invoke(_count);
            if (ResultType == "Improvement")
            {
                gameObject.SetActive(_count == 0);
            }
            
        }
    }

    private void Awake() {
        purchaseButton.onClick.AddListener(Purchase);
    }

    private void Purchase() {
        bool canAffordPurchase = true;
        foreach(var price in shopItemsPrices) {
            if(price.UnlockCount <= Count) {
                if(!inventoryMenu.CanRemoveItems(price.Name,price.Value)) {
                    canAffordPurchase = false;
                    break;
                }
            }
        }
        if(canAffordPurchase) {
            foreach(var price in shopItemsPrices) {
                if(price.UnlockCount <= Count) {
                    inventoryMenu.RemoveItems(price.Name,price.Value);
                    price.Value *= 2;
                }
            }
            Count += 1;
            if(ResultType == "Power") {
                RecalculateMainResourceClickIncrement();
                worldLocation.RecalculateMainResourceClickIncrement();
            }
            else if(ResultType == "Worker") {
                RecalculateMainResourceAutoIncrement();
                worldLocation.RecalculateMainResourceAutoIncrement();
            }
            else if (ResultType == "Improvement")
            {
                gameObject.SetActive(false);
            }
            else {
                inventoryMenu.AddItems(ResultType,ResultQuantity);
            }
        }
    }

    
}
