using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestorMenu : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    public TMP_Text investorsYouHaveText;
    [SerializeField]
    public TMP_Text investorsToClaimText;
    [SerializeField]
    private Button claimInvestors;
    [SerializeField]
    private TextAsset jsonInvestors;
    [SerializeField]
    private GameObject parentForInvestorsUpgrades;
    [SerializeField]
    private GameObject prefabOfInvestorsUpgrades;

    public void Init() {
        claimInvestors.onClick.AddListener(() => ClaimAllInvestors());
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        referenceHub.worldMenu.OnWorldLocationLeft += (WorldLocation location) => {
            if(location != null) {
                referenceHub.inventoryMenu.OreItemsSlots[location.MainResourceName].OnCountIncremented -= CountingInvestors;
                foreach(var upgrade in location.InvestorUpgrades) {
                    upgrade.gameObject.SetActive(false);
                }
            }
        };
        referenceHub.worldMenu.OnWorldLocationEntered += (WorldLocation location) => {
            referenceHub.inventoryMenu.OreItemsSlots[location.MainResourceName].OnCountIncremented += CountingInvestors;
            foreach(var upgrade in location.InvestorUpgrades) {
                if(!upgrade.Purchased) {
                    upgrade.MakeVisible();
                }
            }
            CountingInvestors(0);
        };
        var investorUpgradeInstancesDatas = JsonUtility.FromJson<InstanceWrapperDataJson<InvestorUpgradeDataJson>>(jsonInvestors.text);
        foreach(var location in referenceHub.worldMenu.WorldLocations) {
            foreach(var investorUpgradeInstancesData in investorUpgradeInstancesDatas.data) {
                var prefab = Instantiate(prefabOfInvestorsUpgrades,parentForInvestorsUpgrades.transform);
                var investorUpgradeInstance = prefab.GetComponent<InvestorUpgradeInstance>();
                investorUpgradeInstance.Init(investorUpgradeInstancesData,location,referenceHub);
                location.InvestorUpgrades.Add(investorUpgradeInstance);
            }
        }
    }
    
    public void CountingInvestors(SafeUDecimal resourceToInvestors) {
        var cwl = referenceHub.worldMenu.CurrentWorldLocation;
        cwl.differenceOfMaterial += resourceToInvestors;
        if(cwl.differenceOfMaterial >= cwl.quantityToAddInvestor) {
            var value = cwl.differenceOfMaterial / cwl.quantityToAddInvestor;
            cwl.InvestorsToClaim += value;
            cwl.differenceOfMaterial -= cwl.quantityToAddInvestor * value;
        }
        UpdateInvestors();
    }

    public void UpdateInvestors() {
        var cwl = referenceHub.worldMenu.CurrentWorldLocation;
        investorsYouHaveText.text = cwl.InvestorsYouHave.ToString();
        investorsToClaimText.text = cwl.InvestorsToClaim.ToString();
    }

    public void ClaimAllInvestors() {
        var cwl = referenceHub.worldMenu.CurrentWorldLocation;
        if(cwl.InvestorsToClaim > 0) {
            cwl.mainResourceAutoIncrementTimer = 0;
            foreach(var shopitem in cwl.ShopItems) {
                shopitem.ResetItem();
            }
            foreach(var upgrade in cwl.InvestorUpgrades) {
                upgrade.ResetUpgrade();
            }
            cwl.InvestorsYouHave += cwl.InvestorsToClaim;
            cwl.InvestorsToClaim = 0;
            cwl.Level = 0;
            cwl.Experience = 0;
            cwl.maxExperience = 40;
            cwl.differenceOfMaterial = 0;
            referenceHub.inventoryMenu.OreItemsSlots[cwl.MainResourceName].Count = 0;
            cwl.RecalculateMainResourceClickIncrement();
            cwl.RecalculateMainResourceAutoIncrement();
        }
    }
}
