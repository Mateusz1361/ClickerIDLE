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
    
    public void UpdateCoal(int coal)
    {
        coalText.text = $"{coal}";
    }
    public void UpdateCopper(int copper)
    {
        copperText.text = $"{copper}";
    }
    public void UpdateIron(int Iron)
    {
        ironText.text = $"{Iron}";
    }
    public void UpdateGold(int Gold)
    {
        goldText.text = $"{Gold}";
    }
    public void UpdateDiamond(int Diamond)
    {
        diamondText.text = $"{Diamond}";
    }
    public void UpdateEmerald(int emerald)
    {
        emeraldText.text = $"{emerald}";
    }

}
