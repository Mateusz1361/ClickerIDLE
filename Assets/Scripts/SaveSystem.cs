using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveShopItemPriceData {
    public string value;
}

[Serializable]
public class SaveShopItemData {
    public string name;
    public string resultQuantity;
    public string count;
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
public class SaveInventoryItemData {
    public string name;
    public string count;
}

[Serializable]
public class SaveData {
    public SaveWorldLocationData[] worldLocationSaveDatas;
    public SaveInventoryItemData[] saveInventoryOreItemDatas;
    public SaveInventoryItemData[] saveInventoryItemDatas;
    public SaveInventoryItemData saveInventoryPickaxeItemData;
    public SaveInventoryItemData saveInventorySwordItemData;
    public SaveInventoryItemData saveInventoryArmorItemData;
    public SaveShopItemData[] shopItems;
    public string dynamite;
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
    [SerializeField]
    private ReferenceHub referenceHub;
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
                saveData.worldLocationSaveDatas[i].shopItemsToSaveData[j] = new SaveShopItemData{
                    name = item.name,
                    resultQuantity = item.ResultQuantity.ToString(),
                    count = item.Count.ToString(),
                    multiplier = item.multiplier.ToString(),
                    shopItemPriceDatas = new SaveShopItemPriceData[item.shopItemsPrices.Count],
                };
                for(int k = 0;k < item.shopItemsPrices.Count;k += 1) {
                    saveData.worldLocationSaveDatas[i].shopItemsToSaveData[j].shopItemPriceDatas[k] = new SaveShopItemPriceData {
                        value = item.shopItemsPrices[k].Value.ToString()
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
                count = slot.Count.ToString()
            };
            index += 1;
        }

        saveData.saveInventoryItemDatas = new SaveInventoryItemData[inventoryMenu.itemSlots.Count];
        for(int i = 0;i < inventoryMenu.itemSlots.Count;i +=1 ) {
            saveData.saveInventoryItemDatas[i] = new() {
                name = inventoryMenu.itemSlots[i].ItemTemplate?.name,
                count = inventoryMenu.itemSlots[i].Count.ToString()
            };
        }
        saveData.saveInventoryPickaxeItemData = new() {
            name = inventoryMenu.pickaxeInventoryItemSlot.ItemTemplate?.name,
            count = inventoryMenu.pickaxeInventoryItemSlot.Count.ToString()
        };
        saveData.saveInventorySwordItemData = new() {
            name = inventoryMenu.swordInventoryItemSlot.ItemTemplate?.name,
            count = inventoryMenu.swordInventoryItemSlot.Count.ToString()
        };
        saveData.saveInventoryArmorItemData = new() {
            name = inventoryMenu.armorInventoryItemSlot.ItemTemplate?.name,
            count = inventoryMenu.armorInventoryItemSlot.Count.ToString()
        };

        saveData.shopItems = new SaveShopItemData[shopMenu.ShopItems.Count];
        for(int i = 0;i < shopMenu.ShopItems.Count;i += 1) {
            saveData.shopItems[i] = new() {
                name = shopMenu.ShopItems[i].name,
                count = shopMenu.ShopItems[i].Count.ToString(),
                resultQuantity = shopMenu.ShopItems[i].ResultQuantity.ToString(),
                multiplier = shopMenu.ShopItems[i].multiplier.ToString(),
                shopItemPriceDatas = new SaveShopItemPriceData[shopMenu.ShopItems[i].shopItemsPrices.Count]
            };
            for(int j = 0;j< shopMenu.ShopItems[i].shopItemsPrices.Count;j += 1) {
                saveData.shopItems[i].shopItemPriceDatas[j] = new() {
                    value = shopMenu.ShopItems[i].shopItemsPrices[j].Value.ToString()
                };
            }
        }
        saveData.dynamite = referenceHub.currentWorldLocationMenu.choosingDynamite;

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
            currentLocation.InvestorsToClaim = SafeUDecimal.Parse(savedLocation.investorsToClaim);
            currentLocation.InvestorsYouHave = SafeUDecimal.Parse(savedLocation.investorsYouHave);
            currentLocation.Level = savedLocation.level;
            currentLocation.maxExperience = savedLocation.maxExperience;
            currentLocation.Experience = savedLocation.experience;
            currentLocation.differenceOfMaterial = SafeUDecimal.Parse(savedLocation.differenceOfMaterial);
            currentLocation.quantityToAddInvestor = SafeUInteger.Parse(savedLocation.quantityToAddInvestor);
            currentLocation.mainResourceAutoIncrementTimer = savedLocation.mainResourceAutoIncrementTimer;
            currentLocation.Purchased = savedLocation.purchased;

