using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuyOptionInstance : MonoBehaviour {
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private TMP_Text powerText;
    [SerializeField]
    private GameObject unavailabilityMarker;
    [SerializeField]
    private TMP_Text unlockAtLevelText;
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private GameObject boughtAllMarker;
    [SerializeField]
    private GameObject currenciesPrefab;
    [SerializeField]
    private GameObject parentCurrencies;
    [HideInInspector]
    private MainView mainView;
    [HideInInspector]
    private EquipmentMenu equipmentMenu;

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
        Power = data.power;
        UnlockLevel = data.unlockLevel;
        
        foreach(var item in data.price) {
            var prefab = Instantiate(currenciesPrefab,parentCurrencies.transform);
            prefab.GetComponent<BuyOptionCurrencyInstance>().InitInstance(item.name,item.value,equipmentMenu.ResourceInstances[item.name].Icon);
            Price.Add(prefab.GetComponent<BuyOptionCurrencyInstance>());
        }
        OnEnable();
    }
    
    private List<BuyOptionCurrencyInstance> _price;
    public List<BuyOptionCurrencyInstance> Price {
        get {
            _price ??= new();
            return _price;
        }
    }

    private ulong _power;
    public ulong Power {
        get {
            return _power;
        }
        private set {
            _power = value;
            powerText.text = NumberFormat.ShortForm(_power);
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
        }
    }

    private void OnAcceptButtonClick() {
        bool canAfford = true;
        foreach(var item in Price) {
            if(equipmentMenu.ResourceInstances[item.Name].Count < item.Value) {
                canAfford = false;
                break;
            }
        }
        if(canAfford) {
            foreach(var item in Price) {
                equipmentMenu.ResourceInstances[item.Name].Count -= item.Value;
                item.Value *= 2;
            }
            mainView.StoneIncrement += Power;
        }
    }
}
