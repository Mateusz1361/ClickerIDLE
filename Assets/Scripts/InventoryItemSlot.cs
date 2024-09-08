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

    private SafeInteger _count = 0;
    public SafeInteger Count {
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
            if(ItemTemplate.type == "Pickaxe") {
                EquipItem();
                buttonList.SetActive(false);
            }
        });
    }

    public void EquipItem() {
        if(ItemTemplate == null || Count == 0) return;
        var pis = inventoryMenu.pickaxeInventoryItemSlot;
        if(this != pis && pis.ItemTemplate == null) {
            pis.ItemTemplate = ItemTemplate;
            pis.Count = 1;
            Count -= 1;
            if(Count == 0) ItemTemplate = null;
        }
        else if(this == pis && inventoryMenu.CanAddItems(ItemTemplate.name,Count)) {
            inventoryMenu.AddItems(ItemTemplate.name,Count);
            ItemTemplate = null;
            Count = 0;
        }
    }

    private void OnEnable() {
        if(ItemTemplate != null) {
            actionButton.gameObject.SetActive(ItemTemplate.type == "Pickaxe");
        }
    }

    private InventoryMenu inventoryMenu = null;
    public void Init(InventoryMenu _inventoryMenu) {
        inventoryMenu = _inventoryMenu;
    }
}
