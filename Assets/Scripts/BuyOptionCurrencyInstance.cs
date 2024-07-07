using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyOptionCurrencyInstance : MonoBehaviour
{
    public string Name
    {
        get; 
        private set; 
    }
    private ulong _value;
    


    [SerializeField]
    private TMP_Text valueText;
    [SerializeField]
    private Image iconImage; 
    public void InitInstance(string name,ulong value, Sprite icon)
    {
        iconImage.sprite = icon;
        Name = name;
        Value = value;
    }
    public ulong Value {
        get { return _value; }
        set { _value = value; valueText.text = _value.ToString(); }
    }


}
