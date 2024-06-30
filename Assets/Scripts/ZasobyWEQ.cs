using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZasobyWEQ : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coalText;
    [SerializeField] private TextMeshProUGUI copperText;
    [SerializeField] private TextMeshProUGUI ironText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI diamondText;
    [SerializeField] private TextMeshProUGUI emeraldText;
    
    public void UpdateCoal(double coal)
    {
        coalText.text = $"{coal}";
    }
    public void UpdateCopper(double copper)
    {
        copperText.text = $"{copper}";
    }
    public void UpdateIron(double Iron)
    {
        ironText.text = $"{Iron}";
    }
    public void UpdateGold(double Gold)
    {
        goldText.text = $"{Gold}";
    }
    public void UpdateDiamond(double Diamond)
    {
        diamondText.text = $"{Diamond}";
    }
    public void UpdateEmerald(double emerald)
    {
        emeraldText.text = $"{emerald}";
    }

}
