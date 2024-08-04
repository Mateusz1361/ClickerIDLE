using System.Collections.Generic;
using System.Numerics;
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
    public List<InvestorUpgradeInstance> _investorUpgradeInstances;

    public void Init() {
        claimInvestors.onClick.AddListener(() => ClaimAllInvestors());
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        worldMenu.OnWorldLocationLeft += (WorldLocation location) => {
            if (location != null) {
                data.ResourceInstances[location.MainResourceName].OnCountIncrement -= CountingInvestors;
                foreach (var investorsUpgrades in _investorUpgradeInstances)
                {
                    investorsUpgrades.gameObject.SetActive(false);
                }
            }
        };
        worldMenu.OnWorldLocationEntered += (WorldLocation location) =>
        {
            data.ResourceInstances[location.MainResourceName].OnCountIncrement += CountingInvestors;
            CountingInvestors(0);
            foreach (var investorsUpgrades in _investorUpgradeInstances)
            {
                investorsUpgrades.gameObject.SetActive(investorsUpgrades.worldLocation == location);
            }
        };
        _investorUpgradeInstances = new();
        var investorUpgradeInstancesDatas = JsonUtility.FromJson<InstanceWrapper<InvestorUpgradeData>>(jsonInvestors.text);
        
        for (int i = 0;i < worldMenu.WorldLocations.Count; i++)
        {
            var location = worldMenu.WorldLocations[i];
            foreach (var investorUpgradeInstancesData in investorUpgradeInstancesDatas.data)
            {
                var prefab = Instantiate(prefabOfInvestorsUpgrades, parentForInvestorsUpgrades.transform);
                prefab.SetActive(location == worldMenu.CurrentWorldLocation);
                var investorUpgradeInstance = prefab.GetComponent<InvestorUpgradeInstance>();
                investorUpgradeInstance.Init(investorUpgradeInstancesData,location);
                _investorUpgradeInstances.Add(investorUpgradeInstance);
            }
        }
    }
    
    public void CountingInvestors(Rational resourceToInvestors)
    {
        var cWL = worldMenu.CurrentWorldLocation;
        cWL.differenceOfMaterial += resourceToInvestors;
        while (cWL.differenceOfMaterial >= cWL.quantityToAddInvestor)
        {
            cWL.differenceOfMaterial -= cWL.quantityToAddInvestor;
            cWL.InvestorsToClaim += 1;
            
        }
        UpdateInvestors();
    }
    public void UpdateInvestors()
    {
        var cWL = worldMenu.CurrentWorldLocation;
        investorsYouHaveText.text = cWL.InvestorsYouHave.ToString();
        investorsToClaimText.text = cWL.InvestorsToClaim.ToString();
    }
    public void ClaimAllInvestors()
    {
        worldMenu.CurrentWorldLocation.MainResourceAutoIncrement = 0;
        worldMenu.CurrentWorldLocation.mainResourceAutoIncrementTimer = 0;
        foreach (var shopitem in worldMenu.CurrentWorldLocation.ShopItems) { 
            shopitem.ResetItem();
        }
        worldMenu.CurrentWorldLocation.InvestorsYouHave += worldMenu.CurrentWorldLocation.InvestorsToClaim;
        worldMenu.CurrentWorldLocation.InvestorsToClaim = 0;
        worldMenu.CurrentWorldLocation.Level = 0;
        worldMenu.CurrentWorldLocation.Experience = 0;
        worldMenu.CurrentWorldLocation.maxExperience = 40;
        worldMenu.CurrentWorldLocation.differenceOfMaterial = 0;
        inventoryMenu.ResourceInstances[worldMenu.CurrentWorldLocation.MainResourceName].Count = 0;
        saveSystem.SaveGame();
    }
    
}
