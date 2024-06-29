using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TradingPanel : MonoBehaviour
{

    [SerializeField] private ClickerUI clickerUI;
    [SerializeField] private ZasobyWEQ zasobyWEQ;
    [SerializeField] public ClickerManager clickerManager;

    [Header("trade Stone na Dolary")]
    [SerializeField] public Button tradingStoneDolars;
    [SerializeField] public TextMeshProUGUI minusStoneText;
    [SerializeField] public TextMeshProUGUI addDolarsText;
    [SerializeField] public TextMeshProUGUI ilosctradowText;
    [SerializeField] public ulong minusStone = 10;
    [SerializeField] public int addDolars = 1;
    [SerializeField] public int ilosctradow = 0;


    public void TradingNumberOne()
    {
        if (clickerManager.stoneCounter >=minusStone)
        {
            clickerManager.stoneCounter -= minusStone;
            clickerManager.dolarCounter += addDolars;
            ilosctradow++;
            //gdyby bylo potrzebne to juz tu jest 
            //zasobyWEQ.UpdateZasoby(clickerManager.wegiel, clickerManager.miedz, clickerManager.zelazo, clickerManager.zloto, clickerManager.diament, clickerManager.emerald);
            clickerUI.UpdateUI(clickerManager.stoneCounter, clickerManager.dolarCounter, clickerManager.poziom);
            TradingNumberOneUpdate();
        }
    }
    public void TradingNumberOneUpdate()
    {
        minusStoneText.text = $"{minusStone}";
        addDolarsText.text = $"{addDolars}";
        ilosctradowText.text = $"{ilosctradow}";

    }
}
