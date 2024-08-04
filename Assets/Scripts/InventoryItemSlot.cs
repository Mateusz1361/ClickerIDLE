using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text countText;

    public void SetIcon(Sprite sprite) {
        icon.sprite = sprite;
        icon.enabled = sprite != null;
    }

    private uint _count;
    public uint Count {
        get {
            return _count;
        }
        set {
            _count = value;
            countText.text = _count <= 1 ? "" : _count.ToString();
        }
    }
}
