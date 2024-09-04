using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestorMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    public TMP_Text investorsYouHaveText;
    [SerializeField]
    public TMP_Text investorsToClaimText;
    [SerializeField]
    private InventoryMenu data;
    [SerializeField]
    private WorldMenu worldMenu;
    [SerializeField]
    private Button claimInvestors;
    [SerializeField]
    private SaveSystem saveSystem;
    [SerializeField]
    private TextAsset jsonInvestors;
    [SerializeField]
    private GameObject parentForInvestorsUpgrades;
    [SerializeField]
    private GameObject prefabOfInvestorsUpgrades;
    [SerializeField]
    private InventoryMenu inventoryMenu;

    public void Init() {
        claimInvestors.onClick.AddListener(() => ClaimAllInvestors());
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        worldMenu.OnWorldLocationLeft += (WorldLocation location) => {
            if(location != null) {
                data.ResourceInstances[location.MainResourceName].OnCountIncrement -= CountingInvestors;
                foreach(var upgrade in location.InvestorUpgrades) {
                    upgrade.gameObject.SetActive(false);
                }
            }
        };
        worldMenu.OnWorldLocationEntered += (WorldLocation location) => {
            data.ResourceInstances[location.MainResourceName].OnCountIncrement += CountingInvestors;
            foreach(var upgrade in location.InvestorUpgrades) {
                if(!upgrade.Purchased) {
                    upgrade.MakeVisible();
                }
            }
            CountingInvestors(0);
        };
        var investorUpgradeInstancesDatas = JsonUtility.FromJson<InstanceWrapper<InvestorUpgradeData>>(jsonInvestors.text);
        foreach(var location in worldMenu.WorldLocations) {
            foreach(var investorUpgradeInstancesData in investorUpgradeInstancesDatas.data) {
                var prefab = Instantiate(prefabOfInvestorsUpgrades,parentForInvestorsUpgrades.transform);
                var investorUpgradeInstance = prefab.GetComponent<InvestorUpgradeInstance>();
                investorUpgradeInstance.Init(investorUpgradeInstancesData,location,worldMenu,this);
                location.InvestorUpgrades.Add(investorUpgradeInstance);
            }
        }
    }
    
    public void CountingInvestors(Rational resourceToInvestors) {
        var cwl = worldMenu.CurrentWorldLocation;
        cwl.differenceOfMaterial += resourceToInvestors;
        while(cwl.differenceOfMaterial >= cwl.quantityToAddInvestor) {
            cwl.differenceOfMaterial -= cwl.quantityToAddInvestor;
            cwl.InvestorsToClaim += 1;
        }
        UpdateInvestors();
    }

    public void UpdateInvestors() {
        var cwl = worldMenu.CurrentWorldLocation;
        investorsYouHaveText.text = cwl.InvestorsYouHave.ToString();
        investorsToClaimText.text = cwl.InvestorsToClaim.ToString();
    }

    public void ClaimAllInvestors() {
        var cwl = worldMenu.CurrentWorldLocation;
        if(cwl.InvestorsToClaim > 0) {
            //cwl.MainResourceAutoIncrement = 0;
            cwl.mainResourceAutoIncrementTimer = 0;
            foreach(var shopitem in cwl.ShopItems) {
                shopitem.ResetItem();
            }
            cwl.InvestorsYouHave += cwl.InvestorsToClaim;
            cwl.InvestorsToClaim = 0;
            cwl.Level = 0;
            cwl.Experience = 0;
            cwl.maxExperience = 40;
            cwl.differenceOfMaterial = 0;
            inventoryMenu.ResourceInstances[cwl.MainResourceName].Count = 0;
            cwl.RecalculateMainResourceClickIncrement();
            cwl.RecalculateMainResourceAutoIncrement();
        }
    }
}
