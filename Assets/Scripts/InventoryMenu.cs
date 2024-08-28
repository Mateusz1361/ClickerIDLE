using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryItemTemplate {
    public string name;
    public Sprite icon;
    public uint maxStackCount;
    public string type;
}

public class InventoryMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject resourceInstancePrefab;
    [SerializeField]
    private GameObject oresScrollRect;
    [SerializeField]
    private GameObject oresParent;
    [SerializeField]
    private Button oresButton;
    [SerializeField]
    private GameObject itemsScroll;
    [SerializeField]
    private Transform itemsParent;
    [SerializeField]
    private Button itemsButton;
    [SerializeField]
    private TextAsset resourcesData;
    [SerializeField]
    private GameObject inventoryItemSlotPrefab;
    [SerializeField]
    private TextAsset inventoryItemDataAsset;
    [SerializeField]
    public InventoryItemSlot pickaxeItemSlot;

    private Dictionary<string,ResourceInstance> _resourceInstances;

    public Dictionary<string,ResourceInstance> ResourceInstances {
        get {
            if(_resourceInstances == null) {
                InitResourceInstances();
            }
            return _resourceInstances;
        }
    }

    private ResourceInstance _stone;
    public ResourceInstance Stone {
        get {
            if(_stone == null) {
                _stone = ResourceInstances["Stone"];
            }
            return _stone;
        }
    }

    private ResourceInstance _money;
    public ResourceInstance Money {
        get {
            if(_money == null) {
                _money = ResourceInstances["Money"];
            }
            return _money;
        }
    }

    public Dictionary<string,InventoryItemTemplate> itemTemplates;
    public List<InventoryItemSlot> itemSlots;

    public bool CanAddItems(string name,uint count = 1) {
        var itemTemplate = itemTemplates[name] ?? throw new ArgumentException($"There's no item named '{name}'.");
        return CanAddItems(itemTemplate,count);
    }

    public bool CanAddItems(InventoryItemTemplate itemTemplate,uint count = 1) {
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == null) {
                if(count <= itemTemplate.maxStackCount) {
                    return true;
                }
                count -= itemTemplate.maxStackCount;
            }
            else if(slot.ItemTemplate == itemTemplate && slot.Count < itemTemplate.maxStackCount) {
                if(slot.Count + count <= itemTemplate.maxStackCount) {
                    return true;
                }
                count -= itemTemplate.maxStackCount - slot.Count;
            }
        }
        return false;
    }

    public bool AddItems(string name,uint count = 1) {
        var itemTemplate = itemTemplates[name] ?? throw new ArgumentException($"There's no item named '{name}'.");
        return AddItems(itemTemplate,count);
    }

    public bool AddItems(InventoryItemTemplate itemTemplate,uint count = 1) {
        if(count == 0) {
            return true;
        }
        if(!CanAddItems(itemTemplate,count)) {
            return false;
        }
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == null) {
                slot.ItemTemplate = itemTemplate;
                if(count <= itemTemplate.maxStackCount) {
                    slot.Count = count;
                    return true;
                }
                slot.Count = itemTemplate.maxStackCount;
                count -= itemTemplate.maxStackCount;
            }
            else if(slot.ItemTemplate == itemTemplate && slot.Count < itemTemplate.maxStackCount) {
                if(slot.Count + count <= itemTemplate.maxStackCount) {
                    slot.Count += count;
                    return true;
                }
                count -= itemTemplate.maxStackCount - slot.Count;
                slot.Count = itemTemplate.maxStackCount;
            }
        }
        return false;
    }

    public bool RemoveItem(string name) {
        var itemTemplate = itemTemplates[name] ?? throw new ArgumentException($"There's no item named '{name}'.");
        return RemoveItem(itemTemplate);
    }

    public bool RemoveItem(InventoryItemTemplate itemTemplate) {
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == itemTemplate) {
                if(slot.Count == 1) {
                    slot.ItemTemplate = null;
                }
                slot.Count -= 1;
                return true;
            }
        }
        return false;
    }

    public bool HasItem(string name) {
        var itemTemplate = itemTemplates[name] ?? throw new ArgumentException($"There's no item named '{name}'.");
        return HasItem(itemTemplate);
    }

    public bool HasItem(InventoryItemTemplate itemTemplate) {
        foreach(var slot in itemSlots) {
            if(slot.ItemTemplate == itemTemplate && slot.Count > 0) {
                return true;
            }
        }
        return false;
    }

    public void Init() {
        oresScrollRect.SetActive(true);
        itemsScroll.SetActive(false);
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        oresButton.onClick.AddListener(() => {
            oresScrollRect.SetActive(true);
            itemsScroll.SetActive(false);
        });
        itemsButton.onClick.AddListener(() => {
            oresScrollRect.SetActive(false);
            itemsScroll.SetActive(true);
        });
        if(_resourceInstances == null) {
            InitResourceInstances();
        }

        itemTemplates = new();
        var inventoryItemDataWrapper = JsonUtility.FromJson<InstanceWrapper<InventoryItemData>>(inventoryItemDataAsset.text);
        foreach(var item in inventoryItemDataWrapper.data) {
            itemTemplates.Add(item.name,new() {
                name = item.name,
                icon = Resources.Load<Sprite>($"Images/{item.iconPath}"),
                maxStackCount = item.maxStackCount,
                type = item.type
            });
        }

        pickaxeItemSlot.Init(this);
        itemSlots = new();
        for(int i = 0;i < 20;i += 1) {
            var prefab = Instantiate(inventoryItemSlotPrefab,itemsParent);
            var component = prefab.GetComponent<InventoryItemSlot>();
            component.Init(this);
            itemSlots.Add(component);
        }
    }

    private void InitResourceInstances() {
        _resourceInstances = new();
        var resourceInstanceDatas = JsonUtility.FromJson<InstanceWrapper<ResourceInstanceData>>(resourcesData.text);
        foreach(var resourceInstanceData in resourceInstanceDatas.data) {
            var prefab = Instantiate(resourceInstancePrefab,oresParent.transform);
            var resourceInstance = prefab.GetComponent<ResourceInstance>();
            resourceInstance.InitInstance(resourceInstanceData);
            _resourceInstances.Add(resourceInstanceData.name,resourceInstance);
        }
    }
}
