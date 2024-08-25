using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text countText;
    [SerializeField]
    private Button clickButton;
    private InventoryMenu inventoryMenu;

    private InventoryItemTemplate _itemTemplate;
    public InventoryItemTemplate ItemTemplate {
        get {
            return _itemTemplate;
        }
        set {
            _itemTemplate = value;
            icon.sprite = _itemTemplate?.icon;
            icon.enabled = icon.sprite != null;
        }
    }

    private uint _count;
    public uint Count {
        get {
            return _count;
        }
        set {
            _count = value;
            countText.text = _count <= 1 ? "" : _count.ToString();
        }
    }

    public void Init(InventoryMenu _inventoryMenu) {
        inventoryMenu = _inventoryMenu;
        clickButton.onClick.AddListener(OnItemButtonClick);
        ItemTemplate = null;
        Count = 0;
    }

    private void OnItemButtonClick() {
        if(ItemTemplate != null) {
            if(inventoryMenu.pickaxeItemSlot != this) {
                if(ItemTemplate.type == "Pickaxe") {
                    if(inventoryMenu.pickaxeItemSlot.ItemTemplate == null) {
                        inventoryMenu.pickaxeItemSlot.ItemTemplate = ItemTemplate;
                        inventoryMenu.pickaxeItemSlot.Count = Count;
                        ItemTemplate = null;
                        Count = 0;
                    }
                    else {
                        var tempItem = ItemTemplate;
                        var tempCount = Count;
                        ItemTemplate = inventoryMenu.pickaxeItemSlot.ItemTemplate;
                        Count = inventoryMenu.pickaxeItemSlot.Count;
                        inventoryMenu.pickaxeItemSlot.ItemTemplate = tempItem;
                        inventoryMenu.pickaxeItemSlot.Count = tempCount;
                    }
                }
            }
            else if(inventoryMenu.AddItems(ItemTemplate,Count)) {
                ItemTemplate = null;
                Count = 0;
            }
        }
    }
}
