using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject shopItemPrefab;
    [SerializeField]
    private TextAsset shopDataTextAsset;
    [SerializeField]
    private Button mineShopButton;
    [SerializeField]
    private Button generalShopButton;
    [SerializeField]
    private RectTransform mineShopItemsContent;
    [SerializeField]
    private RectTransform generalShopItemsContent;
    [SerializeField]
    private GameObject mineShopScroll;
    [SerializeField]
    private GameObject generalShopScroll;

    public readonly List<ShopItem> ShopItems = new();

    private void Awake() {
        mineShopScroll.SetActive(true);
        generalShopScroll.SetActive(false);
        mineShopButton.onClick.AddListener(() => {
            mineShopScroll.SetActive(true);
            generalShopScroll.SetActive(false);
        });
        generalShopButton.onClick.AddListener(() => {
            mineShopScroll.SetActive(false);
            generalShopScroll.SetActive(true);
        });
    }

    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        var shopData = JsonUtility.FromJson<InstanceWrapperData<ShopItemData>>(shopDataTextAsset.text);
        var shopItems = shopData.data.ToList();

        var groups = shopItems.GroupBy((item) => item.belongsToMine).ToDictionary((x) => x.Key,(y) => y.ToArray());
        foreach(var item in groups[false]) {
            var prefab = Instantiate(shopItemPrefab,generalShopItemsContent);
            var component = prefab.GetComponent<ShopItem>();
            component.InitItem(null,item,referenceHub);
            ShopItems.Add(component);
        }
        foreach(var worldLocation in referenceHub.worldMenu.WorldLocations) {
            foreach(var item in groups[true]) {
                var prefab = Instantiate(shopItemPrefab,mineShopItemsContent);
                var component = prefab.GetComponent<ShopItem>();
                component.InitItem(worldLocation,item,referenceHub);
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
