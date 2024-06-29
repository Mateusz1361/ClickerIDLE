using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickerUI : MonoBehaviour
{
    [SerializeField] public ClickerManager clickerManager;

    [SerializeField] public TextMeshProUGUI stoneCounterText;
    [SerializeField] private TextMeshProUGUI stoneCounterText2;
    [SerializeField] private TextMeshProUGUI dolarsamouttext;
    [SerializeField] private TextMeshProUGUI poziomtext;

    [Header("Basic Info")]
    [SerializeField] private TextMeshProUGUI iloscStonaNaClick;
    [SerializeField] private TextMeshProUGUI iloscStonaNaSec;
    

    [Header("Kielnia")]
    [SerializeField] public TextMeshProUGUI cenaKielniaText;
    [SerializeField] public TextMeshProUGUI mocKielniText;
    [SerializeField] public TextMeshProUGUI iloscKielniText;


    [Header("Dluto")]
    [SerializeField] public TextMeshProUGUI cenaDlutaText;
    [SerializeField] public TextMeshProUGUI mocDlutaText;
    [SerializeField] public TextMeshProUGUI iloscDlutText;
    public void UpdateUI(ulong amount, int dolarsamount, int poziom)
    {
        dolarsamouttext.text = $"{dolarsamount}";
        poziomtext.text = $"{poziom}";
        iloscStonaNaSec.text = $" {clickerManager.stonePerSecond}";
        
        if (amount < 1000000)
        {
            stoneCounterText.text = $"{amount}";
            stoneCounterText2.text = $"{amount}";
        }
        else if  (amount >= 1000000 && amount < 1000000000)
        {
            stoneCounterText.text = $"{amount / 1000}K";
            stoneCounterText2.text = $"{amount / 1000}K";
        }
        else if (amount >= 1000000000 && amount < 1000000000000)
        {
            stoneCounterText.text = $"{amount / 1000000}M";
            stoneCounterText2.text = $"{amount / 1000000}M";
        }
        else if (amount >= 1000000000000 && amount < 1000000000000000)
        {
            stoneCounterText.text = $"{amount / 1000000000}B";
            stoneCounterText2.text = $"{amount / 1000000000}B";
        }
        else if (amount >= 1000000000000000 )
        {
            stoneCounterText.text = $"{amount / 1000000000000}T";
            stoneCounterText2.text = $"{amount / 1000000000000}T";
        }
    }

    public void KielniaUI(int cenaPrzedmiotu,int mocPrzedmiotu,int iloscPrzedmiotu) 
    {
        cenaKielniaText.text = $"{cenaPrzedmiotu}";
        mocKielniText.text = $"{mocPrzedmiotu}";
        iloscKielniText.text = $"{iloscPrzedmiotu}";
        iloscStonaNaClick.text = $"{clickerManager.addStone}";

    }

    public void DlutoUI(int cenaPrzedmiotu, int mocPrzedmiotu, int iloscPrzedmiotu)
    {
        cenaDlutaText.text = $"{cenaPrzedmiotu}";
        mocDlutaText.text = $"{mocPrzedmiotu}";
        iloscDlutText.text = $"{iloscPrzedmiotu}";
        iloscStonaNaClick.text = $"{clickerManager.addStone}";
    }
    


}
