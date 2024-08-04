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
public class SaveInventoryData {
    public string name;
    public string count;
}

[Serializable]
public class SaveData {
    public SaveInventoryData[] inventoryToSaveData;
    public SaveWorldLocationData[] worldLocationsToSaveData;
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

    public void SaveGame() {
        SaveData saveData = new();
        saveData.inventoryToSaveData = new SaveInventoryData[inventoryMenu.ResourceInstances.Count];
        int index = 0;
        foreach((var name,var resource) in inventoryMenu.ResourceInstances) { 
            saveData.inventoryToSaveData[index] = new SaveInventoryData{name=name,count=resource.Count.ToString()};
            index++;
        }
        saveData.worldLocationsToSaveData = new SaveWorldLocationData[worldMenu.WorldLocations.Count];
        for (int i =0; i<worldMenu.WorldLocations.Count;i++) {
            var location = worldMenu.WorldLocations[i];

            saveData.worldLocationsToSaveData[i] = new() {
                level = location.Level,
                experience = location.Experience,
                maxExperience = location.maxExperience,
                differenceOfMaterial = location.differenceOfMaterial.ToString(),
                quantityToAddInvestor = location.quantityToAddInvestor.ToString(),
                mainResourceAutoIncrement = location.MainResourceAutoIncrement.ToString(),
                mainResourceAutoIncrementTimer = location.mainResourceAutoIncrementTimer,
                investorsToClaim = location.InvestorsToClaim.ToString(),
                investorsYouHave = location.InvestorsYouHave.ToString(),
                purchased = location.Purchased,
                name = location.Name,
                shopItemsToSaveData = new SaveShopItemData[location.ShopItems.Count],
                investorUpgradeDatas = new SaveInvestorUpgradeData[location.InvestorUpgrades.Count]
            };

            for (int j=0; j<location.ShopItems.Count;j++)
            {
                var item = location.ShopItems[j];
                saveData.worldLocationsToSaveData[i].shopItemsToSaveData[j] = new SaveShopItemData{
                    name=item.name,
                    resultQuantity=item.ResultQuantity.ToString(),
                    count=item.Count,
                    clickIncrement = item.mainResourceClickIncrement.ToString(),
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

        var temp = JsonUtility.ToJson(saveData);
        Directory.CreateDirectory(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/save.json",temp);
    }

    public void LoadGame()
    {
        if (!Directory.Exists(Application.persistentDataPath)) { return; }
        if(!File.Exists(Application.persistentDataPath + "/save.json")) { return; }
        var temp = File.ReadAllText(Application.persistentDataPath + "/save.json");
        var savaData = JsonUtility.FromJson<SaveData>(temp);
        for (int i = 0; i < savaData.inventoryToSaveData.Length; i++)
        {
            var item = savaData.inventoryToSaveData[i];
            inventoryMenu.ResourceInstances[item.name].Count = Rational.Parse(item.count);
        }
        for (int i = 0;i<savaData.worldLocationsToSaveData.Length; i++)
        {
            var location = savaData.worldLocationsToSaveData[i];
            worldMenu.WorldLocations[i].InvestorsToClaim = BigInteger.Parse(location.investorsToClaim);
            worldMenu.WorldLocations[i].InvestorsYouHave = BigInteger.Parse(location.investorsYouHave);
            worldMenu.WorldLocations[i].Level = location.level;
            worldMenu.WorldLocations[i].maxExperience = location.maxExperience;
            worldMenu.WorldLocations[i].Experience = location.experience;
            worldMenu.WorldLocations[i].differenceOfMaterial = Rational.Parse(location.differenceOfMaterial);
            worldMenu.WorldLocations[i].quantityToAddInvestor = Rational.Parse(location.quantityToAddInvestor);
            worldMenu.WorldLocations[i].MainResourceAutoIncrement = Rational.Parse(location.mainResourceAutoIncrement);
            worldMenu.WorldLocations[i].mainResourceAutoIncrementTimer = location.mainResourceAutoIncrementTimer;
            worldMenu.WorldLocations[i].Purchased = location.purchased;
            for (int j = 0; j < location.shopItemsToSaveData.Length; j++)
            {
                var item = location.shopItemsToSaveData[j];
                var found = worldMenu.WorldLocations[i].ShopItems.Find((instance) => instance.name == item.name);
                if(found != null) {
                    found.ResultQuantity = BigInteger.Parse(item.resultQuantity);
                    found.Count = item.count;
                    found.multiplier = Rational.Parse(item.multiplier);
                    found.mainResourceClickIncrement = Rational.Parse(item.clickIncrement);

                    for(int k = 0;k < item.shopItemPriceDatas.Length;k += 1) {
                        found.shopItemsPrices[k].Value = BigInteger.Parse(item.shopItemPriceDatas[k].value);
                    }
                }
            }
            for(int j = 0;j < location.investorUpgradeDatas.Length;j += 1) {
                var upgrade = location.investorUpgradeDatas[j];
                var found = worldMenu.WorldLocations[i].InvestorUpgrades.Find((instance) => instance.WhatYouGetText == upgrade.whatYouGetText);
                if(found != null) {
                    found.Purchased = upgrade.purchased;
                }
            }
            investorMenu.UpdateInvestors();
        }
    }

    private void OnApplicationQuit() {
        SaveGame();
    }
}
