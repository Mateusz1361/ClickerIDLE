using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TradingInstance : MonoBehaviour
{
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private TMP_Text powerText;
    [SerializeField]
    private TMP_Text currencyText;
    [SerializeField]
    private Button buyButton;
    public ClickerManager clickerManager;
    public ClickerUI clickerUI;
    public Image iconOfTrade;
    private int _price;
    private int _power;
    private string _currency;
    private void Start()
    {
        buyButton.onClick.AddListener(Trade);
    }
    public void Trade()
    {
        if(Currency == "Stone")
        {
            if (Price <= (int)clickerManager.stoneCounter)
            {
                clickerManager.stoneCounter -= (ulong)Price;
                clickerUI.UpdateStone(clickerManager.stoneCounter); 
            }
        }else if (Currency == "Coal")
        {
            if (Price <= clickerManager.coal)
            {
                clickerManager.coal -= Price;
            }
        }
        else if (Currency == "Copper")
        {
            if (Price <= clickerManager.copper)
            {
                clickerManager.copper -= Price;
            }
        }
        else if (Currency == "Iron")
        {
            if (Price <= clickerManager.iron)
            {
                clickerManager.iron -= Price;
            }
        }
        else if (Currency == "Gold")
        {
            if (Price <= clickerManager.gold)
            {
                clickerManager.gold -= Price;
            }
        }
        else if (Currency == "Diamond")
        {
            if (Price <= clickerManager.diamond)
            {
                clickerManager.diamond -= Price;
            }
        }
        else if (Currency == "Emerald")
        {
            if (Price <= clickerManager.emerald)
            {
                clickerManager.emerald -= Price;
            }
        }
        clickerManager.dolarCounter += Power;
        clickerUI.UpdateDolars(clickerManager.dolarCounter);


    }
    public int Price
    {
        get
        {
            return _price;
        }
        set
        {
            _price = value;
            priceText.text = _price.ToString();
        }
    }

    public int Power
    {
        get
        {
            return _power;
        }
        set
        {
            _power = value;
            powerText.text = _power.ToString();
        }
    }

    public string Currency
    {
        get
        {
            return _currency;
        }
        set
        {
            _currency = value;
            currencyText.text = _currency.ToString();
        }
    }

}
