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
    private MainView mainView;
    [HideInInspector]
    private EquipmentMenu equipmentMenu;

    public string CurrencyIn { get; private set; }
    public string CurrencyOut { get; private set; }

    private void Awake() {
        acceptButton.onClick.AddListener(OnAcceptButtonClick);
    }

    public void InitInstance(MainView _mainView,EquipmentMenu _equipmentMenu,Sprite currencyInSprite,Sprite currencyOutSprite,ulong price,ulong gain,string currencyIn,string currencyOut) {
        mainView = _mainView;
        equipmentMenu = _equipmentMenu;
        currencyInIcon.sprite = currencyInSprite;
        currencyOutIcon.sprite = currencyOutSprite;
        Price = price;
        Gain = gain;
        CurrencyIn = currencyIn;
        CurrencyOut = currencyOut;
    }

    private ulong _price;
    public ulong Price {
        get {
            return _price;
        }
        private set {
            _price = value;
            priceText.text = _price.ToString();
        }
    }

    private ulong _gain;
    public ulong Gain {
        get {
            return _gain;
        }
        private set {
            _gain = value;
            gainText.text = _gain.ToString();
        }
    }

    private void OnAcceptButtonClick() {
        bool priceTaken = false;
        if(CurrencyIn.Equals("Stone") && Price <= mainView.StoneCount) {
            mainView.StoneCount -= Price;
            priceTaken = true;
        }
        else if(CurrencyIn.Equals("Dollar") && Price <= mainView.DollarCount) {
            mainView.DollarCount -= Price;
            priceTaken = true;
        }
        else {
            foreach((var oreInstanceName,var oreInstance) in equipmentMenu.OreInstances) {
                if(CurrencyIn.Equals(oreInstanceName) && Price <= oreInstance.Count) {
                    oreInstance.Count -= Price;
                    priceTaken = true;
                    break;
                }
            }
        }
        if(priceTaken) {
            if(CurrencyOut.Equals("Stone")) {
                mainView.StoneCount += Gain;
            }
            else if(CurrencyOut.Equals("Dollar")) {
                mainView.DollarCount += Gain;
            }
            else {
                foreach((var oreInstanceName,var oreInstance) in equipmentMenu.OreInstances) {
                    if(CurrencyOut.Equals(oreInstanceName)) {
                        oreInstance.Count += Gain;
                        break;
                    }
                }
            }
        }
    }
}
