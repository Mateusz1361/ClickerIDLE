using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestorMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private TMP_Text investorsYouHaveText;
    [SerializeField]
    private TMP_Text investorsToClaimText;
    [SerializeField]
    private InventoryMenu data;
    [SerializeField]
    private WorldMenu worldMenu;
    [SerializeField]
    private Button claimInvestors;
    [SerializeField]
    private SaveSystem saveSystem;
    
    public void Init() {
        claimInvestors.onClick.AddListener(() => saveSystem.SaveGame());
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        worldMenu.OnWorldLocationLeft += (WorldLocation location) => {
            if (location != null) {
                data.ResourceInstances[location.MainResourceName].OnCountIncrement -= CountingInvestors;
            }
        };
        worldMenu.OnWorldLocationEntered += (WorldLocation location) =>
        {
            data.ResourceInstances[location.MainResourceName].OnCountIncrement += CountingInvestors;
            CountingInvestors(0);
        };

    }
    
    public void CountingInvestors(Rational resourceToInvestors)
    {
        var cWL = worldMenu.CurrentWorldLocation;
        cWL.differenceOfMaterial += resourceToInvestors;
        while (cWL.differenceOfMaterial >= cWL.quantityToAddInvestor)
        {
            cWL.differenceOfMaterial -= cWL.quantityToAddInvestor;
            cWL.investorsToClaim += 1;
            
        }
        investorsYouHaveText.text = cWL.investorsYouHave.ToString();
        investorsToClaimText.text = cWL.investorsToClaim.ToString();
    }
    
}
