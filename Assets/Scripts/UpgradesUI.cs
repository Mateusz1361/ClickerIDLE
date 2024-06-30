using System;
using System.IO;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
[Serializable]
public class ShopInstanceData
{
    public int price;
    public int power;
    public string icon;
    public int unlocklevel;
}

[Serializable]
public class ShopData
{
    public ShopInstanceData[] data;

}
public class UpgradesUI : MonoBehaviour
{

    [SerializeField] private ClickerUI clickerUI;
    [SerializeField] private ZasobyWEQ zasobyWEQ;
    [SerializeField] public ClickerManager clickerManager;
    [SerializeField] private GameObject shopPrefab;
    [SerializeField] private GameObject parent;
    
    
    public void Shoping()
    {
        List<int> quantities = new List<int>();
        foreach (var prefab in parent.GetComponentsInChildren<ShopInstance>()) { 
            quantities.Add(prefab.Quantity);
        }
        while (parent.transform.childCount>0)
        {
            DestroyImmediate(parent.transform.GetChild(0));
        }
        var ShopData = JsonUtility.FromJson<ShopData>(File.ReadAllText(Application.streamingAssetsPath + "/ShopData.json"));
        int index = 0;
        foreach (var shopData in ShopData.data)
        {
            
            var Shop = Instantiate(shopPrefab, parent.transform);
            var instance = Shop.GetComponent<ShopInstance>();
            if (shopData.unlocklevel == clickerManager.poziom)
            {
                instance.clickerManager = clickerManager;
                instance.clickerUI = clickerUI;
                instance.Price = shopData.price;
                instance.Power = shopData.power;
                instance.Quantity = 0;
                if (index < quantities.Count) { 
                    instance.Quantity = quantities[index];

                
                }
            }
            index += 1;    
        }
    }


}   
