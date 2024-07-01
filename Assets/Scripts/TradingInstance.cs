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
    private TMP_Text currencyThatGetsBackText;
    [SerializeField]
    private Button buyButton;
    public ClickerManager clickerManager;
    public ClickerUI clickerUI;
    public Image iconOfTrade;
    private int _price;
    private int _power;
    private string _currency;
    private string _currencyThatGetsBack;
    private void Start()
    {
        buyButton.onClick.AddListener(Trade);
    }
    public void Trade()
    {
        
        if(Currency == "Stone")
        {
            if (Price <= clickerManager.stoneCounter)
            {
                clickerManager.stoneCounter -= Price;
                clickerUI.UpdateStone(clickerManager.stoneCounter);
                Decider(CurrencyThatGetsBack, Power);
            }
        }else if (Currency == "Coal")
        {
            if (Price <= clickerManager.coal)
            {
                clickerManager.coal -= Price;
                Decider(CurrencyThatGetsBack, Power);
            }
        }
        else if (Currency == "Copper")
        {
            if (Price <= clickerManager.copper)
            {
                clickerManager.copper -= Price;
                Decider(CurrencyThatGetsBack, Power);
            }
        }
        else if (Currency == "Iron")
        {
            if (Price <= clickerManager.iron)
            {
                clickerManager.iron -= Price;
                Decider(CurrencyThatGetsBack, Power);
            }
        }
        else if (Currency == "Gold")
        {
            if (Price <= clickerManager.gold)
            {
                clickerManager.gold -= Price;
                Decider(CurrencyThatGetsBack, Power);
            }
        }
        else if (Currency == "Diamond")
        {
            if (Price <= clickerManager.diamond)
            {
                clickerManager.diamond -= Price;
                Decider(CurrencyThatGetsBack, Power);
            }
        }
        else if (Currency == "Emerald")
        {
            if (Price <= clickerManager.emerald)
            {
                clickerManager.emerald -= Price;
                Decider(CurrencyThatGetsBack, Power);
            }
        }
        
        
        
    }
    public void Decider(string something,int poweer)
    {
        
        switch (something)
        {
            case "Emerald":
                clickerManager.emerald+=poweer;
                break;
            case "Diamond":
                clickerManager.diamond += poweer;
                break;
            case "Gold":
                clickerManager.gold += poweer;
                break;
            case "Iron":
                clickerManager.iron += poweer;
                break;
            case "Copper":
                clickerManager.copper += poweer;
                break;
            case "Coal":
                clickerManager.coal += poweer;
                break;
            case "Dolar":
                clickerManager.dolarCounter += poweer;
                clickerUI.UpdateDolars(clickerManager.dolarCounter);
                break;

        }
        
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
    public string CurrencyThatGetsBack
    {
        get
        {
            return _currencyThatGetsBack;
        }
        set
        {
            _currencyThatGetsBack = value;
            currencyThatGetsBackText.text = _currencyThatGetsBack.ToString();
        }
    }
    

}
