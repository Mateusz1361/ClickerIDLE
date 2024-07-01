using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

[Serializable]
public class TradeInstanceData
{
    public int price;
    public int adding;
    public int unlocklevel;
    public string icon;
    public string currency;
    public string currencyThatGetsBack;
}

[Serializable]
public class TradeData
{
    public TradeInstanceData[] data;

}

public class TradingPanel : MonoBehaviour
{
    [SerializeField] private ClickerManager clickerManager;
    [SerializeField] private ClickerUI clickerUI;
    [SerializeField]
    private GameObject TradePrefab;
    [SerializeField]
    private GameObject parent;

    public void Trading()
    {
        while (parent.transform.childCount > 0)
        {
            DestroyImmediate(parent.transform.GetChild(0).gameObject);
        }
        var tradesData = JsonUtility.FromJson<TradeData>(File.ReadAllText(Application.streamingAssetsPath + "/TradeData.json"));
        
        foreach (var tradeData in tradesData.data)
        {
            if (tradeData.unlocklevel <= clickerManager.poziom)
            {
                var icon = Resources.Load<Sprite>(tradeData.icon);
                var trade = Instantiate(TradePrefab, parent.transform);
                var instance = trade.GetComponent<TradingInstance>();
                instance.clickerManager = clickerManager;
                instance.clickerUI = clickerUI;
                instance.iconOfTrade.sprite = icon;
                instance.Price = tradeData.price;
                instance.Power = tradeData.adding;
                instance.Currency = tradeData.currency;
                instance.CurrencyThatGetsBack = tradeData.currencyThatGetsBack;
            }
        }
    }
}


