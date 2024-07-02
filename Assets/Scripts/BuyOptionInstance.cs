using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyOptionInstance : MonoBehaviour {
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

    public void InitInstance(MainView _mainView,ulong price,ulong power,int unlockLevel) {
        mainView = _mainView;
        Price = price;
        Power = power;
        UnlockLevel = unlockLevel;
        OnEnable();
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

    private ulong _power;
    public ulong Power {
        get {
            return _power;
        }
        private set {
            _power = value;
            powerText.text = _power.ToString();
        }
    }

    private int _unlockLevel;
    public int UnlockLevel {
        get {
            return _unlockLevel;
        }
        private set {
            _unlockLevel = value;
            unlockAtLevelText.text = $"Unlock at level {_unlockLevel}";
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
        if(mainView.StoneCount >= Price) {
            mainView.StoneCount -= Price;
            mainView.StoneIncrement += Power;
            Quantity += 1;
            Price *= 2;
            OnEnable();
        }
    }
}
