using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using JetBrains.Annotations;
using System.Numerics;
[Serializable]
public class SaveShopItemData
{
    public int index;
    public string resultQuantity;
    public ulong count;
    public string clickIncrement;
    public string multiplier;
}
[Serializable]
public class SaveWorldLocationData
{
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
    public string mainResourceClickIncrement;
    public string mainResourceAutoIncrement;
    public float mainResourceAutoIncrementTimer;
}
[Serializable]
public class SaveInventoryData
{
    public string name;
    public string count;
}
[Serializable]
public class SaveData
{
    public SaveInventoryData[] inventoryToSaveData;
    public SaveWorldLocationData[] worldLocationsToSaveData;
}
public class SaveSystem : MonoBehaviour
{
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
        foreach ((var name, var resource) in inventoryMenu.ResourceInstances) { 
            saveData.inventoryToSaveData[index] = new SaveInventoryData{name=name, count=resource.Count.ToString()};
            index++;
        }
        saveData.worldLocationsToSaveData = new SaveWorldLocationData[worldMenu.WorldLocations.Count];
        for (int i =0; i<worldMenu.WorldLocations.Count;i++) {
            var location = worldMenu.WorldLocations[i];
            
            saveData.worldLocationsToSaveData[i] = new();
            saveData.worldLocationsToSaveData[i].level= location.Level;
            saveData.worldLocationsToSaveData[i].experience= location.Experience;
            saveData.worldLocationsToSaveData[i].maxExperience=location.maxExperience;
            saveData.worldLocationsToSaveData[i].differenceOfMaterial=location.differenceOfMaterial.ToString();
            saveData.worldLocationsToSaveData[i].quantityToAddInvestor = location.quantityToAddInvestor.ToString();
            //saveData.worldLocationsToSaveData[i].mainResourceClickIncrement=location.mainResourceClickIncrement.ToString();
            saveData.worldLocationsToSaveData[i].mainResourceAutoIncrement=location.MainResourceAutoIncrement.ToString();
            saveData.worldLocationsToSaveData[i].mainResourceAutoIncrementTimer = location.mainResourceAutoIncrementTimer;
            saveData.worldLocationsToSaveData[i].investorsToClaim = location.InvestorsToClaim.ToString();
            saveData.worldLocationsToSaveData[i].investorsYouHave = location.InvestorsYouHave.ToString();
            saveData.worldLocationsToSaveData[i].purchased = location.Purchased;
            saveData.worldLocationsToSaveData[i].name = location.Name;
            saveData.worldLocationsToSaveData[i].shopItemsToSaveData = new SaveShopItemData[location.ShopItems.Count];

            for (int j=0; j<location.ShopItems.Count;j++)
            {
                var item = location.ShopItems[j];
                saveData.worldLocationsToSaveData[i].shopItemsToSaveData[j] = new SaveShopItemData{index=item.indexOfShopItem,resultQuantity=item.ResultQuantity.ToString(),count=item.Count,clickIncrement = item.mainResourceClickIncrement.ToString(),multiplier = item.multiplier.ToString()};
            }
        }

        var temp = JsonUtility.ToJson(saveData);
        Directory.CreateDirectory(Application.persistentDataPath);
        File.WriteAllText(Application.persistentDataPath + "/save.json",temp);
    }
    public void LoadGame()
    {
        if (!Directory.Exists(Application.persistentDataPath)) {  return; }
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
            //worldMenu.WorldLocations[i].mainResourceClickIncrement = Rational.Parse(location.mainResourceClickIncrement);
            worldMenu.WorldLocations[i].MainResourceAutoIncrement = Rational.Parse(location.mainResourceAutoIncrement);
            worldMenu.WorldLocations[i].mainResourceAutoIncrementTimer = location.mainResourceAutoIncrementTimer;
            worldMenu.WorldLocations[i].Purchased = location.purchased;
            for (int j = 0; j < location.shopItemsToSaveData.Length; j++)
            {
                var item = location.shopItemsToSaveData[j];
                worldMenu.WorldLocations[i].ShopItems[j].indexOfShopItem = item.index;
                worldMenu.WorldLocations[i].ShopItems[j].ResultQuantity = BigInteger.Parse(item.resultQuantity);
                worldMenu.WorldLocations[i].ShopItems[j].Count = item.count;
                worldMenu.WorldLocations[i].ShopItems[j].multiplier = Rational.Parse(item.multiplier);
                worldMenu.WorldLocations[i].ShopItems[j].mainResourceClickIncrement = Rational.Parse(item.clickIncrement);

                for (ulong c = 0; c < item.count; c++)
                {
                    for (int k = 0; k < worldMenu.WorldLocations[i].ShopItems[j].shopItemsPrices.Count; k++)
                    {
                        if (worldMenu.WorldLocations[i].ShopItems[j].shopItemsPrices[k].UnlockCount <= c)
                        {
                            worldMenu.WorldLocations[i].ShopItems[j].shopItemsPrices[k].Value *= 2;
                        }
                    }
                }
                
            }
            investorMenu.UpdateInvestors();
        }
    }
    private void OnApplicationQuit() {
        SaveGame();
    }
}
