using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OreInstance : MonoBehaviour {
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text countText;

    public void InitInstance(Sprite icon,string name) {
        iconImage.sprite = icon;
        nameText.text = name;
        Count = 0;
    }

    public float minDrop;
    public float maxDrop;

    private ulong _count;
    public ulong Count {
        get {
            return _count;
        }
        set {
            _count = value;
            countText.text = _count.ToString();
        }
    }
}
