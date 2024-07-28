using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour {
    [SerializeField]
    private Image icon;
    [SerializeField]
    private TMP_Text countText;

    public Sprite Icon {
        get {
            return icon.sprite;
        }
        set {
            icon.enabled = (value != null);
            icon.sprite = value;
        }
    }

    public void SetCount(uint count) {
        countText.text = (count <= 1) ? "" : count.ToString();
    }
}
