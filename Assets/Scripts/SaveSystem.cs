using System;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveShopItemPriceDataJson {
    public string value;
}

[Serializable]
public class SaveShopItemDataJson {
    public string name;
    public string resultQuantity;
    public string count;
    public string multiplier;
    public SaveShopItemPriceDataJson[] shopItemPriceDatas;
}

[Serializable]
public class SaveInvestorUpgradeDataJson {
    public string whatYouGetText;
    public bool purchased;
}

[Serializable]
public class SaveWorldLocationDataJson {
    public string name;
    public bool purchased;
    public SaveShopItemDataJson[] shopItemsToSaveData;
    public string investorsToClaim;
    public string investorsYouHave;
    public ulong level;
    public double experience;
    public double maxExperience;
    public string differenceOfMaterial;
    public string quantityToAddInvestor;
    public string mainResourceAutoIncrement;
    public float mainResourceAutoIncrementTimer;
    public SaveInvestorUpgradeDataJson[] investorUpgradeDatas;
}

[Serializable]
public class SaveInventoryItemDataJson {
    public string name;
    public string count;
}

[Serializable]
public class SaveDataJson {
    public SaveWorldLocationDataJson[] worldLocationSaveDatas;
    public SaveInventoryItemDataJson[] saveInventoryOreItemDatas;
    public SaveInventoryItemDataJson[] saveInventoryItemDatas;
    public SaveInventoryItemDataJson saveInventoryPickaxeItemData;
    public SaveInventoryItemDataJson saveInventorySwordItemData;
    public SaveInventoryItemDataJson saveInventoryArmorItemData;
    public SaveShopItemDataJson[] shopItems;
}

