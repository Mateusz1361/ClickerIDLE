using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOreItemSlot : MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text countText;
    [SerializeField]
    private Button clickButton;

    private ItemTemplate _itemTemplate = null;
    public ItemTemplate ItemTemplate {
        get {
            return _itemTemplate;
        }
        private set {
            _itemTemplate = value;
            icon.sprite = _itemTemplate?.icon;
            icon.gameObject.SetActive(icon.sprite != null);
        }
    }

    public event Action<SafeUDecimal> OnCountChanged;
    public event Action<SafeUDecimal> OnCountIncremented;

    private SafeUDecimal _count = 0;
    public SafeUDecimal Count {
        get {
            return _count;
        }
        set {
            if(_count != value) {
                OnCountChanged?.Invoke(value);
            }
            if(value > _count) {
                OnCountIncremented?.Invoke(value - _count);
            }
            _count = value;
            countText.text = NumberFormat.ShortForm(_count);
            countText.gameObject.SetActive(_itemTemplate != null);
        }
    }

    public void Init(ItemTemplate _template) {
        ItemTemplate = _template;
    }
}
