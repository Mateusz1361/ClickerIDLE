using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvestorUpgradeInstance : MonoBehaviour {
    [SerializeField]
    private TMP_Text whatYouGetText;
    [SerializeField]
    private Button buyUpgradeButton;
    [SerializeField]
    private TMP_Text buyUpgradeButtonText;
    [SerializeField]
    private Image iconOfInvestorsUpgrade;
    [HideInInspector]
    public WorldLocation worldLocation;
    [HideInInspector]
    public string whatYouMultiply;
    public SafeUDecimal multiplier;
    private ReferenceHub referenceHub;

    private SafeUInteger _price = 0;
    public SafeUInteger Price {
        get {
            return _price;
        }
        set {
            _price = value;
            buyUpgradeButtonText.text = $"Buy for\n{_price}";
        }
    }

    public string WhatYouGetText {
        get {
            return whatYouGetText.text;
        }
    }

    public void Init(InvestorUpgradeData investorUpgradeData,WorldLocation _worldLocation,ReferenceHub _referenceHub) {
        referenceHub = _referenceHub;
        worldLocation = _worldLocation;
        whatYouMultiply = investorUpgradeData.whatYouMultiply;
        multiplier = SafeUDecimal.Parse(investorUpgradeData.multiplier);
        whatYouGetText.text = investorUpgradeData.whatYouGetText;
        Price = investorUpgradeData.price;
        iconOfInvestorsUpgrade.sprite = Resources.Load<Sprite>("Images/WorkersButton");
        buyUpgradeButton.onClick.AddListener(BuyInvestorUpgrade);
        Purchased = false;
    }

    public void ResetUpgrade() {
        Purchased = false;
    }

    public void MakeVisible() {
        gameObject.SetActive(!Purchased && worldLocation == referenceHub.worldMenu.CurrentWorldLocation);
    }

    private bool _purchased;
    public bool Purchased {
        get {
            return _purchased;
        }
        set {
            _purchased = value;
            MakeVisible();
        }
    }

    public void BuyInvestorUpgrade() {
        if(worldLocation.InvestorsYouHave >= Price) {
            foreach(var shopitem in worldLocation.ShopItems) {
                if(whatYouMultiply == shopitem.name) {
                    worldLocation.InvestorsYouHave -= Price;
                    referenceHub.investorMenu.UpdateInvestors();
                    shopitem.multiplier *= multiplier;
                    Purchased = true;
                    shopitem.RecalculateMainResourceClickIncrement();
                    shopitem.RecalculateMainResourceAutoIncrement();
                    break;
                }
            }
            worldLocation.RecalculateMainResourceClickIncrement();
            worldLocation.RecalculateMainResourceAutoIncrement();
        }
    }
}
