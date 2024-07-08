using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuyOptionInstance : MonoBehaviour {
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private TMP_Text resultTypeText;
    [SerializeField]
    private TMP_Text resultValueText;
    [SerializeField]
    private GameObject unavailabilityMarker;
    [SerializeField]
    private TMP_Text unlockAtLevelText;
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private GameObject boughtAllMarker;
    [SerializeField]
    private GameObject currencyPrefab;
    [SerializeField]
    private GameObject currencyInstanceParent;
    private MainView mainView;
    private EquipmentMenu equipmentMenu;

    public event Action OnQuantityChanged;

    private void Awake() {
        acceptButton.onClick.AddListener(OnAcceptButtonClick);
    }

    private void OnEnable() {
        if(mainView != null) {
            unavailabilityMarker.SetActive(mainView.Level < UnlockLevel);
            acceptButton.interactable = mainView.Level >= UnlockLevel && Quantity < 10;
            boughtAllMarker.SetActive(!unavailabilityMarker.activeSelf && Quantity >= 10);
        }
    }

    public void InitInstance(MainView _mainView,EquipmentMenu _equipmentMenu,BuyOptionInstanceData data) {
        mainView = _mainView;
        equipmentMenu = _equipmentMenu;
        UnlockLevel = data.unlockLevel;
        ResultType = data.result.type;
        ResultValue = data.result.value;

        foreach(var item in data.price) {
            var prefab = Instantiate(currencyPrefab,currencyInstanceParent.transform);
            prefab.GetComponent<BuyOptionCurrencyInstance>().InitInstance(this,item,equipmentMenu.ResourceInstances[item.name].Icon);
            Price.Add(prefab.GetComponent<BuyOptionCurrencyInstance>());
        }
        Quantity = 0;
        OnEnable();
    }

    private BuyOptionResultInstanceData _result;
    public string ResultType {
        get {
            return _result.type;
        }
        private set {
            _result ??= new();
            _result.type = value;
            resultTypeText.text = _result.type;
        }
    }
    public ulong ResultValue {
        get {
            return _result.value;
        }
        private set {
            _result ??= new();
            _result.value = value;
            resultValueText.text = _result.value.ToString();
        }
    }

    private List<BuyOptionCurrencyInstance> _price;
    public List<BuyOptionCurrencyInstance> Price {
        get {
            _price ??= new();
            return _price;
        }
    }

    private int _unlockLevel;
    public int UnlockLevel {
        get {
            return _unlockLevel;
        }
        private set {
            _unlockLevel = value;
            unlockAtLevelText.text = $"Unlocked at level {_unlockLevel}";
        }
    }

    private int _quantity;
    public int Quantity {
        get {
            return _quantity;
        }
        set {
            _quantity = value;
            quantityText.text = _quantity.ToString();
            OnQuantityChanged?.Invoke();
        }
    }

    private void OnAcceptButtonClick() {
        bool canAfford = true;
        foreach(var item in Price) {
            if(Quantity < item.UnlockQuantity) {
                continue;
            }
            if(equipmentMenu.ResourceInstances[item.Name].Count < item.Value) {
                canAfford = false;
                break;
            }
        }
        if(canAfford) {
            foreach(var item in Price) {
                if(Quantity < item.UnlockQuantity) {
                    continue;
                }
                equipmentMenu.ResourceInstances[item.Name].Count -= item.Value;
                item.Value *= 2;
            }
            if(ResultType == "Power") {
                mainView.StoneIncrement += ResultValue;
            }
            else if(ResultType == "Worker") {
                mainView.AutomaticStoneGain += ResultValue;
            }
            Quantity += 1;
        }
    }
}
