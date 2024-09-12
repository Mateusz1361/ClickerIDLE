using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeOptionInstance : MonoBehaviour {
    [SerializeField]
    private Image currencyInIcon;
    [SerializeField]
    private Image currencyOutIcon;
    [SerializeField]
    private TMP_Text buyText;
    [SerializeField]
    private TMP_Text sellText;
    [SerializeField]
    private TMP_Text totalBuyAmountText;
    [SerializeField]
    private TMP_Text totalSellAmountText;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button sellButton;
    [SerializeField]
    private Button incrementQuantityButton;
    [SerializeField]
    private Button decrementQuantityButton;
    [SerializeField]
    private TMP_InputField quantityInput;
    [HideInInspector]
    private InventoryMenu inventoryMenu;

    public string CurrencyOut { get; private set; }
    public string CurrencyIn { get; private set; }

    private void Awake() {
        buyButton.onClick.AddListener(OnBuyButtonClick);
        sellButton.onClick.AddListener(OnSellButtonClick);
        incrementQuantityButton.onClick.AddListener(() => {
            Quantity += 1;
        });
        decrementQuantityButton.onClick.AddListener(() => {
            if(Quantity > 0) {
                Quantity -= 1;
            }
        });
        quantityInput.text = "0";
        quantityInput.onEndEdit.AddListener((string text) => {
            Quantity = SafeUInteger.Parse(text);
        });
    }

    public void InitInstance(InventoryMenu _inventoryMenu,TradeOptionInstanceData _data) {
        inventoryMenu = _inventoryMenu;
        currencyInIcon.sprite = inventoryMenu.ItemTemplates[_data.currencyIn].icon;
        currencyOutIcon.sprite = inventoryMenu.ItemTemplates[_data.currencyOut].icon;
        BuyPrice = SafeUDecimal.Parse(_data.buyPrice);
        SellPrice = SafeUDecimal.Parse(_data.sellPrice);
        CurrencyIn = _data.currencyIn;
        CurrencyOut = _data.currencyOut;
        Quantity = 0;
    }

    private SafeUDecimal _buyPrice;
    public SafeUDecimal BuyPrice {
        get {
            return _buyPrice;
        }
        private set {
            _buyPrice = value;
            buyText.text = _buyPrice.ToString();
        }
    }

    private SafeUDecimal _sellPrice;
    public SafeUDecimal SellPrice {
        get {
            return _sellPrice;
        }
        private set {
            _sellPrice = value;
            sellText.text = _sellPrice.ToString();
        }
    }

    private SafeUDecimal _quantity;
    public SafeUDecimal Quantity {
        get {
            return _quantity;
        }
        set {
            _quantity = value;
            quantityInput.text = _quantity.ToString();
            totalBuyAmountText.text = (_quantity * BuyPrice).ToString();
            totalSellAmountText.text = (_quantity * SellPrice).ToString();
        }
    }

    private void OnBuyButtonClick() {
        SafeUDecimal quantity = Quantity;
        if(inventoryMenu.CanRemoveItems(CurrencyOut,quantity * BuyPrice) && inventoryMenu.CanAddItems(CurrencyIn,quantity)) {
            inventoryMenu.RemoveItems(CurrencyOut,quantity * BuyPrice);
            inventoryMenu.AddItems(CurrencyIn,quantity);
        }
    }

    private void OnSellButtonClick() {
        SafeUDecimal quantity = Quantity;
        if(inventoryMenu.CanRemoveItems(CurrencyIn,quantity) && inventoryMenu.CanAddItems(CurrencyOut,quantity * SellPrice)) {
            inventoryMenu.RemoveItems(CurrencyIn,quantity);
            inventoryMenu.AddItems(CurrencyOut,quantity * SellPrice);
        }
    }
}
