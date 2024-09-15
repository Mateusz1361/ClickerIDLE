using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryItem : MonoBehaviour {
    [SerializeField]
    private Image resultItemIcon;
    [SerializeField]
    private TMP_Text resultItemCountText;
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Button startProcessButton;
    [SerializeField]
    private Slider progressSlider; 
    [SerializeField] 
    private Transform parentOfPrices;
    [SerializeField]
    private GameObject factoryPricePrefab;
    [SerializeField]
    private GameObject unlockMarker;
    [SerializeField]
    private TMP_Text unlockLevelText;
    private ReferenceHub referenceHub;
    private float duration = 0.0f;
    private float elapsed = 0.0f;
    private bool isUpdating = false;
    private string mineToUnlock = "";
    private FactoryResultData factoryResultData;

    private void Awake() {
        startProcessButton.onClick.AddListener(StartFactory);
    }

    private void OnEnable() {
        if(referenceHub.worldMenu) {
            unlockMarker.SetActive(!referenceHub.worldMenu.WorldLocations.Find((location) => location.Name.Equals(mineToUnlock)).Purchased);
        }
    }

    public void Init(FactoryItemData factoryItemData,ReferenceHub _referenceHub) {
        referenceHub = _referenceHub;
        duration = factoryItemData.duration;
        nameText.text = factoryItemData.name;
        progressSlider.value = 0;
        foreach(var price in factoryItemData.price) {
            var prefab = Instantiate(factoryPricePrefab,parentOfPrices);
            var component  = prefab.GetComponent<ShopItemPrice>();
            component.InitPrice(referenceHub.inventoryMenu,null,new() { name = price.name, unlockCount = 0, value = price.value });
        }
        factoryResultData = factoryItemData.result;
        resultItemIcon.sprite = referenceHub.inventoryMenu.ItemTemplates[factoryResultData.type].icon;
        resultItemCountText.text = factoryResultData.value.ToString();
        mineToUnlock = factoryItemData.toUnlock;
        unlockLevelText.text = $"Unlocked when {mineToUnlock} is unlocked";
    }

    public void StartFactory() {
        var children = parentOfPrices.GetComponentsInChildren<ShopItemPrice>();
        bool canAfford = true;
        foreach(var price in children) {
            if(!referenceHub.inventoryMenu.CanRemoveItems(price.Name,price.Value)) {
                canAfford = false;
                break;
            }
        }
        if(canAfford) {
            foreach(var price in children) {
                referenceHub.inventoryMenu.RemoveItems(price.Name,price.Value);
            }
            isUpdating = true;
            elapsed = 0.0f;
            startProcessButton.interactable = false;
        }
    }

    public void UpdateFactory() {
        if(isUpdating) {
            elapsed += Time.deltaTime;
            progressSlider.value = Mathf.Clamp01(elapsed / duration);
            if(elapsed >= duration) {
                elapsed = duration;
                if(referenceHub.inventoryMenu.CanAddItems(factoryResultData.type,factoryResultData.value)) {
                    referenceHub.inventoryMenu.AddItems(factoryResultData.type,factoryResultData.value);
                    progressSlider.value = 0.0f;
                    isUpdating = false;
                    startProcessButton.interactable = true;
                    if (factoryResultData.type == "Dynamite")
                    {
                        referenceHub.currentWorldLocationMenu.RefreshQuantityOfDynamite();
                    }
                }
            }
        }
    }
}
