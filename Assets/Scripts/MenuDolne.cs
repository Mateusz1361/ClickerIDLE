using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class MenuDolne : MonoBehaviour
{

    [SerializeField] public ClickerManager clickerManager;

    [Header("Menu Dolne")]
    [SerializeField] public Button shopButton;
    [SerializeField] public Button workerButton;
    [SerializeField] public Button tradeButton;
    [SerializeField] public Button adsButton;
    [SerializeField] public Button eqButton;

    [Header("przyciski zamknij")]
    [SerializeField] public Button shopButtonclose;
    [SerializeField] public Button workerButtonclose;
    [SerializeField] public Button tradeButtonclose;
    [SerializeField] public Button adsButtonclose;
    [SerializeField] public Button eqButtonclose;

    [Header("obrazy przyciskow")]
    [SerializeField] private GameObject shopButtonIMG;
    [SerializeField] private GameObject workerButtonIMG;
    [SerializeField] private GameObject tradeButtonIMG;
    [SerializeField] private GameObject adsButtonIMG;
    [SerializeField] private GameObject eqButtonIMG;

    [Header("TlaTegoGówna")]
    [SerializeField] public GameObject shopTloIMG;
    [SerializeField] public GameObject workerTloIMG;
    [SerializeField] public GameObject tradeTloIMG;
    [SerializeField] public GameObject adsTloIMG;
    [SerializeField] public GameObject eqTloIMG;


    void Start()
    {

        shopTloIMG.SetActive(false);
        workerTloIMG.SetActive(false);
        tradeTloIMG.SetActive(false);
        adsTloIMG.SetActive(false);
        eqTloIMG.SetActive(false);
    }


    public void OpenEQ()
    {
        eqTloIMG.SetActive(true);
    }
    public void CloseEQ()
    {
        eqTloIMG.SetActive(false);
    }




    public void OpenShop()
    {
        shopTloIMG.SetActive(true);
        
    }
    public void CloseShop()
    {
        shopTloIMG.SetActive(false);
    }




    public void OpenWorker()
    {
        workerTloIMG.SetActive(true);
        
    }
    public void CloseWorker()
    {
        workerTloIMG.SetActive(false);
    }




    public void OpenTradingPanel()
    {
        tradeTloIMG.SetActive(true);
        
    }
    public void CloseTradingPanel()
    {
        tradeTloIMG.SetActive(false);
    }




    public void OpenRemovingADS()
    {
        adsTloIMG.SetActive(true);
        
    }
    public void CloseRemovingADS()
    {
        adsTloIMG.SetActive(false);
    }

}