            for(int j = 0;j < savedLocation.shopItemsToSaveData.Length;j += 1) {
                var item = savedLocation.shopItemsToSaveData[j];
                var found = currentLocation.ShopItems.Find((instance) => instance.name == item.name);
                if(found != null) {
                    found.ResultQuantity = SafeUInteger.Parse(item.resultQuantity);
                    found.Count = SafeUInteger.Parse(item.count);
                    found.multiplier = SafeUInteger.Parse(item.multiplier);
                    found.RecalculateMainResourceAutoIncrement();
                    found.RecalculateMainResourceClickIncrement();
                    for(int k = 0;k < item.shopItemPriceDatas.Length;k += 1) {
                        found.shopItemsPrices[k].Value = SafeUInteger.Parse(item.shopItemPriceDatas[k].value);
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
                inventoryMenu.OreItemsSlots[itemData.name].Count = SafeUDecimal.Parse(itemData.count);
            }
        }

        for(int i = 0;i < saveData.saveInventoryItemDatas.Length;i += 1) {
            var itemData = saveData.saveInventoryItemDatas[i];
            if(itemData.name != null && inventoryMenu.ItemTemplates.ContainsKey(itemData.name) && i < inventoryMenu.itemSlots.Count) {
                inventoryMenu.itemSlots[i].ItemTemplate = inventoryMenu.ItemTemplates[itemData.name];
                inventoryMenu.itemSlots[i].Count = SafeUDecimal.Parse(itemData.count);
            }
        }
        if(inventoryMenu.ItemTemplates.ContainsKey(saveData.saveInventoryPickaxeItemData.name)) {
            inventoryMenu.pickaxeInventoryItemSlot.ItemTemplate = inventoryMenu.ItemTemplates[saveData.saveInventoryPickaxeItemData.name];
            inventoryMenu.pickaxeInventoryItemSlot.Count = SafeUDecimal.Parse(saveData.saveInventoryPickaxeItemData.count);
        }
        if(inventoryMenu.ItemTemplates.ContainsKey(saveData.saveInventorySwordItemData.name)) {
            inventoryMenu.swordInventoryItemSlot.ItemTemplate = inventoryMenu.ItemTemplates[saveData.saveInventorySwordItemData.name];
            inventoryMenu.swordInventoryItemSlot.Count = SafeUDecimal.Parse(saveData.saveInventorySwordItemData.count);
        }
        if(inventoryMenu.ItemTemplates.ContainsKey(saveData.saveInventoryArmorItemData.name)) {
            inventoryMenu.armorInventoryItemSlot.ItemTemplate = inventoryMenu.ItemTemplates[saveData.saveInventoryArmorItemData.name];
            inventoryMenu.armorInventoryItemSlot.Count = SafeUDecimal.Parse(saveData.saveInventoryArmorItemData.count);
        }

        for(int i = 0;i < saveData.shopItems.Length;i += 1) {
            var item = saveData.shopItems[i];
            var found = shopMenu.ShopItems.Find((instance) => instance.name == item.name);
            if(found != null) {
                found.ResultQuantity = SafeUInteger.Parse(item.resultQuantity);
                found.Count = SafeUInteger.Parse(item.count);
                found.multiplier = SafeUInteger.Parse(item.multiplier);
                found.RecalculateMainResourceAutoIncrement();
                found.RecalculateMainResourceClickIncrement();
                for(int j = 0;j < item.shopItemPriceDatas.Length;j += 1) {
                    found.shopItemsPrices[j].Value = SafeUInteger.Parse(item.shopItemPriceDatas[j].value);
                }
            }
        }
        referenceHub.currentWorldLocationMenu.choosingDynamite = saveData.dynamite;
    }
}
