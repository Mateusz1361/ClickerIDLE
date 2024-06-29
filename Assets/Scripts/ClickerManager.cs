using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ClickerManager : MonoBehaviour
{
    public static UnityEvent<ulong,ulong,ulong> OnItemBought = new UnityEvent<ulong,ulong, ulong>();


    [Header("Glowne")]
    [SerializeField] private ClickerUI clickerUI;
    [SerializeField] private Pracownik pracownik;
    [SerializeField] private TradingPanel tradingPanel;
    [SerializeField] private MenuDolne menuDOLNE;
    [SerializeField] private ZasobyWEQ zasobyWEQ;
    [SerializeField] private UpgradesUI upgradesUI;
    [SerializeField] private GameObject zasobyTlo;
    [SerializeField] private Slider slider;
    [SerializeField] public GameObject poziomIMG;
    [SerializeField] private GameObject poziomLVLIMG;



    [Header("Przyciski")]
    [SerializeField] private Button clickerButton;
    [SerializeField] private Button zasobyButton;
    [SerializeField] private Button zasobyButtonClose;
    [SerializeField] private Button lvling;

    [SerializeField]
    private TextMeshProUGUI StonePerClickText;


    // zmienne og�lne
    public ulong stoneCounter = 0;
    public int addStone = 1;

    public ulong stonePerSecond = 0;

    public int dolarCounter = 0;
    int addDolar = 1;

    
    // zmienne od expienia
    public int poziom = 1;
    public float experience = 0f;
    float _minExperience = 30f;
    float _maxExperience = 60f;


    //zmienne do Dropu Dolarow
    float _minDrop = 1.01f;
    float _maxDrop = 1.02f;

    //Zasoby
    public int wegiel = 0;
    public int miedz = 0;
    public int zelazo = 0;
    public int zloto = 0;
    public int diament = 0;
    public int emerald = 0;

    public int addwegiel = 1;
    public int addmiedz = 1;
    public int addzelazo = 1;
    public int addzloto = 1;
    public int adddiament = 1;
    public int addemerald = 1;

    // G��wna funkcja START
    private void Start()
    {
        clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);
        zasobyWEQ.UpdateZasoby(wegiel, miedz, zelazo, zloto, diament, emerald);
        
        clickerButton.onClick.AddListener(AddStone);

        zasobyTlo.SetActive(false);
        poziomLVLIMG.SetActive(false);
        
      

        zasobyButton.onClick.AddListener(OpenZas);
        zasobyButtonClose.onClick.AddListener(CloseZas);

        lvling.onClick.AddListener(LVLUP);

        menuDOLNE.eqButton.onClick.AddListener(menuDOLNE.OpenEQ);
        menuDOLNE.eqButtonclose.onClick.AddListener(menuDOLNE.CloseEQ);

        menuDOLNE.shopButton.onClick.AddListener(menuDOLNE.OpenShop);
        menuDOLNE.shopButtonclose.onClick.AddListener(menuDOLNE.CloseShop);

        menuDOLNE.workerButton.onClick.AddListener(menuDOLNE.OpenWorker);
        menuDOLNE.workerButtonclose.onClick.AddListener(menuDOLNE.CloseWorker);

        menuDOLNE.tradeButton.onClick.AddListener(menuDOLNE.OpenTradingPanel);
        menuDOLNE.tradeButtonclose.onClick.AddListener(menuDOLNE.CloseTradingPanel);

        menuDOLNE.adsButton.onClick.AddListener(menuDOLNE.OpenRemovingADS);
        menuDOLNE.adsButtonclose.onClick.AddListener(menuDOLNE.CloseRemovingADS);

        

        

        tradingPanel.tradingStoneDolars.onClick.AddListener(tradingPanel.TradingNumberOne);
        tradingPanel.TradingNumberOneUpdate();

        InvokeRepeating(nameof(AddPointsPerSecond), 0f, 1f);
        
    }

    // =========================================================
    private void AddPointsPerSecond()
    {
        if (stonePerSecond > 0)
        {
            stoneCounter += stonePerSecond;
            clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);
            StonePerClickText.text = addStone.ToString();
        }
    }
     

    // Funkcja kt�ra dodaje nam wykopanego stone i dolarki
    public void AddStone()
    {
        
        // Generujemy Liczb� potrzebn� do wykopania Dolara z kamienia
        float rnd = UnityEngine.Random.Range(1.01f, 10.00f);

        if (rnd > _minDrop && rnd < _maxDrop)
        {
            //Wykop 1 dolarka
            dolarCounter += addDolar;
            clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);
            
        }else if (rnd >= 3.51 && rnd <= 5)
        {
            wegiel += addwegiel;
            zasobyWEQ.UpdateZasoby(wegiel, miedz, zelazo, zloto, diament, emerald);

        }else if (rnd >= 2.31 && rnd <= 3.5)
        {
            miedz += addmiedz;
            zasobyWEQ.UpdateZasoby(wegiel, miedz, zelazo, zloto, diament, emerald);
        }
        else if (rnd >= 1.71 && rnd <=  2.30)
        {
            zelazo += addzelazo;
            zasobyWEQ.UpdateZasoby(wegiel, miedz, zelazo, zloto, diament, emerald);
        }
        else if (rnd >= 1.31 && rnd <= 1.7)
        {
            zloto += addzloto;
            zasobyWEQ.UpdateZasoby(wegiel, miedz, zelazo, zloto, diament, emerald);
        }
        else if (rnd >= 1.07 && rnd <= 1.30)
        {
            diament += adddiament;
            zasobyWEQ.UpdateZasoby(wegiel, miedz, zelazo, zloto, diament, emerald);
        }
        else if (rnd >= 1.02 && rnd <= 1.06 )
        {
            emerald += addemerald;
            zasobyWEQ.UpdateZasoby(wegiel, miedz, zelazo, zloto, diament, emerald);
        }
        else if(rnd >= 2.01 && rnd <= 10)
        {
            stoneCounter += Convert.ToUInt64(addStone);

            if (poziom <= 9)
            {

                if (experience >= 100 * poziom)
                {
                    poziomLVLIMG.SetActive(true);
                    zasobyTlo.SetActive(false);
                    poziomIMG.SetActive(true);
                    clickerUI.stoneCounterText.enabled = true;
                    zasobyButton.enabled = true;
                    clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);
                }
                else
                {
                    float addExperience = UnityEngine.Random.Range(_minExperience, _maxExperience);
                    Debug.Log(addExperience);
                    experience += addExperience * addStone;
                    slider.value += addExperience * addStone / (100 * poziom);

                    if (experience >= 100 * poziom)
                    {
                        poziomLVLIMG.SetActive(true);
                        zasobyTlo.SetActive(false);
                        poziomIMG.SetActive(true);
                        clickerUI.stoneCounterText.enabled = true;
                        zasobyButton.enabled = true;
                        clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);
                    }
                    else
                    {

                    }

                }
                clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);

            }
            else
            {

            }
            

           
        }
        clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);

        Debug.Log($"Tw�j poziom {poziom}, Tw�j exp {experience}");
    }

    // =========================================================

    public void LVLUP()
    {
        if (poziom <= 9)
        {
            poziom += 1;
            experience = 0f;
            slider.value = 0f;
            poziomLVLIMG.SetActive(false);
            clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);

            if (poziom > 9)
            {
                slider.value = 1000;
                clickerUI.UpdateUI(stoneCounter, dolarCounter, poziom);
            }
            else
            {

            }
        }
        else
        {

        }

    }

   

    //Otwieramy oraz zamykamy zasoby u g�ry
    private void OpenZas()
    {
        zasobyTlo.SetActive(true);
        poziomIMG.SetActive(false);                        
        poziomLVLIMG.SetActive(false); ;
        clickerUI.stoneCounterText.enabled = false;
        zasobyButton.enabled = false;
    }
    private void CloseZas()
    {
        zasobyTlo.SetActive(false);
        poziomIMG.SetActive(true);
        if (experience > poziom * 100)
        {
            poziomLVLIMG.SetActive(true);
        }
        else 
        {
            poziomLVLIMG.SetActive(false);
        }
        
        clickerUI.stoneCounterText.enabled = true;
        zasobyButton.enabled = true;
    }


    // =========================================================

    
    
    
  

}
