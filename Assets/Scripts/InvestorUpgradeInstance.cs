using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

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
    public Rational multiplier;
    private WorldMenu worldMenu;
    private InvestorMenu investorMenu;

    private Rational _price;
    public Rational Price {
        get {
            return _price;
        }
        set {
            _price = value;
            buyUpgradeButtonText.text = $"Buy for\n{NumberFormat.ShortForm(_price)}";
        }
    }

    public string WhatYouGetText {
        get {
            return whatYouGetText.text;
        }
    }

    public void Init(InvestorUpgradeData investorUpgradeData,WorldLocation _worldLocation,WorldMenu _worldMenu,InvestorMenu _investorMenu) {
        worldMenu = _worldMenu;
        investorMenu = _investorMenu;
        worldLocation = _worldLocation;
        whatYouMultiply = investorUpgradeData.whatYouMultiply;
        multiplier = Rational.Parse(investorUpgradeData.multiplier);
        whatYouGetText.text = investorUpgradeData.whatYouGetText;
        Price = investorUpgradeData.price;
        iconOfInvestorsUpgrade.sprite = Resources.Load<Sprite>("Images/WorkersButton");
        buyUpgradeButton.onClick.AddListener(BuyInvestorUpgrade);
        Purchased = false;
    }

    public void MakeVisible() {
        gameObject.SetActive(!Purchased && worldLocation == worldMenu.CurrentWorldLocation);
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
                    worldLocation.InvestorsYouHave -= (BigInteger)Price;
                    investorMenu.UpdateInvestors();
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
