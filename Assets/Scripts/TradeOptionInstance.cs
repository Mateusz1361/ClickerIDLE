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
        Buy = _data.buy;
        Sell = _data.sell;
        CurrencyIn = _data.currencyIn;
        CurrencyOut = _data.currencyOut;
    }

    private double _buy;
    public double Buy {
        get {
            return _buy;
        }
        private set {
            _buy = value;
            buyText.text = _buy.ToString();
        }
    }

    private double _sell;
    public double Sell {
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
        //@TODO: Implement this.
        /*var temp = double.Parse(quantityOfResource.text) * Buy;
        TotalText.text = temp.ToString();
        
        if (equipmentMenu.Money.Count >= temp)
        {
            
            equipmentMenu.ResourceInstances[CurrencyIn].Count += ulong.Parse(quantityOfResource.text);
            equipmentMenu.Money.Count -= (ulong)temp; 
        }*/
    }

    private void OnSellButtonClick() {
        /*var temp = double.Parse(quantityOfResource.text) * Sell;
        TotalText.text = temp.ToString();
       
        if (equipmentMenu.ResourceInstances[CurrencyIn].Count >= ulong.Parse(quantityOfResource.text))
        {
            
            equipmentMenu.ResourceInstances[CurrencyIn].Count -= ulong.Parse(quantityOfResource.text);
            equipmentMenu.Money.Count += (ulong)temp;
        }*/
    }
}
