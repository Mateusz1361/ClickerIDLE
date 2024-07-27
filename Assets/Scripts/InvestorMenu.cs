using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestorMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private TMP_Text investorsText;
    [SerializeField]
    private InventoryMenu data;
    [SerializeField]
    private WorldMenu worldMenu;
    
    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        worldMenu.OnWorldLocationLeft += (WorldLocation location) => {
            if (location != null) {
                data.ResourceInstances[location.MainResourceName].OnCountIncreament -= CountingInvestors;
            }
        };
        worldMenu.OnWorldLocationEntered += (WorldLocation location) =>
        {
            data.ResourceInstances[location.MainResourceName].OnCountIncreament += CountingInvestors;
            CountingInvestors(0);
        };
    }
    
    public void CountingInvestors(Rational resourceToInvestors)
    {
        var cWL = worldMenu.CurrentWorldLocation;
        cWL.differenceOfStone += resourceToInvestors;
        while (cWL.differenceOfStone >= cWL.quantityToAddInvestor)
        {
            cWL.differenceOfStone -= cWL.quantityToAddInvestor;
            cWL.investors += 1;
            
        }
        investorsText.text = cWL.investors.ToString();
    }
    
}
