using System;
using System.IO;
using UnityEngine;
using System.Numerics;

[Serializable]
public class SaveShopItemPriceData {
    public string value;
}

[Serializable]
public class SaveShopItemData {
    public string name;
    public string resultQuantity;
    public ulong count;
    public string clickIncrement;
    public string autoIncrement;
    public string multiplier;
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
    public string investorsToClaim;
    public string investorsYouHave;
    public ulong level;
    public double experience;
    public double maxExperience;
    public string differenceOfMaterial;
    public string quantityToAddInvestor;
    public string mainResourceAutoIncrement;
    public float mainResourceAutoIncrementTimer;
    public SaveInvestorUpgradeData[] investorUpgradeDatas;
}

[Serializable]
public class SaveInventoryResourceData {
    public string name;
    public string count;
}

[Serializable]
public class SaveInventoryItemData {
    public string name;
    public uint count;
}

[Serializable]
public class SaveData {
    public SaveInventoryResourceData[] inventoryResources;
    public SaveWorldLocationData[] worldLocationsToSaveData;
    public SaveInventoryItemData[] saveInventoryItemDatas;
    public SaveInventoryItemData saveInventoryItemPickaxeData;
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
            inventoryResources = new SaveInventoryResourceData[inventoryMenu.ResourceInstances.Count]
        };
        int index = 0;
        foreach((var name,var resource) in inventoryMenu.ResourceInstances) { 
            saveData.inventoryResources[index] = new SaveInventoryResourceData{name = name,count = resource.Count.ToString()};
            index++;
        }
        saveData.worldLocationsToSaveData = new SaveWorldLocationData[worldMenu.WorldLocations.Count];
        for(int i = 0;i < worldMenu.WorldLocations.Count;i += 1) {
            var location = worldMenu.WorldLocations[i];

            saveData.worldLocationsToSaveData[i] = new() {
                level = location.Level,
                experience = location.Experience,
                maxExperience = location.maxExperience,
                differenceOfMaterial = location.differenceOfMaterial.ToString(),
                quantityToAddInvestor = location.quantityToAddInvestor.ToString(),
                mainResourceAutoIncrementTimer = location.mainResourceAutoIncrementTimer,
                investorsToClaim = location.InvestorsToClaim.ToString(),
                investorsYouHave = location.InvestorsYouHave.ToString(),
                purchased = location.Purchased,
                name = location.Name,
                shopItemsToSaveData = new SaveShopItemData[location.ShopItems.Count],
                investorUpgradeDatas = new SaveInvestorUpgradeData[location.InvestorUpgrades.Count]
            };

            for(int j = 0;j < location.ShopItems.Count;j += 1) {
                var item = location.ShopItems[j];
                saveData.worldLocationsToSaveData[i].shopItemsToSaveData[j] = new SaveShopItemData{
                    name = item.name,
                    resultQuantity = item.ResultQuantity.ToString(),
                    count = item.Count,
                    multiplier = item.multiplier.ToString(),
                    shopItemPriceDatas = new SaveShopItemPriceData[item.shopItemsPrices.Count]
                };
                for(int k = 0;k < item.shopItemsPrices.Count;k += 1) {
                    saveData.worldLocationsToSaveData[i].shopItemsToSaveData[j].shopItemPriceDatas[k] = new SaveShopItemPriceData {
                        value = item.shopItemsPrices[k].Value.ToString()
                    };
                }
            }

            for(int j = 0;j < location.InvestorUpgrades.Count;j += 1) {
                var upgrade = location.InvestorUpgrades[j];
                saveData.worldLocationsToSaveData[i].investorUpgradeDatas[j] = new SaveInvestorUpgradeData {
                    purchased = upgrade.Purchased,
                    whatYouGetText = upgrade.WhatYouGetText
                };
            }
        }

        saveData.saveInventoryItemPickaxeData = new() {
            name = inventoryMenu.pickaxeItemSlot.ItemTemplate?.name,
            count = inventoryMenu.pickaxeItemSlot.Count
        };
        saveData.saveInventoryItemDatas = new SaveInventoryItemData[inventoryMenu.itemSlots.Count];
        for(int i = 0;i < inventoryMenu.itemSlots.Count;i += 1) {
            var slot = inventoryMenu.itemSlots[i];
            saveData.saveInventoryItemDatas[i] = new() { name = slot.ItemTemplate?.name,count = slot.Count };
        }

        var temp = JsonUtility.ToJson(saveData);
        Directory.CreateDirectory(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/save.json",temp);
    }

    public void LoadGame() {
        if(!Directory.Exists(Application.persistentDataPath)) return;
        if(!File.Exists(Application.persistentDataPath + "/save.json")) return;

        var temp = File.ReadAllText(Application.persistentDataPath + "/save.json");
        var savaData = JsonUtility.FromJson<SaveData>(temp);
        for(int i = 0;i < savaData.inventoryResources.Length;i += 1) {
            var item = savaData.inventoryResources[i];
            inventoryMenu.ResourceInstances[item.name].Count = Rational.Parse(item.count);
        }

        for(int i = 0;i < savaData.worldLocationsToSaveData.Length;i += 1) {
            var savedLocation = savaData.worldLocationsToSaveData[i];

            var currentLocation = worldMenu.WorldLocations[i];
            currentLocation.InvestorsToClaim = BigInteger.Parse(savedLocation.investorsToClaim);
            currentLocation.InvestorsYouHave = BigInteger.Parse(savedLocation.investorsYouHave);
            currentLocation.Level = savedLocation.level;
            currentLocation.maxExperience = savedLocation.maxExperience;
            currentLocation.Experience = savedLocation.experience;
            currentLocation.differenceOfMaterial = Rational.Parse(savedLocation.differenceOfMaterial);
            currentLocation.quantityToAddInvestor = Rational.Parse(savedLocation.quantityToAddInvestor);
            currentLocation.mainResourceAutoIncrementTimer = savedLocation.mainResourceAutoIncrementTimer;
            currentLocation.Purchased = savedLocation.purchased;

            for(int j = 0;j < savedLocation.shopItemsToSaveData.Length;j += 1) {
                var item = savedLocation.shopItemsToSaveData[j];
                var found = currentLocation.ShopItems.Find((instance) => instance.name == item.name);
                if(found != null) {
                    found.ResultQuantity = BigInteger.Parse(item.resultQuantity);
                    found.Count = item.count;
                    found.multiplier = Rational.Parse(item.multiplier);
                    found.RecalculateMainResourceAutoIncrement();
                    found.RecalculateMainResourceClickIncrement();
                    for(int k = 0;k < item.shopItemPriceDatas.Length;k += 1) {
                        found.shopItemsPrices[k].Value = BigInteger.Parse(item.shopItemPriceDatas[k].value);
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

        if(inventoryMenu.itemTemplates.ContainsKey(savaData.saveInventoryItemPickaxeData.name)) {
            var pickaxeItemTemplate = inventoryMenu.itemTemplates[savaData.saveInventoryItemPickaxeData.name];
            inventoryMenu.pickaxeItemSlot.ItemTemplate = pickaxeItemTemplate;
            inventoryMenu.pickaxeItemSlot.Count = savaData.saveInventoryItemPickaxeData.count;
        }
        for(int i = 0;i < savaData.saveInventoryItemDatas.Length;i += 1) {
            if(i >= inventoryMenu.itemSlots.Count) break;
            var itemData = savaData.saveInventoryItemDatas[i];
            if(inventoryMenu.itemTemplates.ContainsKey(itemData.name)) {
                var template = inventoryMenu.itemTemplates[itemData.name];
                inventoryMenu.itemSlots[i].ItemTemplate = template;
                inventoryMenu.itemSlots[i].Count = itemData.count;
            }
        }
    }
}
