using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryItem : MonoBehaviour
{
    [SerializeField]
    private Image iconOfFactory;
    [SerializeField]
    private Button startProcessButton;
    [SerializeField]
    private Slider progressSlider; 
    [SerializeField] 
    private Transform parentOfPrices;
    [SerializeField]
    private GameObject factoryPricePrefab;
    private InventoryMenu inventoryMenu;
    private float duration = 5f;
    private float elapsed = 0f;
    private bool isUpdating = false;
    private FactoryResultData factoryResultData;

    public void Init(FactoryItemData factoryItemData, InventoryMenu _inventoryMenu)
    {
        inventoryMenu = _inventoryMenu;
        startProcessButton.onClick.AddListener(() => StartTheProcess());
        progressSlider.value = 0;
        foreach (var price in factoryItemData.price)
        {
            var prefab = Instantiate(factoryPricePrefab, parentOfPrices);
            var component  = prefab.GetComponent<ShopItemPrice>();
            component.InitPrice(inventoryMenu, null, new() { name = price.name, unlockCount = 0, value = price.value });
            iconOfFactory.sprite = Resources.Load<Sprite>("Images/MineButton");

        }
        factoryResultData = factoryItemData.result;
    }
    public void StartTheProcess()
    {
        bool canAfford = true;
        foreach (var price in parentOfPrices.GetComponentsInChildren<ShopItemPrice>())
        {
            Debug.Log(price.Name);
            if (price.Value > inventoryMenu.ResourceInstances[price.Name].Count)
            {
                canAfford = false;
            }
        }
        if (canAfford)
        {
            foreach (var price in parentOfPrices.GetComponentsInChildren<ShopItemPrice>())
            {
                inventoryMenu.ResourceInstances[price.Name].Count -= price.Value;
            }
            isUpdating = true;
            elapsed = 0f;
            startProcessButton.interactable = false;
            

        }
        
        
    }
    private void Update()
    {
        if (isUpdating)
        {

            if (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                progressSlider.value = Mathf.Clamp01(elapsed / duration);
            }
            else
            {
                progressSlider.value = 0f;
                isUpdating = false;
                startProcessButton.interactable = true;
                inventoryMenu.AddItems(factoryResultData.type, (uint)factoryResultData.value);
            }
        }
        

    }
}
