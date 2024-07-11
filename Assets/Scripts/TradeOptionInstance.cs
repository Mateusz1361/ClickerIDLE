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
    private EquipmentMenu equipmentMenu;

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
            Quantity = ulong.Parse(text);
        });
    }

    public void InitInstance(EquipmentMenu _equipmentMenu,TradeOptionInstanceData _data) {
        equipmentMenu = _equipmentMenu;
        currencyInIcon.sprite = equipmentMenu.ResourceInstances[_data.currencyIn].Icon;
        currencyOutIcon.sprite = equipmentMenu.ResourceInstances[_data.currencyOut].Icon;
        BuyPrice = new(_data.buyPrice);
        SellPrice = new(_data.sellPrice);
        CurrencyIn = _data.currencyIn;
        CurrencyOut = _data.currencyOut;
        Quantity = 0;
    }

    private Rational _buyPrice;
    public Rational BuyPrice {
        get {
            return _buyPrice;
        }
        private set {
            _buyPrice = value;
            buyText.text = _buyPrice.ToString();
        }
    }

    private Rational _sellPrice;
    public Rational SellPrice {
        get {
            return _sellPrice;
        }
        private set {
            _sellPrice = value;
            sellText.text = _sellPrice.ToString();
        }
    }

    private ulong _quantity;
    public ulong Quantity {
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
        Rational quantity = Quantity;
        if(equipmentMenu.ResourceInstances[CurrencyOut].Count >= quantity * BuyPrice) {
            equipmentMenu.ResourceInstances[CurrencyOut].Count -= quantity * BuyPrice;
            equipmentMenu.ResourceInstances[CurrencyIn].Count += quantity;
        }
    }

    private void OnSellButtonClick() {
        Rational quantity = Quantity;
        if(equipmentMenu.ResourceInstances[CurrencyIn].Count >= quantity) {
            equipmentMenu.ResourceInstances[CurrencyIn].Count -= quantity;
            equipmentMenu.ResourceInstances[CurrencyOut].Count += quantity * SellPrice;
        }
    }
}
