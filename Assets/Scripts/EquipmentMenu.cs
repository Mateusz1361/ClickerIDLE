using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EquipmentMenu : MonoBehaviour {
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject oreInstancePrefab;
    [SerializeField]
    private GameObject parent;

    private Dictionary<string,OreInstance> _oreInstances;

    public Dictionary<string,OreInstance> OreInstances {
        get {
            if(_oreInstances == null) {
                InitOreInstances();
            }
            return _oreInstances;
        }
    }

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        if(_oreInstances == null) {
            InitOreInstances();
        }
    }

    private void InitOreInstances() {
        _oreInstances = new();
        var oreInstanceDatas = JsonUtility.FromJson<InstanceWrapper<OreInstanceData>>(File.ReadAllText(Application.streamingAssetsPath + "/OresData.json"));
        foreach(var oreInstanceData in oreInstanceDatas.data) {
            var prefab = Instantiate(oreInstancePrefab,parent.transform);
            var oreInstance = prefab.GetComponent<OreInstance>();
            oreInstance.InitInstance(Resources.Load<Sprite>($"Images/OreIcons/{oreInstanceData.iconName}"),oreInstanceData.name);
            oreInstance.Count = 0;
            oreInstance.minDrop = oreInstanceData.minDrop;
            oreInstance.maxDrop = oreInstanceData.maxDrop;
            _oreInstances.Add(oreInstanceData.name,oreInstance);
        }
    }
}
