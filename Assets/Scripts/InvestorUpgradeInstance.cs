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
    private Image iconOfInvestorsUpgrade;
    [HideInInspector]
    public WorldLocation worldLocation;
    [HideInInspector]
    public string whatYouMultiply;
    public Rational multiplier;
    public Rational price;
    private WorldMenu worldMenu;
    private InvestorMenu investorMenu;

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
        price = investorUpgradeData.price;
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
        if(worldLocation.InvestorsYouHave >= price) {
            foreach(var shopitem in worldLocation.ShopItems) {
                if(whatYouMultiply == shopitem.name) {
                    worldLocation.InvestorsYouHave -= (BigInteger)price;
                    investorMenu.UpdateInvestors();
                    shopitem.multiplier *= multiplier;
                    Purchased = true;
                    break;
                }
            }
        }
    }
}
