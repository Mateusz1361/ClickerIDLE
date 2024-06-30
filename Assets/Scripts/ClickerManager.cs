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
    public TextMeshProUGUI StonePerClickText;


    // zmienne ogólne
    public double stoneCounter = 0;
    public double addStone = 1;
    public ulong investment = 0;
    public double stonePerSecond = 0;

    public int dolarCounter = 0;
    int addDolar = 1;

    
    // zmienne od expienia
    public int poziom = 1;
    public double experience = 0f;
    float _minExperience = 30f;
    float _maxExperience = 60f;


    //zmienne do Dropu Dolarow
    float _minDrop = 1.01f;
    float _maxDrop = 1.02f;
    private double _coal;
    private double _copper;
    private double _iron;
    private double _gold;
    private double _diamond;
    private double _emerald;
    //Zasoby
    public double coal { get { return _coal; } set { _coal = value; zasobyWEQ.UpdateCoal(_coal); } } 
    public double copper { get { return _copper; } set { _copper = value; zasobyWEQ.UpdateCopper(_copper); } } 
    public double iron { get { return _iron; } set { _iron = value; zasobyWEQ.UpdateIron(_iron); } } 
    public double gold { get { return _gold; } set { _gold = value; zasobyWEQ.UpdateGold(_gold); } } 
    public double diamond { get { return _diamond; } set { _diamond = value; zasobyWEQ.UpdateDiamond(_diamond); } } 
    public double emerald { get { return _emerald; } set { _emerald = value; zasobyWEQ.UpdateEmerald(_emerald); } } 

    public double addwegiel = 1;
    public double addmiedz = 1;
    public double addzelazo = 1;
    public double addzloto = 1;
    public double adddiament = 1;
    public double addemerald = 1;

    // G³ówna funkcja START
    private void Start()
    {
        upgradesUI.Shoping();
        clickerUI.UpdateStone(stoneCounter);
        clickerUI.UpdateDolars(dolarCounter);
        clickerUI.UpdateLevel(poziom);
        coal = 0;
        copper = 0;
        iron = 0;
        gold = 0;
        diamond = 0;
        emerald = 0;
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

        

        

        InvokeRepeating(nameof(AddPointsPerSecond), 0f, 1f);
        
        
    }
    //dodana juz na pozniej zeby mialo to rece i nogi
    public void ResetAllThingsButKeepInvestors()
    {
        AddingInvestors();
    }
    private void AddingInvestors()
    {
        investment = (ulong)stoneCounter / 1000000;
    }
    // =========================================================
    private void AddPointsPerSecond()
    {
        if (stonePerSecond > 0)
        {
            stoneCounter += stonePerSecond;
            clickerUI.UpdateStone(stoneCounter);
            
        }
    }
     

    // Funkcja która dodaje nam wykopanego stone i dolarki
    public void AddStone()
    {
        
        // Generujemy Liczbê potrzebn¹ do wykopania Dolara z kamienia
        float rnd = UnityEngine.Random.Range(1.01f, 10.00f);

        if (rnd > _minDrop && rnd < _maxDrop)
        {
            //Wykop 1 dolarka
            dolarCounter += addDolar;
            clickerUI.UpdateDolars(dolarCounter);
            
        }else if (rnd >= 3.51 && rnd <= 5)
        {
            coal += addwegiel + 0.4 * investment;
            

        }else if (rnd >= 2.31 && rnd <= 3.5)
        {
            copper += addmiedz+0.4 * investment;
            
        }
        else if (rnd >= 1.71 && rnd <=  2.30)
        {
            iron += addzelazo + 0.4 * investment;
            
        }
        else if (rnd >= 1.31 && rnd <= 1.7)
        {
            gold += addzloto + 0.4 * investment;
            
        }
        else if (rnd >= 1.07 && rnd <= 1.30)
        {
            diamond += adddiament + 0.4 * investment;
            
        }
        else if (rnd >= 1.02 && rnd <= 1.06 )
        {
            emerald += addemerald + 0.4 * investment;
            
        }
        else if(rnd >= 2.01 && rnd <= 10)
        {
            stoneCounter += Convert.ToUInt64(addStone);
            clickerUI.UpdateStone(stoneCounter);
            if (poziom <= 9)
            {

                if (experience >= 100 * poziom)
                {
                    poziomLVLIMG.SetActive(true);
                    zasobyTlo.SetActive(false);
                    poziomIMG.SetActive(true);
                    clickerUI.stoneCounterText.enabled = true;
                    zasobyButton.enabled = true;
                    clickerUI.UpdateLevel(poziom);
                }
                else
                {
                    double addExperience = UnityEngine.Random.Range(_minExperience, _maxExperience);
                    Debug.Log(addExperience);
                    experience += addExperience * addStone;
                    slider.value += (float)(addExperience * addStone / (100 * poziom));

                    if (experience >= 100 * poziom)
                    {
                        poziomLVLIMG.SetActive(true);
                        zasobyTlo.SetActive(false);
                        poziomIMG.SetActive(true);
                        clickerUI.stoneCounterText.enabled = true;
                        zasobyButton.enabled = true;
                        clickerUI.UpdateLevel(poziom);
                    }
                    else
                    {

                    }

                }
                clickerUI.UpdateLevel(poziom);

            }
            else
            {

            }
            

           
        }
        clickerUI.UpdateLevel(poziom);

        Debug.Log($"Twój poziom {poziom}, Twój exp {experience}");
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
            clickerUI.UpdateLevel(poziom);

            if (poziom > 9)
            {
                
                slider.value = 1000;
                clickerUI.UpdateLevel(poziom);
            }
            else
            {

            }
        }
        else
        {

        }
        
        upgradesUI.Shoping();

    }

   

    //Otwieramy oraz zamykamy zasoby u góry
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
