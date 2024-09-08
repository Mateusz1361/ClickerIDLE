using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemTemplate {
    public string name;
    public Sprite icon;
    public ulong maxStackCount;
    public string type;
    public uint clicksToPop;
    public ulong price;
}

public class InventoryMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject itemsScroll;
    [SerializeField]
    private Transform itemsParent;
    [SerializeField]
    private Transform oreItemsParent;
    [SerializeField]
    private GameObject inventoryItemSlotPrefab;
    [SerializeField]
    private GameObject inventoryOreItemSlotPrefab;
    [SerializeField]
    private TextAsset inventoryItemDataAsset;
    public InventoryItemSlot pickaxeInventoryItemSlot;

    private Dictionary<string,ItemTemplate> _itemTemplates = null;
    public Dictionary<string,ItemTemplate> ItemTemplates {
        get {
            if(_itemTemplates == null) {
                InitItems();
            }
            return _itemTemplates;
        }
    }

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public bool CanAddItems(string name,SafeInteger count) {
        if(OreItemsSlots.ContainsKey(name)) {
            return true;
        }
        var template = ItemTemplates[name] ?? throw new ArgumentException($"There's no item named \"{name}\".");
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == null) {
                if(count <= template.maxStackCount) {
                    return true;
                }
                count -= template.maxStackCount;
            }
            else if(slot.ItemTemplate == template && slot.Count < template.maxStackCount) {
                if(slot.Count + count <= template.maxStackCount) {
                    return true;
                }
                count -= template.maxStackCount - slot.Count;
            }
        }
        return false;
    }

    public bool CanRemoveItems(string name,SafeInteger count) {
        if(OreItemsSlots.ContainsKey(name)) {
            return OreItemsSlots[name].Count >= count;
        }
        var template = ItemTemplates[name] ?? throw new ArgumentException($"There's no item named \"{name}\".");
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == template) {
                if(slot.Count >= count) {
                    return true;
                }
                count -= slot.Count;
            }
        }
        return false;
    }

    public bool AddItems(string name,SafeInteger count) {
        if(!CanAddItems(name,count)) return false;
        if(OreItemsSlots.ContainsKey(name)) {
            OreItemsSlots[name].Count += count;
            return true;
        }
        var template = ItemTemplates[name] ?? throw new ArgumentException($"There's no item named \"{name}\".");
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == null) {
                slot.ItemTemplate = template;
                if(count <= template.maxStackCount) {
                    slot.Count = count;
                    return true;
                }
                slot.Count = template.maxStackCount;
                count -= template.maxStackCount;
            }
            else if(slot.ItemTemplate == template && slot.Count < template.maxStackCount) {
                if(slot.Count + count <= template.maxStackCount) {
                    slot.Count += count;
                    return true;
                }
                count -= template.maxStackCount - slot.Count;
                slot.Count = template.maxStackCount;
            }
        }
        return false;
    }

    public bool RemoveItems(string name,SafeInteger count) {
        if(!CanRemoveItems(name,count)) return false;
        if(OreItemsSlots.ContainsKey(name)) {
            OreItemsSlots[name].Count -= count;
            return true;
        }
        var template = ItemTemplates[name] ?? throw new ArgumentException($"There's no item named \"{name}\".");
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == template) {
                if(slot.Count == count) {
                    slot.ItemTemplate = null;
                    slot.Count = 0;
                    return true;
                }
                if(slot.Count > count) {
                    slot.Count -= count;
                    return true;
                }
                count -= slot.Count;
            }
        }
        return false;
    }

    private Dictionary<string,InventoryOreItemSlot> _oreItemSlots = null;
    public Dictionary<string,InventoryOreItemSlot> OreItemsSlots {
        get {
            if(_oreItemSlots == null) {
                InitItems();
            }
            return _oreItemSlots;
        }
    }

    private InventoryOreItemSlot _money = null;
    public InventoryOreItemSlot Money {
        get {
            if(_oreItemSlots == null) {
                InitItems();
            }
            _money = _money != null ? _money : OreItemsSlots["Money"];
            return _money;
        }
    }

    public readonly List<InventoryItemSlot> itemSlots = new();
    public void Init() {
        for(int i = 0;i < 20;i += 1) {
            var prefab = Instantiate(inventoryItemSlotPrefab,itemsParent);
            var component = prefab.GetComponent<InventoryItemSlot>();
            component.Init(this);
            component.ItemTemplate = null;
            component.Count = 0;
            itemSlots.Add(component);
        }
        pickaxeInventoryItemSlot.Init(this);
        pickaxeInventoryItemSlot.ItemTemplate = null;
        pickaxeInventoryItemSlot.Count = 0;
    }

    private void InitItems() {
        _itemTemplates = new();
        _oreItemSlots = new();
        var itemDatas = JsonUtility.FromJson<InstanceWrapperData<InventoryItemData>>(inventoryItemDataAsset.text);
        foreach(var item in itemDatas.data) {
            ItemTemplate template = new() {
                name = item.name,
                icon = Resources.Load<Sprite>("Images/" + item.iconPath),
                maxStackCount = item.maxStackCount,
                clicksToPop = item.clicksToPop,
                type = item.type,
                price = item.price
            };
            _itemTemplates.Add(item.name,template);
            if(template.type.Contains("Ore")) {
                var prefab = Instantiate(inventoryOreItemSlotPrefab,oreItemsParent);
                var component = prefab.GetComponent<InventoryOreItemSlot>();
                component.Init(template);
                component.Count = 0;
                _oreItemSlots.Add(template.name,component);
            }
        }
    }
}
