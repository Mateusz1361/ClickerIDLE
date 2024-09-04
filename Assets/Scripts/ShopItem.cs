using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System.Collections.Generic;

public class ShopItem : MonoBehaviour {
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
    public int indexOfShopItem = 0;
    public Rational multiplier = 1;
    public List<ShopItemPrice> shopItemsPrices;
    
    private ShopItemData defaultSettings;
    public new string name;

    private Rational cacheMainResourceClickIncrement = 0;
    private Rational cacheMainResourceAutoIncrement = 0;

    public Rational MainResourceClickIncrement()
    {
        return cacheMainResourceClickIncrement;
    }
    public Rational MainResourceAutoIncrement()
    {
        return cacheMainResourceAutoIncrement;
    }

    public void RecalculateMainResourceClickIncrement()
    {
        cacheMainResourceClickIncrement = ResultQuantity * Count * multiplier;
    }

    public void RecalculateMainResourceAutoIncrement()
    {
        cacheMainResourceAutoIncrement = ResultQuantity * Count * multiplier;
    }
    
    public string ResultType { get; private set; }

    private ulong _unlockLevel;
    public ulong UnlockLevel {
        get {
            return _unlockLevel;
        }
        private set {
            _unlockLevel = value;
            unlockLevelText.text = $"Unlock at level {NumberFormat.ShortForm(_unlockLevel)}";
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

    public void InitItem(WorldLocation _worldLocation,InventoryMenu _inventoryMenu,ShopItemData data,int index) {
        worldLocation = _worldLocation;
        inventoryMenu = _inventoryMenu;
        UnlockLevel = data.unlockLevel;
        indexOfShopItem = index;
        name = data.name;
        ResultType = data.result.type;
        if(ResultType == "Power") {
            buyItemIcon.sprite = Resources.Load<Sprite>("Images/MineButton");
        }
        else if(ResultType == "Worker") {
            buyItemIcon.sprite = Resources.Load<Sprite>("Images/WorkersButton");
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

    private BigInteger _resultQuantity;
    public BigInteger ResultQuantity {
        get {
            return _resultQuantity;
        }
        set {
            _resultQuantity = value;
            
        }
    }

    public event Action<ulong> OnCountChange;
    private ulong _count;
    public ulong Count {
        get {
            return _count;
        }
        set {
            _count = value;
            OnCountChange?.Invoke(_count);
            buyItemQuantityText.text = NumberFormat.ShortForm(_count);
        }
    }

    private void Awake() {
        purchaseButton.onClick.AddListener(Purchase);
    }

    private void Purchase() {
        bool canAffordPurchase = true;
        foreach(var price in shopItemsPrices) {
            if(price.UnlockCount <= Count) {
                if(inventoryMenu.ResourceInstances[price.Name].Count < price.Value) {
                    canAffordPurchase = false;
                    break;
                }
            }
        }
        if(canAffordPurchase) {
            foreach(var price in shopItemsPrices) {
                if(price.UnlockCount <= Count) {
                    inventoryMenu.ResourceInstances[price.Name].Count -= price.Value;
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
            //ResultQuantity += 1;
        }
    }
}
