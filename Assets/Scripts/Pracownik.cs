using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pracownik : MonoBehaviour
{
    [Header("Podpiecia")]
    [SerializeField] private ClickerManager clickerManager;
    [SerializeField] private ClickerUI clickerUI;
    [SerializeField] private ZasobyWEQ zasobyWEQ;

    [Header("Glowne")]
    [SerializeField] private TextMeshProUGUI addStoneperSecondText;

    public ulong levelOfWorker = 0;
    public ulong powerOfWorker = 0;
    public ulong priceOfWorker = 0;
    public ulong addStonePerSecond;
    public ulong IdentityOfWorker = 0;
    

    void Start()
    {
        
    }

    
   
}