public class SaveSystem : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;

    private void OnApplicationQuit() {
        SaveGame();
    }

    public void SaveGame() {
        SaveDataJson saveData = new() {
            worldLocationSaveDatas = new SaveWorldLocationDataJson[referenceHub.worldMenu.WorldLocations.Count]
        };
        for(int i = 0;i < referenceHub.worldMenu.WorldLocations.Count;i += 1) {
            var location = referenceHub.worldMenu.WorldLocations[i];

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
                shopItemsToSaveData = new SaveShopItemDataJson[location.ShopItems.Count],
                investorUpgradeDatas = new SaveInvestorUpgradeDataJson[location.InvestorUpgrades.Count]
                
            };

            for(int j = 0;j < location.ShopItems.Count;j += 1) {
                var item = location.ShopItems[j];
                saveData.worldLocationSaveDatas[i].shopItemsToSaveData[j] = new SaveShopItemDataJson{
                    name = item.name,
                    resultQuantity = item.ResultQuantity.ToString(),
                    count = item.Count.ToString(),
                    multiplier = item.multiplier.ToString(),
                    shopItemPriceDatas = new SaveShopItemPriceDataJson[item.shopItemsPrices.Count],
                };
                for(int k = 0;k < item.shopItemsPrices.Count;k += 1) {
                    saveData.worldLocationSaveDatas[i].shopItemsToSaveData[j].shopItemPriceDatas[k] = new SaveShopItemPriceDataJson {
                        value = item.shopItemsPrices[k].Value.ToString()
                    };
                }
            }

            for(int j = 0;j < location.InvestorUpgrades.Count;j += 1) {
                var upgrade = location.InvestorUpgrades[j];
                saveData.worldLocationSaveDatas[i].investorUpgradeDatas[j] = new SaveInvestorUpgradeDataJson {
                    purchased = upgrade.Purchased,
                    whatYouGetText = upgrade.WhatYouGetText
                };
            }
        }

        saveData.saveInventoryOreItemDatas = new SaveInventoryItemDataJson[referenceHub.inventoryMenu.OreItemsSlots.Count];
        int index = 0;
        foreach(var slot in referenceHub.inventoryMenu.OreItemsSlots.Values) {
            saveData.saveInventoryOreItemDatas[index] = new() {
                name = slot.ItemTemplate.name,
                count = slot.Count.ToString()
            };
            index += 1;
        }

        saveData.saveInventoryItemDatas = new SaveInventoryItemDataJson[referenceHub.inventoryMenu.itemSlots.Count];
        for(int i = 0;i < referenceHub.inventoryMenu.itemSlots.Count;i +=1 ) {
            saveData.saveInventoryItemDatas[i] = new() {
                name = referenceHub.inventoryMenu.itemSlots[i].ItemTemplate?.name,
                count = referenceHub.inventoryMenu.itemSlots[i].Count.ToString()
            };
        }
        saveData.saveInventoryPickaxeItemData = new() {
            name = referenceHub.inventoryMenu.pickaxeInventoryItemSlot.ItemTemplate?.name,
            count = referenceHub.inventoryMenu.pickaxeInventoryItemSlot.Count.ToString()
        };
        saveData.saveInventorySwordItemData = new() {
            name = referenceHub.inventoryMenu.swordInventoryItemSlot.ItemTemplate?.name,
            count = referenceHub.inventoryMenu.swordInventoryItemSlot.Count.ToString()
        };
        saveData.saveInventoryArmorItemData = new() {
            name = referenceHub.inventoryMenu.armorInventoryItemSlot.ItemTemplate?.name,
            count = referenceHub.inventoryMenu.armorInventoryItemSlot.Count.ToString()
        };

        saveData.shopItems = new SaveShopItemDataJson[referenceHub.shopMenu.ShopItems.Count];
        for(int i = 0;i < referenceHub.shopMenu.ShopItems.Count;i += 1) {
            saveData.shopItems[i] = new() {
                name = referenceHub.shopMenu.ShopItems[i].name,
                count = referenceHub.shopMenu.ShopItems[i].Count.ToString(),
                resultQuantity = referenceHub.shopMenu.ShopItems[i].ResultQuantity.ToString(),
                multiplier = referenceHub.shopMenu.ShopItems[i].multiplier.ToString(),
                shopItemPriceDatas = new SaveShopItemPriceDataJson[referenceHub.shopMenu.ShopItems[i].shopItemsPrices.Count]
            };
            for(int j = 0;j< referenceHub.shopMenu.ShopItems[i].shopItemsPrices.Count;j += 1) {
                saveData.shopItems[i].shopItemPriceDatas[j] = new() {
                    value = referenceHub.shopMenu.ShopItems[i].shopItemsPrices[j].Value.ToString()
                };
            }
        }

        var temp = JsonUtility.ToJson(saveData);
        Directory.CreateDirectory(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/save.json",temp);
    }

    public void LoadGame() {
        if(!Directory.Exists(Application.persistentDataPath)) return;
        if(!File.Exists(Application.persistentDataPath + "/save.json")) return;

        var temp = File.ReadAllText(Application.persistentDataPath + "/save.json");
        var saveData = JsonUtility.FromJson<SaveDataJson>(temp);

        for(int i = 0;i < saveData.worldLocationSaveDatas.Length;i += 1) {
            var savedLocation = saveData.worldLocationSaveDatas[i];

            var currentLocation = referenceHub.worldMenu.WorldLocations[i];
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
            referenceHub.investorMenu.UpdateInvestors();
            currentLocation.RecalculateMainResourceClickIncrement();
            currentLocation.RecalculateMainResourceAutoIncrement();
        }

        for(int i = 0;i < saveData.saveInventoryOreItemDatas.Length;i += 1) {
            var itemData = saveData.saveInventoryOreItemDatas[i];
            if(referenceHub.inventoryMenu.OreItemsSlots.ContainsKey(itemData.name)) {
                referenceHub.inventoryMenu.OreItemsSlots[itemData.name].Count = SafeUDecimal.Parse(itemData.count);
            }
        }

        for(int i = 0;i < saveData.saveInventoryItemDatas.Length;i += 1) {
            var itemData = saveData.saveInventoryItemDatas[i];
            if(itemData.name != null && referenceHub.inventoryMenu.ItemTemplates.ContainsKey(itemData.name) && i < referenceHub.inventoryMenu.itemSlots.Count) {
                referenceHub.inventoryMenu.itemSlots[i].ItemTemplate = referenceHub.inventoryMenu.ItemTemplates[itemData.name];
                referenceHub.inventoryMenu.itemSlots[i].Count = SafeUDecimal.Parse(itemData.count);
            }
        }
        if(referenceHub.inventoryMenu.ItemTemplates.ContainsKey(saveData.saveInventoryPickaxeItemData.name)) {
            referenceHub.inventoryMenu.pickaxeInventoryItemSlot.ItemTemplate = referenceHub.inventoryMenu.ItemTemplates[saveData.saveInventoryPickaxeItemData.name];
            referenceHub.inventoryMenu.pickaxeInventoryItemSlot.Count = SafeUDecimal.Parse(saveData.saveInventoryPickaxeItemData.count);
        }
        if(referenceHub.inventoryMenu.ItemTemplates.ContainsKey(saveData.saveInventorySwordItemData.name)) {
            referenceHub.inventoryMenu.swordInventoryItemSlot.ItemTemplate = referenceHub.inventoryMenu.ItemTemplates[saveData.saveInventorySwordItemData.name];
            referenceHub.inventoryMenu.swordInventoryItemSlot.Count = SafeUDecimal.Parse(saveData.saveInventorySwordItemData.count);
        }
        if(referenceHub.inventoryMenu.ItemTemplates.ContainsKey(saveData.saveInventoryArmorItemData.name)) {
            referenceHub.inventoryMenu.armorInventoryItemSlot.ItemTemplate = referenceHub.inventoryMenu.ItemTemplates[saveData.saveInventoryArmorItemData.name];
            referenceHub.inventoryMenu.armorInventoryItemSlot.Count = SafeUDecimal.Parse(saveData.saveInventoryArmorItemData.count);
        }

        for(int i = 0;i < saveData.shopItems.Length;i += 1) {
            var item = saveData.shopItems[i];
            var found = referenceHub.shopMenu.ShopItems.Find((instance) => instance.name == item.name);
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
    }
}
