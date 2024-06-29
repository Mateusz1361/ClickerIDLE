using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesUI : MonoBehaviour
{

    [SerializeField] private ClickerUI clickerUI;
    [SerializeField] private ZasobyWEQ zasobyWEQ;
    [SerializeField] public ClickerManager clickerManager;


    [Header("Przyciski")]
    [SerializeField] public Button upgradeKielnia;
    [SerializeField] public Button upgradeDluto;
    [SerializeField] public Button upgradeIron;
    [SerializeField] public Button upgradeGold;
    [SerializeField] public Button upgradeDiamond;
    [SerializeField] public Button upgradeEmerald;


    [Header("Mnozniki")]
    [SerializeField] public int mnoznikWegla;
    [SerializeField] public int mnoznikMiedzi;
    [SerializeField] public int mnoznikZelaza;
    [SerializeField] public int mnoznikZlota;
    [SerializeField] public int mnoznikDiamentu;
    [SerializeField] public int mnoznikEmeraldu;


    [Header("Kielnia")]
    [SerializeField] public int cenaKielnia = 10;
    [SerializeField] public int mocKielni = 2;
    [SerializeField] public int iloscKielni = 0;


    [Header("Dluto")]
    [SerializeField] public int cenaDluta = 20;
    [SerializeField] public int mocDluta = 5;
    [SerializeField] public int iloscDlut = 0;


    [Header("M³otek górniczy")]
    [SerializeField] public int cenaMlotkaGórniczego = 50;
    [SerializeField] public int mocMlotkaGórniczego = 15;
    [SerializeField] public int iloscMlotkowGórniczych = 0;


    [Header("D³uto skalne")]
    [SerializeField] public int cenaDlutaSkalnego = 200;
    [SerializeField] public int mocDlutaSkalnego = 50;
    [SerializeField] public int iloscDlutSkalnego = 0;


    [Header("£opata górnicza")]
    [SerializeField] public int cenaLopataGorniczej = 500;
    [SerializeField] public int mocLopataGorniczej = 120;
    [SerializeField] public int iloscLopatGorniczych = 0;


    [Header("Wiert³o do ska³")]
    [SerializeField] public int cenaWiertlaSkalnego = 2000;
    [SerializeField] public int mocWiertlaSkalnego = 450;
    [SerializeField] public int iloscWiertelSkalnych = 0;

    public void BuyingKielnia()
    {
        if(Convert.ToUInt32(clickerManager.stoneCounter) >= cenaKielnia && iloscKielni<100)
        {
            clickerManager.stoneCounter -= Convert.ToUInt64(cenaKielnia);
            cenaKielnia *= 2;
            iloscKielni++;
            clickerManager.addStone += mocKielni;
            clickerUI.UpdateUI(clickerManager.stoneCounter, clickerManager.dolarCounter, clickerManager.poziom);
            zasobyWEQ.UpdateZasoby(clickerManager.wegiel, clickerManager.miedz, clickerManager.zelazo, clickerManager.zloto, clickerManager.diament, clickerManager.emerald);
            clickerUI.KielniaUI(cenaKielnia, mocKielni, iloscKielni);
        }

    }


    public void BuyingDluto()
    {
        if (Convert.ToUInt32(clickerManager.stoneCounter) >= cenaDluta && iloscDlut < 100)
        {
            clickerManager.stoneCounter -= Convert.ToUInt64(cenaDluta);
            cenaDluta *= 2;
            iloscDlut++;
            clickerManager.addStone += mocDluta;
            clickerUI.UpdateUI(clickerManager.stoneCounter, clickerManager.dolarCounter, clickerManager.poziom);
            zasobyWEQ.UpdateZasoby(clickerManager.wegiel, clickerManager.miedz, clickerManager.zelazo, clickerManager.zloto, clickerManager.diament, clickerManager.emerald);
            clickerUI.DlutoUI(cenaDluta, mocDluta, iloscDlut);
        }

    }/// <summary>
    /// //cena i inne wymyslic za co kupowac 
    /// 
    /// </summary>

    public void BuyingMlotkaGórniczego()
    {
        if (Convert.ToUInt32(clickerManager.stoneCounter) >= cenaDluta && iloscDlut < 100)
        {
            clickerManager.stoneCounter -= Convert.ToUInt64(cenaDluta);
            cenaDluta *= 2;
            iloscDlut++;
            clickerManager.addStone += mocDluta;
            clickerUI.UpdateUI(clickerManager.stoneCounter, clickerManager.dolarCounter, clickerManager.poziom);
            zasobyWEQ.UpdateZasoby(clickerManager.wegiel, clickerManager.miedz, clickerManager.zelazo, clickerManager.zloto, clickerManager.diament, clickerManager.emerald);
            clickerUI.DlutoUI(cenaDluta, mocDluta, iloscDlut);
        }

    }

    public void BuyingDlutaSkalnego()
    {
        if (Convert.ToUInt32(clickerManager.stoneCounter) >= cenaDluta && iloscDlut < 100)
        {
            clickerManager.stoneCounter -= Convert.ToUInt64(cenaDluta);
            cenaDluta *= 2;
            iloscDlut++;
            clickerManager.addStone += mocDluta;
            clickerUI.UpdateUI(clickerManager.stoneCounter, clickerManager.dolarCounter, clickerManager.poziom);
            zasobyWEQ.UpdateZasoby(clickerManager.wegiel, clickerManager.miedz, clickerManager.zelazo, clickerManager.zloto, clickerManager.diament, clickerManager.emerald);
            clickerUI.DlutoUI(cenaDluta, mocDluta, iloscDlut);
        }

    }

    public void BuyingLopataGorniczej()
    {
        if (Convert.ToUInt32(clickerManager.stoneCounter) >= cenaDluta && iloscDlut < 100)
        {
            clickerManager.stoneCounter -= Convert.ToUInt64(cenaDluta);
            cenaDluta *= 2;
            iloscDlut++;
            clickerManager.addStone += mocDluta;
            clickerUI.UpdateUI(clickerManager.stoneCounter, clickerManager.dolarCounter, clickerManager.poziom);
            zasobyWEQ.UpdateZasoby(clickerManager.wegiel, clickerManager.miedz, clickerManager.zelazo, clickerManager.zloto, clickerManager.diament, clickerManager.emerald);
            clickerUI.DlutoUI(cenaDluta, mocDluta, iloscDlut);
        }

    }

    public void BuyingWiertlaSkalnego()
    {
        if (Convert.ToUInt32(clickerManager.stoneCounter) >= cenaDluta && iloscDlut < 100)
        {
            clickerManager.stoneCounter -= Convert.ToUInt64(cenaDluta);
            cenaDluta *= 2;
            iloscDlut++;
            clickerManager.addStone += mocDluta;
            clickerUI.UpdateUI(clickerManager.stoneCounter, clickerManager.dolarCounter, clickerManager.poziom);
            zasobyWEQ.UpdateZasoby(clickerManager.wegiel, clickerManager.miedz, clickerManager.zelazo, clickerManager.zloto, clickerManager.diament, clickerManager.emerald);
            clickerUI.DlutoUI(cenaDluta, mocDluta, iloscDlut);
        }

    }

}   
