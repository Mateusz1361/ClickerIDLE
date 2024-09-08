using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveShopItemPriceData {
    public ulong value;
}

[Serializable]
public class SaveShopItemData {
    public string name;
    public ulong resultQuantity;
    public ulong count;
    public ulong multiplier;
    public SaveShopItemPriceData[] shopItemPriceDatas;
}

[Serializable]
public class SaveInvestorUpgradeData {
    public string whatYouGetText;
    public bool purchased;
}

[Serializable]
public class SaveWorldLocationData {
    public string name;
    public bool purchased;
    public SaveShopItemData[] shopItemsToSaveData;
    public ulong investorsToClaim;
    public ulong investorsYouHave;
    public ulong level;
    public double experience;
    public double maxExperience;
    public ulong differenceOfMaterial;
    public ulong quantityToAddInvestor;
    public ulong mainResourceAutoIncrement;
    public float mainResourceAutoIncrementTimer;
    public SaveInvestorUpgradeData[] investorUpgradeDatas;
}

[Serializable]
public class SaveInventoryItemData {
    public string name;
    public ulong count;
}

[Serializable]
public class SaveData {
    public SaveWorldLocationData[] worldLocationSaveDatas;
    public SaveInventoryItemData[] saveInventoryOreItemDatas;
    public SaveInventoryItemData[] saveInventoryItemDatas;
    public SaveInventoryItemData saveInventoryPickaxeItemData;
}

public class SaveSystem : MonoBehaviour {
    [SerializeField]
    private ShopMenu shopMenu;
    [SerializeField]
    private WorldMenu worldMenu;
    [SerializeField]
    private InventoryMenu inventoryMenu;
    [SerializeField]
    private InvestorMenu investorMenu;

    private void OnApplicationQuit() {
        SaveGame();
    }

