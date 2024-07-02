using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradeOptionInstance : MonoBehaviour {
    [SerializeField]
    private Image currencyInIcon;
    [SerializeField]
    private Image currencyOutIcon;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private TMP_Text gainText;
    [SerializeField]
    private Button acceptButton;
    [HideInInspector]
    private EquipmentMenu equipmentMenu;

    public string CurrencyIn { get; private set; }
    public string CurrencyOut { get; private set; }

    private void Awake() {
        acceptButton.onClick.AddListener(OnAcceptButtonClick);
    }

    public void InitInstance(EquipmentMenu _equipmentMenu,TradeOptionInstanceData _data) {
        equipmentMenu = _equipmentMenu;
        currencyInIcon.sprite = equipmentMenu.ResourceInstances[_data.currencyIn].Icon;
        currencyOutIcon.sprite = equipmentMenu.ResourceInstances[_data.currencyOut].Icon;
        Price = _data.price;
        Gain = _data.gain;
        CurrencyIn = _data.currencyIn;
        CurrencyOut = _data.currencyOut;
    }

    private ulong _price;
    public ulong Price {
        get {
            return _price;
        }
        private set {
            _price = value;
            priceText.text = NumberFormat.Format(_price);
        }
    }

    private ulong _gain;
    public ulong Gain {
        get {
            return _gain;
        }
        private set {
            _gain = value;
            gainText.text = NumberFormat.Format(_gain);
        }
    }

    private void OnAcceptButtonClick() {
        bool priceTaken = false;
        foreach((var resourceInstanceName,var resourceInstance) in equipmentMenu.ResourceInstances) {
            if(CurrencyIn.Equals(resourceInstanceName) && Price <= resourceInstance.Count) {
                resourceInstance.Count -= Price;
                priceTaken = true;
                break;
            }
        }
        if(priceTaken) {
            foreach((var resourceInstanceName,var resourceInstance) in equipmentMenu.ResourceInstances) {
                if(CurrencyOut.Equals(resourceInstanceName)) {
                    resourceInstance.Count += Gain;
                    break;
                }
            }
        }
    }
}
