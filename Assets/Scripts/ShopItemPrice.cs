using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        resourceIcon.sprite = inventoryMenu.ItemTemplates[data.name].icon;
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

    private void OnCountChange(SafeInteger count) {
        content.gameObject.SetActive(UnlockCount <= count);
    }

    public SafeInteger UnlockCount { get; private set; }

    private SafeInteger _value = 0;
    public SafeInteger Value {
        get {
            return _value;
        }
        set {
            _value = value;
            resourceQuantity.text = _value.ToString();
        }
    }
}
