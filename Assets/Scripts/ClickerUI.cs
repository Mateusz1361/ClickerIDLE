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
    
    public void UpdateStone(double amount)
    {
        
        iloscStonaNaSec.text = $" {clickerManager.stonePerSecond}";
        if(amount < 1000)
        {
            stoneCounterText.text = $"{amount}";
            stoneCounterText2.text = $"{amount}";
        }
        
        else if (amount < 1000000)
        {
            stoneCounterText.text = $"{amount/1000:F3}K";
            stoneCounterText2.text = $"{amount/1000:F3}K";
        }
        else if  (amount >= 1000000 && amount < 1000000000)
        {
            stoneCounterText.text = $"{amount / 1000000:F3}M";
            stoneCounterText2.text = $"{amount / 1000000:F3}M";
        }
        else if (amount >= 1000000000 && amount < 1000000000000)
        {
            stoneCounterText.text = $"{amount / 1000000000:F3}B";
            stoneCounterText2.text = $"{amount / 1000000000:F3}B";
        }
        else if (amount >= 1000000000000 && amount < 1000000000000000)
        {
            stoneCounterText.text = $"{amount / 1000000000000:F3}T";
            stoneCounterText2.text = $"{amount / 1000000000000:F3}T";
        }
        else if (amount >= 1000000000000000 )
        {
            stoneCounterText.text = $"{amount / 1000000000000000:F3}Q";
            stoneCounterText2.text = $"{amount / 1000000000000000:F3}Q";
        }
    }
    public void UpdateLevel(int level)
    {
        poziomtext.text = $"{level}";
    }
    public void UpdateDolars(int dolars)
    {
        dolarsamouttext.text = $"{dolars}";
    }





}
