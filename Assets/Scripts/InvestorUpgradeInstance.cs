using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;


public class InvestorUpgradeInstance : MonoBehaviour
{
    public WorldLocation worldLocation;
    string whatYouMultiply;
    Rational multiplier;
    Rational price;

    [SerializeField]
    public TMP_Text whatYouGetText;
    [SerializeField]
    public Button buyUpgradeButton;
    [SerializeField]
    public Image iconOfInvestorsUpgrade;
    

    
       
    public void Init(InvestorUpgradeData investorUpgradeData, WorldLocation _worldLocation)
    {
       
        worldLocation = _worldLocation;
        whatYouMultiply = investorUpgradeData.whatYouMultiply;
        multiplier = Rational.Parse(investorUpgradeData.multiplier);
        whatYouGetText.text = investorUpgradeData.whatYouGetText;
        price = investorUpgradeData.price;
        iconOfInvestorsUpgrade.sprite = Resources.Load<Sprite>("Images/WorkersButton");
        buyUpgradeButton.onClick.AddListener(BuyInvestorUpgrade);
    }
    public void BuyInvestorUpgrade() {
        Debug.Log("dupa1");
        Debug.Log(price);
        Debug.Log(worldLocation.InvestorsYouHave);
        if (worldLocation.InvestorsYouHave >= price)
        {
            Debug.Log("dupa2");
            foreach (var shopitem in worldLocation.ShopItems)
            {
                Debug.Log("dupa3");
                if (whatYouMultiply == shopitem.name)
                {
                    Debug.Log("dupa4");
                    worldLocation.InvestorsYouHave -= (BigInteger)price;
                    shopitem.multiplier *= multiplier;
                    gameObject.SetActive(false);
                }
            }
        }
        
        
    }



}


