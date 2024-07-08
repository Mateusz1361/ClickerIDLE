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
    private TMP_Text TotalText;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Button sellButton;
    [SerializeField]
    private Button addOneButton;
    [SerializeField]
    private Button removeOneButton;
    [SerializeField]
    private TMP_InputField quantityOfResource;
    [HideInInspector]
    private EquipmentMenu equipmentMenu;

    public string CurrencyOut { get; private set; }
    public string CurrencyIn { get; private set; }

    private void Awake() {
        buyButton.onClick.AddListener(OnBuyButtonClick);
        sellButton.onClick.AddListener(OnSellButtonClick);
        addOneButton.onClick.AddListener(AddingOne);
        removeOneButton.onClick.AddListener(RemovingOne);
        quantityOfResource.text = "0";
    }

    public void InitInstance(EquipmentMenu _equipmentMenu,TradeOptionInstanceData _data) {
        equipmentMenu = _equipmentMenu;
        currencyInIcon.sprite = equipmentMenu.ResourceInstances[_data.currencyIn].Icon;
        currencyOutIcon.sprite = equipmentMenu.ResourceInstances[_data.currencyOut].Icon;
        Buy = new(_data.buy);
        Sell = new(_data.sell);
        CurrencyIn = _data.currencyIn;
        CurrencyOut = _data.currencyOut;
    }

    private Rational _buy;
    public Rational Buy {
        get {
            return _buy;
        }
        private set {
            _buy = value;
            buyText.text = _buy.ToString();
        }
    }

    private Rational _sell;
    public Rational Sell {
        get {
            return _sell;
        }
        private set {
            _sell = value;
            sellText.text = _sell.ToString();
        }
    }

    private void AddingOne() {
        var temp = ulong.Parse(quantityOfResource.text) + 1;
        quantityOfResource.text = temp.ToString();
    }

    private void RemovingOne() {
        var value = ulong.Parse(quantityOfResource.text);
        if(value > 0) {
            quantityOfResource.text = (value - 1).ToString();
        }
    }

    private void OnBuyButtonClick() {
        var quantity = Rational.Parse(quantityOfResource.text);
        var tmp = quantity * Buy;
        TotalText.text = tmp.ToString();

        if(equipmentMenu.ResourceInstances[CurrencyOut].Count >= quantity) {
            equipmentMenu.ResourceInstances[CurrencyIn].Count += quantity;
            equipmentMenu.ResourceInstances[CurrencyOut].Count -= tmp;
        }
    }

    private void OnSellButtonClick() {
        var quantity = Rational.Parse(quantityOfResource.text);
        var tmp = quantity * Sell;
        TotalText.text = tmp.ToString();

        if(equipmentMenu.ResourceInstances[CurrencyIn].Count >= quantity) {
            equipmentMenu.ResourceInstances[CurrencyIn].Count -= quantity;
            equipmentMenu.ResourceInstances[CurrencyOut].Count += tmp;
        }
    }
}
