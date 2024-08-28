using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class ShopItemPrice : MonoBehaviour {
    [SerializeField]
    private Image resourceIcon;
    [SerializeField]
    private TMP_Text resourceQuantity;
    [SerializeField]
    private RectTransform content;
    private ShopItemPriceData defaultSettings;
    public string Name { get; private set; }

    public void InitPrice(InventoryMenu inventoryMenu,ShopItem shopItem,ShopItemPriceData data) {
        resourceIcon.sprite = inventoryMenu.ResourceInstances[data.name].Icon;
        Name = data.name;
        Value = data.value;
        UnlockCount = data.unlockCount;
        if(shopItem != null) {
            shopItem.OnCountChange += OnCountChange;
            OnCountChange(0);
        }
        defaultSettings = data;
    }

    public void ResetPrice() { 
        Value = defaultSettings.value;
        OnCountChange(0);
    }

    private void OnCountChange(ulong count) {
        content.gameObject.SetActive(UnlockCount <= count);
    }

    public ulong UnlockCount { get; private set; }

    private BigInteger _value;
    public BigInteger Value {
        get {
            return _value;
        }
        set {
            _value = value;
            resourceQuantity.text = NumberFormat.ShortForm(_value);
        }
    }
}
