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

    public InventoryItemTemplate ItemTemplate { get; private set; }

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
        clickButton.onClick.AddListener(() => {
            if(ItemTemplate == null) {
                return;
            }
            if(inventoryMenu.pickaxeItemSlot != this) {
                if(ItemTemplate.type == "Pickaxe") {
                    if(inventoryMenu.pickaxeItemSlot.ItemTemplate == null) {
                        inventoryMenu.pickaxeItemSlot.SetItemTemplate(ItemTemplate);
                        inventoryMenu.pickaxeItemSlot.Count = Count;
                        SetItemTemplate(null);
                        Count = 0;
                    }
                    else {
                        var tempItem = ItemTemplate;
                        var tempCount = Count;
                        SetItemTemplate(inventoryMenu.pickaxeItemSlot.ItemTemplate);
                        Count = inventoryMenu.pickaxeItemSlot.Count;
                        inventoryMenu.pickaxeItemSlot.SetItemTemplate(tempItem);
                        inventoryMenu.pickaxeItemSlot.Count = tempCount;
                    }
                }
            }
            else if(inventoryMenu.AddItems(ItemTemplate,Count)) {
                SetItemTemplate(null);
                Count = 0;
            }
        });
    }

    public void SetItemTemplate(InventoryItemTemplate itemTemplate) {
        ItemTemplate = itemTemplate;
        icon.sprite = ItemTemplate?.icon;
        icon.enabled = icon.sprite != null;
    }
}