    public void SaveGame() {
        SaveData saveData = new() {
            worldLocationSaveDatas = new SaveWorldLocationData[worldMenu.WorldLocations.Count]
        };
        for(int i = 0;i < worldMenu.WorldLocations.Count;i += 1) {
            var location = worldMenu.WorldLocations[i];

            saveData.worldLocationSaveDatas[i] = new() {
                level = location.Level,
                experience = location.Experience,
                maxExperience = location.maxExperience,
                differenceOfMaterial = (ulong)location.differenceOfMaterial,
                quantityToAddInvestor = (ulong)location.quantityToAddInvestor,
                mainResourceAutoIncrementTimer = location.mainResourceAutoIncrementTimer,
                investorsToClaim = (ulong)location.InvestorsToClaim,
                investorsYouHave = (ulong)location.InvestorsYouHave,
                purchased = location.Purchased,
                name = location.Name,
                shopItemsToSaveData = new SaveShopItemData[location.ShopItems.Count],
                investorUpgradeDatas = new SaveInvestorUpgradeData[location.InvestorUpgrades.Count]
            };

            for(int j = 0;j < location.ShopItems.Count;j += 1) {
                var item = location.ShopItems[j];
                saveData.worldLocationSaveDatas[i].shopItemsToSaveData[j] = new SaveShopItemData{
                    name = item.name,
                    resultQuantity = (ulong)item.ResultQuantity,
                    count = (ulong)item.Count,
                    multiplier = (ulong)item.multiplier,
                    shopItemPriceDatas = new SaveShopItemPriceData[item.shopItemsPrices.Count],
                };
                for(int k = 0;k < item.shopItemsPrices.Count;k += 1) {
                    saveData.worldLocationSaveDatas[i].shopItemsToSaveData[j].shopItemPriceDatas[k] = new SaveShopItemPriceData {
                        value = (ulong)item.shopItemsPrices[k].Value
                    };
                }
            }

            for(int j = 0;j < location.InvestorUpgrades.Count;j += 1) {
                var upgrade = location.InvestorUpgrades[j];
                saveData.worldLocationSaveDatas[i].investorUpgradeDatas[j] = new SaveInvestorUpgradeData {
                    purchased = upgrade.Purchased,
                    whatYouGetText = upgrade.WhatYouGetText
                };
            }
        }

        saveData.saveInventoryOreItemDatas = new SaveInventoryItemData[inventoryMenu.OreItemsSlots.Count];
        int index = 0;
        foreach(var slot in inventoryMenu.OreItemsSlots.Values) {
            saveData.saveInventoryOreItemDatas[index] = new() {
                name = slot.ItemTemplate.name,
                count = (ulong)slot.Count
            };
            index += 1;
        }

        saveData.saveInventoryItemDatas = new SaveInventoryItemData[inventoryMenu.itemSlots.Count];
        for(int i = 0;i < inventoryMenu.itemSlots.Count;i +=1 ) {
            saveData.saveInventoryItemDatas[i] = new() {
                name = inventoryMenu.itemSlots[i].ItemTemplate?.name,
                count = (ulong)inventoryMenu.itemSlots[i].Count
            };
        }
        saveData.saveInventoryPickaxeItemData = new() {
            name = inventoryMenu.pickaxeInventoryItemSlot.ItemTemplate?.name,
            count = (ulong)inventoryMenu.pickaxeInventoryItemSlot.Count
        };

        var temp = JsonUtility.ToJson(saveData);
        Directory.CreateDirectory(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/save.json",temp);
    }

    public void LoadGame() {
        if(!Directory.Exists(Application.persistentDataPath)) return;
        if(!File.Exists(Application.persistentDataPath + "/save.json")) return;

        var temp = File.ReadAllText(Application.persistentDataPath + "/save.json");
        var saveData = JsonUtility.FromJson<SaveData>(temp);

        for(int i = 0;i < saveData.worldLocationSaveDatas.Length;i += 1) {
            var savedLocation = saveData.worldLocationSaveDatas[i];

            var currentLocation = worldMenu.WorldLocations[i];
            currentLocation.InvestorsToClaim = savedLocation.investorsToClaim;
            currentLocation.InvestorsYouHave = savedLocation.investorsYouHave;
            currentLocation.Level = savedLocation.level;
            currentLocation.maxExperience = savedLocation.maxExperience;
            currentLocation.Experience = savedLocation.experience;
            currentLocation.differenceOfMaterial = savedLocation.differenceOfMaterial;
            currentLocation.quantityToAddInvestor = savedLocation.quantityToAddInvestor;
            currentLocation.mainResourceAutoIncrementTimer = savedLocation.mainResourceAutoIncrementTimer;
            currentLocation.Purchased = savedLocation.purchased;

            for(int j = 0;j < savedLocation.shopItemsToSaveData.Length;j += 1) {
                var item = savedLocation.shopItemsToSaveData[j];
                var found = currentLocation.ShopItems.Find((instance) => instance.name == item.name);
                if(found != null) {
                    found.ResultQuantity = item.resultQuantity;
                    found.Count = item.count;
                    found.multiplier = item.multiplier;
                    found.RecalculateMainResourceAutoIncrement();
                    found.RecalculateMainResourceClickIncrement();
                    for(int k = 0;k < item.shopItemPriceDatas.Length;k += 1) {
                        found.shopItemsPrices[k].Value = item.shopItemPriceDatas[k].value;
                    }
                }
            }
            for(int j = 0;j < savedLocation.investorUpgradeDatas.Length;j += 1) {
                var upgrade = savedLocation.investorUpgradeDatas[j];
                var found = currentLocation.InvestorUpgrades.Find((instance) => instance.WhatYouGetText == upgrade.whatYouGetText);
                if(found != null) found.Purchased = upgrade.purchased;
            }
            investorMenu.UpdateInvestors();
            currentLocation.RecalculateMainResourceClickIncrement();
            currentLocation.RecalculateMainResourceAutoIncrement();
        }

        for(int i = 0;i < saveData.saveInventoryOreItemDatas.Length;i += 1) {
            var itemData = saveData.saveInventoryOreItemDatas[i];
            if(inventoryMenu.OreItemsSlots.ContainsKey(itemData.name)) {
                inventoryMenu.OreItemsSlots[itemData.name].Count = itemData.count;
            }
        }

        for(int i = 0;i < saveData.saveInventoryItemDatas.Length;i += 1) {
            var itemData = saveData.saveInventoryItemDatas[i];
            if(itemData.name != null && inventoryMenu.ItemTemplates.ContainsKey(itemData.name) && i < inventoryMenu.itemSlots.Count) {
                inventoryMenu.itemSlots[i].ItemTemplate = inventoryMenu.ItemTemplates[itemData.name];
                inventoryMenu.itemSlots[i].Count = itemData.count;
            }
        }
        if(inventoryMenu.ItemTemplates.ContainsKey(saveData.saveInventoryPickaxeItemData.name)) {
            inventoryMenu.pickaxeInventoryItemSlot.ItemTemplate = inventoryMenu.ItemTemplates[saveData.saveInventoryPickaxeItemData.name];
            inventoryMenu.pickaxeInventoryItemSlot.Count = saveData.saveInventoryPickaxeItemData.count;
        }
    }
}
