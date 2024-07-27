using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestorMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private TMP_Text investorsText;

    private EquipmentMenu data;
    
    private ulong investors = 0;
    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        
    }
    public void OnEnable() {
        CountingInvestors();
    }
    public void CountingInvestors()
    {
        if (data.Stone.Count >= 10)
        {
            Rational rational = data.Stone.Count - 10*investors;
            investors = (ulong)rational; 
            investorsText.text = investors.ToString();
        }
    }
}
