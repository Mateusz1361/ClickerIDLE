using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class ShopInstance : MonoBehaviour
{
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private TMP_Text powerText;
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Button buyButton;
    public ClickerManager clickerManager;
    public Image iconOfShop;
    private int _price;
    private int _power;
    private int _quantity;
    private void Start()
    {
        buyButton.onClick.AddListener(BuyInShop);
    }
    public void BuyInShop()
    {
        if (Price <= (int)clickerManager.stoneCounter && Quantity < 10)
        {
            clickerManager.stoneCounter -= (ulong)Price;
            clickerManager.addStone += Power;
            Quantity += 1;
            Price *= 1;
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

    public int Quantity
    {
        get
        {
            return _quantity;
        }
        set
        {
            _quantity = value;
            quantityText.text = _quantity.ToString();
        }
    }
}