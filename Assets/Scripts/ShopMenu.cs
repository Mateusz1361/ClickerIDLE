using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
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
        var shopData = JsonUtility.FromJson<InstanceWrapperData<ShopItemData>>(shopDataTextAsset.text);
        
        foreach(var worldLocation in referenceHub.worldMenu.WorldLocations) {
            foreach(var item in shopData.data) {
                var prefab = Instantiate(shopItemPrefab,shopItemsContent);
                var component = prefab.GetComponent<ShopItem>();
                component.InitItem(worldLocation,referenceHub.inventoryMenu,item);
                prefab.SetActive(false);
                worldLocation.ShopItems.Add(component);
            }
        }
        referenceHub.worldMenu.OnWorldLocationLeft += OnWorldLocationLeft;
        referenceHub.worldMenu.OnWorldLocationEntered += OnWorldLocationEntered;
        OnWorldLocationEntered(referenceHub.worldMenu.CurrentWorldLocation);
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
