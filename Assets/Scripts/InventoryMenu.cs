using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//klasa do przechowywania rzeczy ogolnie dla itemu 
public class ItemTemplate {
    public string name;
    public Sprite icon;
    public ulong maxStackCount;
    public string type;
    public uint clicksToPop;
    public ulong price;
    public ulong powerOfDynamite;
    public ulong clicksMultiplier;
    public ulong damageMultiplier;
    public double reductionMultiplier;
}

public class InventoryMenu : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
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
    public InventoryItemSlot swordInventoryItemSlot;
    public InventoryItemSlot armorInventoryItemSlot;
    
// slownik ktory mapuje nazwe przedmiotu na klase itemTemplate dla danego przedmiotu => itemTemplates["nazwa przedmiotu"]
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
    //sprawdza czy mozna dodac item (maxStack) zwraca 1,0
    public bool CanAddItems(string name,SafeUDecimal count) {
        if(OreItemsSlots.ContainsKey(name)) {
            return true;
        }
        // ?? - sprawdza po prawej i lewej jesli lewa to 0 to zwraca wyjatek 
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
    // sprawdza czy mozna usunac liczbe przedmiotow 
    public bool CanRemoveItems(string name,SafeUDecimal count) {
        // containsKey sprawdza czy w oreItemsSlots jest czy nie 
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
    // to samo tylko dodawanie itemow w okreslonej ilosci
    public bool AddItems(string name,SafeUDecimal count) {
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
    //usuwa ilosc itemkow 
    public bool RemoveItems(string name,SafeUDecimal count) {
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
    //specjalne sloty na rudy u gory w eq one zawieraja instrukcje Count itp
    private Dictionary<string,InventoryOreItemSlot> _oreItemSlots = null;
    public Dictionary<string,InventoryOreItemSlot> OreItemsSlots {
        get {
            if(_oreItemSlots == null) {
                InitItems();
            }
            return _oreItemSlots;
        }
    }
    //specjalny skrot dla pieniedzy
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
    // mnozniki dla podstawowych przedmiotow
    public ulong GetClicksMultiplier()
    {
        if (pickaxeInventoryItemSlot.ItemTemplate == null)
        {
            return 1;
        }

        return pickaxeInventoryItemSlot.ItemTemplate.clicksMultiplier;
    }
    public ulong GetDamageMultiplier()
    {
        if (swordInventoryItemSlot.ItemTemplate == null)
        {
            return 1;
        }

        return swordInventoryItemSlot.ItemTemplate.damageMultiplier;
    }
    public double GetReductionMultiplier()
    {
        if (armorInventoryItemSlot.ItemTemplate == null)
        {
            return 1;
        }

        return armorInventoryItemSlot.ItemTemplate.reductionMultiplier;
    }
    
    //Lista slotow w eq dla zwyklych przedmiotow
    public readonly List<InventoryItemSlot> itemSlots = new();

    public void Init() {
        for(int i = 0;i < 20;i += 1) {
            var prefab = Instantiate(inventoryItemSlotPrefab,itemsParent);
            var component = prefab.GetComponent<InventoryItemSlot>();
            component.Init(this);
            itemSlots.Add(component);
        }
        pickaxeInventoryItemSlot.Init(this);
        swordInventoryItemSlot.Init(this);
        armorInventoryItemSlot.Init(this);
    }
    //wczytywanie z JSONa wszystkiego
    private void InitItems() {
        _itemTemplates = new();
        _oreItemSlots = new();
        var itemDatas = JsonUtility.FromJson<InstanceWrapperDataJson<InventoryItemDataJson>>(inventoryItemDataAsset.text);
        foreach(var item in itemDatas.data) {
            ItemTemplate template = new() {
                name = item.name,
                icon = Resources.Load<Sprite>("Images/" + item.iconPath),
                powerOfDynamite = item.powerOfDynamite,
                maxStackCount = item.maxStackCount,
                clicksToPop = item.clicksToPop,
                type = item.type,
                price = item.price,
                clicksMultiplier = item.clicksMultiplier,
                damageMultiplier = item.damageMultiplier,
                reductionMultiplier = item.reductionMultiplier
                
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
