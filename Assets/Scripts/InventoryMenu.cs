using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using static UnityEditor.Progress;

public class InventoryItemTemplate {
    public Sprite icon;
    public uint maxStackCount;
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
    private InventoryItemSlot pickaxeItemSlot;

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

    private Dictionary<string,InventoryItemTemplate> itemTemplates;
    private List<InventoryItemSlot> itemSlots;

    public bool AddItem(string name) {
        var itemTemplate = itemTemplates[name] ?? throw new ArgumentException($"There's no item named '{name}'.");
        for(int i = 0;i < itemSlots.Count;i += 1) {
            if(itemSlots[i].ItemTemplate == null) {
                itemSlots[i].SetItemTemplate(itemTemplate);
                itemSlots[i].Count = 1;
                return true;
            }
            else if(itemSlots[i].ItemTemplate == itemTemplate && itemSlots[i].Count < itemTemplate.maxStackCount) {
                itemSlots[i].Count += 1;
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
                icon = Resources.Load<Sprite>($"Images/{item.iconPath}"),
                maxStackCount = item.maxStackCount
            });
        }

        pickaxeItemSlot.SetItemTemplate(null);
        pickaxeItemSlot.Count = 0;

        itemSlots = new();
        for(int i = 0;i < 20;i += 1) {
            var prefab = Instantiate(inventoryItemSlotPrefab,itemsParent);
            var component = prefab.GetComponent<InventoryItemSlot>();
            component.SetItemTemplate(null);
            component.Count = 0;
            itemSlots.Add(component);
        }

        AddItem("Wooden Pickaxe");
        for(int i = 0;i < 12;i += 1) {
            AddItem("Dynamite");
        }
        AddItem("Stone Pickaxe");
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
