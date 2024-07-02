using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject resourceInstancePrefab;
    [SerializeField]
    private GameObject parent;
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

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        if(_resourceInstances == null) {
            InitResourceInstances();
        }
    }

    private void InitResourceInstances() {
        _resourceInstances = new();
        var resourceInstanceDatas = JsonUtility.FromJson<InstanceWrapper<ResourceInstanceData>>(resourcesData.text);
        foreach(var resourceInstanceData in resourceInstanceDatas.data) {
            var prefab = Instantiate(resourceInstancePrefab,parent.transform);
            var resourceInstance = prefab.GetComponent<ResourceInstance>();
            resourceInstance.InitInstance(resourceInstanceData);
            _resourceInstances.Add(resourceInstanceData.name,resourceInstance);
        }
    }
}
