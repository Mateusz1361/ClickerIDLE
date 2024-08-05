using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInstance : MonoBehaviour {
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text countText;

    private ResourceInstanceData data;
    
    public event Action<Rational> OnCountChanged;
    public event Action<Rational> OnCountIncrement;

    public Sprite Icon => iconImage.sprite;
    public string Name => data.name;
    public int ClicksToPop { get { return data.clicksToPop; }  }
    public void InitInstance(ResourceInstanceData _data) {
        data = _data;
        iconImage.sprite = Resources.Load<Sprite>($"Images/{data.iconPath}");
        nameText.text = data.name;
        Count = 0;
    }

    private Rational _count;
    public Rational Count {
        get {
            return _count;
        }
        set {
            if(value > _count) {
                OnCountIncrement?.Invoke(value - _count);
            }
            _count = value;
            countText.text = NumberFormat.ShortForm(_count);
            OnCountChanged?.Invoke(_count);
        }
    }
}
