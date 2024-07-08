using UnityEngine;
using UnityEngine.UI;

public class WorkerMenu : MonoBehaviour {
    [SerializeField]
    private MainView mainView;
    [SerializeField]
    private EquipmentMenu equipmentMenu;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button OpenWorkersButton;
    [SerializeField]
    private GameObject workerInstancePrefab;
    [SerializeField]
    public GameObject parent;
    [SerializeField]
    private TextAsset workersData;
    [SerializeField]
    private ShopMenu shopMenu;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        OpenWorkersButton.onClick.AddListener(() => { parent.gameObject.SetActive(true); shopMenu.parent.SetActive(false); });
        InitWorkers();
        
    }

    private void InitWorkers() {
        var workerInstanceDatas = JsonUtility.FromJson<InstanceWrapper<WorkerInstanceData>>(workersData.text);
        foreach(var workerInstanceData in workerInstanceDatas.data) {
            var prefab = Instantiate(workerInstancePrefab,parent.transform);
            var workerInstance = prefab.GetComponent<WorkerInstance>();
            workerInstance.InitInstance(mainView,equipmentMenu,workerInstanceData.price,workerInstanceData.power,workerInstanceData.unlockLevel);
        }
    }
}
