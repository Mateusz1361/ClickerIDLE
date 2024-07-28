using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject resourceInstancePrefab;
    [SerializeField]
    private GameObject oresScrollRect;
    [SerializeField]
    private GameObject oresParent;
    [SerializeField]
    private Button oresButton;
    [SerializeField]
    private GameObject itemsScroll;
    [SerializeField]
    private Button itemsButton;
    [SerializeField]
    private TextAsset resourcesData;

    private Dictionary<string,ResourceInstance> _resourceInstances;

    public Dictionary<string,ResourceInstance> ResourceInstances {
        get {
            if(_resourceInstances == null) {
                InitResourceInstances();
            }
            return _resourceInstances;
        }
    }

    private ResourceInstance _stone;
    public ResourceInstance Stone {
        get {
            if(_stone == null) {
                _stone = ResourceInstances["Stone"];
            }
            return _stone;
        }
    }

    private ResourceInstance _money;
    public ResourceInstance Money {
        get {
            if(_money == null) {
                _money = ResourceInstances["Money"];
            }
            return _money;
        }
    }

    public void Init() {
        oresScrollRect.SetActive(true);
        itemsScroll.SetActive(false);
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        oresButton.onClick.AddListener(() => {
            oresScrollRect.SetActive(true);
            itemsScroll.SetActive(false);
        });
        itemsButton.onClick.AddListener(() => {
            oresScrollRect.SetActive(false);
            itemsScroll.SetActive(true);
        });
        if(_resourceInstances == null) {
            InitResourceInstances();
        }
    }

    private void InitResourceInstances() {
        _resourceInstances = new();
        var resourceInstanceDatas = JsonUtility.FromJson<InstanceWrapper<ResourceInstanceData>>(resourcesData.text);
        foreach(var resourceInstanceData in resourceInstanceDatas.data) {
            var prefab = Instantiate(resourceInstancePrefab,oresParent.transform);
            var resourceInstance = prefab.GetComponent<ResourceInstance>();
            resourceInstance.InitInstance(resourceInstanceData);
            _resourceInstances.Add(resourceInstanceData.name,resourceInstance);
        }
    }
}
