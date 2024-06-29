using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZasobyWEQ : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wegielText;
    [SerializeField] private TextMeshProUGUI miedzText;
    [SerializeField] private TextMeshProUGUI zelazoText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI diamentText;
    [SerializeField] private TextMeshProUGUI emeraldText;


    public void UpdateZasoby(int wegiel, int miedz, int zelazo, int gold, int diament, int emerald)
    {
        wegielText.text = $"{wegiel}";
        miedzText.text = $"{miedz}";
        zelazoText.text = $"{zelazo}";
        goldText.text = $"{gold}";
        diamentText.text = $"{diament}";
        emeraldText.text = $"{emerald}";
    }
}
