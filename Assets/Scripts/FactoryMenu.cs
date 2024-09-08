using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FactoryMenu : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private TextAsset factoryDataTextAsset;
    [SerializeField]
    private GameObject factoryPrefab;
    [SerializeField]
    private Transform factoryPrefabParent;

    private readonly List<FactoryItem> factoryItems = new();
    
    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        var factoryData = JsonUtility.FromJson<InstanceWrapperData<FactoryItemData>>(factoryDataTextAsset.text);
        foreach(var item in factoryData.data) {
            var prefab = Instantiate(factoryPrefab,factoryPrefabParent);
            var component = prefab.GetComponent<FactoryItem>();
            component.Init(item,referenceHub);
            factoryItems.Add(component);
        }
    }

    public void UpdateFactories() {
        foreach(var item in factoryItems) {
            item.UpdateFactory();
        }
    }
}
