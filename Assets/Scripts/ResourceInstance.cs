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

    public Sprite Icon => iconImage.sprite;
    public string Name => data.name;

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
            _count = value;
            countText.text = _count.ToString();
            OnCountChanged?.Invoke(_count);
        }
    }
}
