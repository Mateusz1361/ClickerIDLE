using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private WorldMenu worldMenu;
    [SerializeField]
    private InventoryMenu inventoryMenu;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject shopItemPrefab;
    [SerializeField]
    private RectTransform shopItemsContent;
    [SerializeField]
    private TextAsset shopDataTextAsset;

    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        var shopData = JsonUtility.FromJson<InstanceWrapper<ShopItemData>>(shopDataTextAsset.text);
        
        foreach(var worldLocation in worldMenu.WorldLocations) {
            int index = 0;
            foreach(var item in shopData.data) {
                var prefab = Instantiate(shopItemPrefab,shopItemsContent);
                var component = prefab.GetComponent<ShopItem>();
                component.InitItem(worldLocation,inventoryMenu,item,index);
                prefab.SetActive(false);
                worldLocation.ShopItems.Add(component);
                index++;
            }
        }
        worldMenu.OnWorldLocationLeft += OnWorldLocationLeft;
        worldMenu.OnWorldLocationEntered += OnWorldLocationEntered;
        OnWorldLocationEntered(worldMenu.CurrentWorldLocation);
    }

    private void OnWorldLocationLeft(WorldLocation location) {
        foreach(var item in location.ShopItems) {
            item.gameObject.SetActive(false);
        }
    }

    private void OnWorldLocationEntered(WorldLocation location) {
        foreach(var item in location.ShopItems) {
            item.gameObject.SetActive(true);
        }
    }
}
