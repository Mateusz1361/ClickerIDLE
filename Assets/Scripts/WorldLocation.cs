using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class WorldLocation : MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Button acceptButton;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text priceText;
    [SerializeField]
    private GameObject lockedMarker;
    private EquipmentMenu equipmentMenu;

    public bool Purchased { get; private set; }

    private void Awake() {
        acceptButton.onClick.AddListener(() => {
            if(!Purchased) {
                Purchase();
            }
            else {
                Debug.Log($"@TODO: Switching to mine '{Name}'.");
            }
        });
    }

    private void OnEnable() {
        lockedMarker.SetActive(!Purchased);
        priceText.gameObject.SetActive(lockedMarker.activeSelf);
    }

    public void InitLocation(EquipmentMenu _equipmentMenu,string name,BigInteger price) {
        equipmentMenu = _equipmentMenu;
        Name = name;
        Price = price;
        Purchased = (Price == 0);
        OnEnable();
    }

    private void Purchase() {
        if(equipmentMenu.Money.Count >= Price) {
            equipmentMenu.Money.Count -= Price;
            Purchased = true;
            OnEnable();
        }
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
