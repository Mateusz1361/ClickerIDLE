using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private TextAsset factoryDataTextAsset;
    [SerializeField]
    private GameObject factoryPrefab;
    [SerializeField]
    private Transform factoryPrefabParent;
    [SerializeField]
    private InventoryMenu inventoryMenu;
    private readonly List<FactoryItem> factoryItems = new();
    
    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        var factoryData = JsonUtility.FromJson<InstanceWrapper<FactoryItemData>>(factoryDataTextAsset.text);
        foreach(var item in factoryData.data) {
            var prefab = Instantiate(factoryPrefab,factoryPrefabParent);
            var component = prefab.GetComponent<FactoryItem>();
            component.Init(item,inventoryMenu);
            factoryItems.Add(component);
        }
    }

    public void UpdateFactories() {
        foreach(var item in factoryItems) {
            item.UpdateFactory();
        }
    }
}
