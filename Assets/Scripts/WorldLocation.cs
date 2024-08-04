using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
using System.Collections.Generic;

public class WorldLocation : MonoBehaviour {
    [SerializeField]
    private Image mainResourceIcon;
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private GameObject lockedMarker;
    private WorldMenu worldMenu;
    private InventoryMenu inventoryMenu;
    private InvestorMenu investorMenu;

    public Rational mainResourceClickIncrement;
    public float mainResourceAutoIncrementTimer;

    public event Action<Rational> OnMainResourceAutoIncrementChange;
    private Rational _mainResourceAutoIncrement;
    public Rational MainResourceAutoIncrement {
        get {
            return _mainResourceAutoIncrement;
        }
        set {
            _mainResourceAutoIncrement = value;
            OnMainResourceAutoIncrementChange?.Invoke(_mainResourceAutoIncrement);
        }
    }

    public event Action<ulong> OnLevelChange;
    private ulong _level;
    public ulong Level {
        get {
            return _level;
        }
        set {
            _level = value;
            OnLevelChange?.Invoke(_level);
        }
    }

    public double maxExperience;

    public event Action<double,double> OnExperienceChange;
    private double _experience;
    public double Experience {
        get {
            return _experience;
        }
        set {
            _experience = Math.Clamp(value,0.0,maxExperience);
            OnExperienceChange?.Invoke(_experience,maxExperience);
        }
    }

    private BigInteger _investorsYouHave;
    public BigInteger InvestorsYouHave { 
        get { 
            return _investorsYouHave; 
        } 
        set 
        {
            _investorsYouHave = value;
            investorMenu.investorsYouHaveText.text = _investorsYouHave.ToString();
        } 
    }
    private BigInteger _investorsToClaim;
    public BigInteger InvestorsToClaim
    {
        get
        {
            return _investorsToClaim;
        }
        set
        {
            _investorsToClaim = value;
            investorMenu.investorsToClaimText.text = _investorsToClaim.ToString();
        }
    }
    public Rational differenceOfMaterial = 0;
    public Rational quantityToAddInvestor = 10;
    

    private List<ShopItem> _shopItems;
    public List<ShopItem> ShopItems {
        get {
            _shopItems ??= new();
            return _shopItems;
        }
    }
   

    private bool _purchased;
    public bool Purchased {
        get {
            return _purchased;
        }
        set {
            _purchased = value;
            lockedMarker.SetActive(!_purchased);
            priceText.gameObject.SetActive(lockedMarker.activeSelf);
        }
    }

    private void Awake() {
        acceptButton.onClick.AddListener(() => {
            if(!Purchased && inventoryMenu.Money.Count >= Price) {
                inventoryMenu.Money.Count -= Price;
                Purchased = true;
            }
            else if(Purchased) {
                worldMenu.CurrentWorldLocation = this;
            }
        });
    }

    public void InitLocation(WorldMenu _worldMenu,InventoryMenu _inventoryMenu,InvestorMenu _investorMenu,WorldLocationData location) {
        worldMenu = _worldMenu;
        inventoryMenu = _inventoryMenu;
        investorMenu = _investorMenu;
        Name = location.name;
        Price = location.price;
        MainResourceName = location.mainResource;
        mainResourceIcon.sprite = inventoryMenu.ResourceInstances[MainResourceName].Icon;
        Purchased = (Price == 0);
        mainResourceClickIncrement = 1;
        mainResourceAutoIncrementTimer = 0.0f;
        MainResourceAutoIncrement = 0;
        Level = 0;
        maxExperience = 40.0;
        Experience = 0.0;
    }

    private string _name;
    public string Name {
        get {
            return _name;
        }
        private set {
            _name = value;
            nameText.text = _name;
        }
    }

    private string _mainResourceName;
    public string MainResourceName {
        get {
            return _mainResourceName;
        }
        private set {
            _mainResourceName = value;
        }
    }

    private BigInteger _price;
    public BigInteger Price {
        get {
            return _price;
        }
        private set {
            _price = value;
            priceText.text = $"Unlock for ${NumberFormat.ShortForm(_price)}";
        }
    }
}
