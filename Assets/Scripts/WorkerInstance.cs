using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WorkerInstance : MonoBehaviour {
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private TMP_Text priceText;
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

    public void InitInstance(MainView _mainView,EquipmentMenu _equipmentMenu,ulong price,ulong power,int unlockLevel) {
        mainView = _mainView;
        equipmentMenu = _equipmentMenu;
        StartPrice = price;
        Price = price;
        Power = power;
        UnlockLevel = unlockLevel;
        Quantity = 0;
        OnEnable();
    }

    public ulong StartPrice { get; private set; }

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

    private ulong _power;
    public ulong Power {
        get {
            return _power;
        }
        private set {
            _power = value;
            powerText.text = NumberFormat.Format(_power);
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

    private ulong Ipow(ulong _base,ulong _power) {
        ulong result = 1;
        for(ulong i = 0;i < _power;i += 1) {
            result *= _base;
        }
        return result;
    }

    private void OnAcceptButtonClick() {
        if(equipmentMenu.Stone.Count >= Price) {
            equipmentMenu.Stone.Count -= Price;
            mainView.AutomaticStoneGain += Power;
            Quantity += 1;
            Price = StartPrice * Ipow(2,(ulong)Quantity);
            OnEnable();
        }
    }
}
