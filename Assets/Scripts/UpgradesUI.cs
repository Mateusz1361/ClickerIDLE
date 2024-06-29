using System;
using System.IO;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
[Serializable]
public class ShopInstanceData
{
    public int price;
    public int power;
    public string icon;
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

    private void Start()
    {

        var ShopData = JsonUtility.FromJson<ShopData>(File.ReadAllText(Application.streamingAssetsPath + "/ShopData.json"));
        foreach (var shopData in ShopData.data)
        {
            
            var Shop = Instantiate(shopPrefab, parent.transform);
            var instance = Shop.GetComponent<ShopInstance>();
            instance.clickerManager = clickerManager;
            
            instance.Price = shopData.price;
            instance.Power = shopData.power;
            instance.Quantity = 0;
        }
    }


}   
