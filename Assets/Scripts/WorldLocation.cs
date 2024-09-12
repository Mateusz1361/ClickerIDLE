using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
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
    private ReferenceHub referenceHub;

    private SafeUDecimal cacheMainResourceClickIncrement = 1;
    private SafeUDecimal cacheMainResourceAutoIncrement = 0;

    public SafeUDecimal MainResourceClickIncrement()  {
        return cacheMainResourceClickIncrement;
    }

    public SafeUDecimal MainResourceAutoIncrement() {
        return cacheMainResourceAutoIncrement;
    }

    public float mainResourceAutoIncrementTimer = 0.0f;

    public event Action<SafeUDecimal> OnMainResourceAutoIncrementChange;
    public void RecalculateMainResourceClickIncrement()  {
        cacheMainResourceClickIncrement = 1;
        foreach(var item in ShopItems) {
            cacheMainResourceClickIncrement += item.MainResourceClickIncrement();
        }
        cacheMainResourceClickIncrement *= (1 + SafeUDecimal.CentiUnits(2) * InvestorsYouHave);
    }

    public void RecalculateMainResourceAutoIncrement() {
        cacheMainResourceAutoIncrement = 0;
        foreach(var item in ShopItems) {
            cacheMainResourceAutoIncrement += item.MainResourceAutoIncrement();
        }
        cacheMainResourceAutoIncrement *= (1 + SafeUDecimal.CentiUnits(2) * InvestorsYouHave);
        OnMainResourceAutoIncrementChange?.Invoke(cacheMainResourceAutoIncrement);
    }

    public event Action<ulong> OnLevelChange;
    private ulong _level = 0;
    public ulong Level {
        get {
            return _level;
        }
        set {
            _level = value;
            OnLevelChange?.Invoke(_level);
        }
    }

    public double maxExperience = 0.0;

    public event Action<double,double> OnExperienceChange;
    private double _experience = 0.0;
    public double Experience {
        get {
            return _experience;
        }
        set {
            _experience = Math.Clamp(value,0.0,maxExperience);
            OnExperienceChange?.Invoke(_experience,maxExperience);
        }
    }

    private SafeUDecimal _investorsYouHave = 0;
    public SafeUDecimal InvestorsYouHave { 
        get { 
            return _investorsYouHave; 
        } 
        set  {
            _investorsYouHave = value;
            referenceHub.investorMenu.investorsYouHaveText.text = _investorsYouHave.ToString();
            RecalculateMainResourceClickIncrement();
            RecalculateMainResourceAutoIncrement();
        } 
    }

    private SafeUDecimal _investorsToClaim = 0;
    public SafeUDecimal InvestorsToClaim {
        get {
            return _investorsToClaim;
        }
        set {
            _investorsToClaim = value;
            referenceHub.investorMenu.investorsToClaimText.text = _investorsToClaim.ToString();
        }
    }
    public SafeUDecimal differenceOfMaterial = 0;
    public SafeUInteger quantityToAddInvestor = 10;

    private List<ShopItem> _shopItems;
    public List<ShopItem> ShopItems {
        get {
            _shopItems ??= new();
            return _shopItems;
        }
    }

    private List<InvestorUpgradeInstance> _investorUpgrades;
    public List<InvestorUpgradeInstance> InvestorUpgrades {
        get {
            _investorUpgrades ??= new();
            return _investorUpgrades;
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
            if(!Purchased && referenceHub.inventoryMenu.Money.Count >= Price) {
                referenceHub.inventoryMenu.Money.Count -= Price;
                Purchased = true;
            }
            else if(Purchased) {
                referenceHub.worldMenu.CurrentWorldLocation = this;
            }
        });
    }

    public void InitLocation(ReferenceHub _referenceHub,WorldLocationData location) {
        referenceHub = _referenceHub;
        Name = location.name;
        Price = location.price;
        MainResourceName = location.mainResource;
        mainResourceIcon.sprite = referenceHub.inventoryMenu.OreItemsSlots[MainResourceName].ItemTemplate.icon;
        Purchased = (Price == 0);
        mainResourceAutoIncrementTimer = 0.0f;
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

    private SafeUInteger _price = 0;
    public SafeUInteger Price {
        get {
            return _price;
        }
        private set {
            _price = value;
            priceText.text = $"Unlock for ${_price}";
        }
    }
}
