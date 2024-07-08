using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyOptionCurrencyInstance : MonoBehaviour {
    [SerializeField]
    private TMP_Text valueText;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private GameObject contentHolder;
    private BuyOptionInstance buyOptionInstance;

    public string Name { get; private set; }
    
    public void InitInstance(BuyOptionInstance _buyOptionInstance,BuyOptionCurrencyInstanceData data,Sprite icon) {
        buyOptionInstance = _buyOptionInstance;
        iconImage.sprite = icon;
        Value = data.value;
        Name = data.name;
        UnlockQuantity = data.unlockQuantity;
        buyOptionInstance.OnQuantityChanged += OnQuantityChanged;
        OnQuantityChanged();
    }

    private ulong _value;
    public ulong Value {
        get {
            return _value;
        }
        set {
            _value = value;
            valueText.text = _value.ToString();
        }
    }

    public int UnlockQuantity { get; private set; }

    private void OnQuantityChanged() {
        contentHolder.SetActive(buyOptionInstance.Quantity >= UnlockQuantity);
    }
}
