using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class InventoryItemSlot : MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text countText;
    [SerializeField]
    private Button clickButton;
    [SerializeField]
    private GameObject buttonList;
    [SerializeField]
    private Button actionButton;
    [SerializeField]
    private Button sellButton;

    private ItemTemplate _itemTemplate = null;
    public ItemTemplate ItemTemplate {
        get {
            return _itemTemplate;
        }
        set {
            _itemTemplate = value;
            icon.sprite = _itemTemplate?.icon;
            icon.gameObject.SetActive(icon.sprite != null);
        }
    }

    private SafeUInteger _count = 0;
    public SafeUInteger Count {
        get {
            return _count;
        }
        set {
            _count = value;
            countText.text = _count.ToString();
            countText.gameObject.SetActive(_count >= 2);
        }
    }

    private void Awake() {
        buttonList.SetActive(false);
        clickButton.onClick.AddListener(() => {
            if(ItemTemplate != null && Count > 0) {
                buttonList.SetActive(!buttonList.activeSelf);
            }
        });
        sellButton.onClick.AddListener(() => {
            if(ItemTemplate != null && Count > 0) {
                inventoryMenu.Money.Count += ItemTemplate.price;
                Count -= 1;
                if(Count == 0) {
                    ItemTemplate = null;
                    buttonList.SetActive(false);
                }
            }
        });
        actionButton.onClick.AddListener(() => {
            if(ItemTemplate == null) return;
            if(ItemTemplate.type == "Pickaxe") {
                EquipItem(inventoryMenu.pickaxeInventoryItemSlot);
            }
            else if (ItemTemplate.type == "Sword") {
                EquipItem(inventoryMenu.swordInventoryItemSlot);
            }
            else if (ItemTemplate.type == "Armor") {
                EquipItem(inventoryMenu.armorInventoryItemSlot);
            }
            buttonList.SetActive(false);
        });
    }

    private void EquipItem(InventoryItemSlot type) {
        if(this != type && type.ItemTemplate == null) {
            type.ItemTemplate = ItemTemplate;
            type.Count = 1;
            Count -= 1;
            if(Count == 0) ItemTemplate = null;
        }
        else if(this == type && inventoryMenu.CanAddItems(ItemTemplate.name,Count)) {
            inventoryMenu.AddItems(ItemTemplate.name,Count);
            ItemTemplate = null;
            Count = 0;
        }
    }

    private void OnEnable() {
        if(ItemTemplate != null) {
            actionButton.gameObject.SetActive(ItemTemplate.type is "Pickaxe" or "Sword" or "Armor");
        }
    }

    private InventoryMenu inventoryMenu = null;
    public void Init(InventoryMenu _inventoryMenu) {
        inventoryMenu = _inventoryMenu;
    }
}
