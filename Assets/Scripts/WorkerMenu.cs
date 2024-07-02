using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WorkerMenu : MonoBehaviour {
    [SerializeField]
    private MainView mainView;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject workerInstancePrefab;
    [SerializeField]
    private GameObject parent;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        InitWorkers();
    }

    private void InitWorkers() {
        var workerInstanceDatas = JsonUtility.FromJson<InstanceWrapper<WorkerInstanceData>>(File.ReadAllText(Application.streamingAssetsPath + "/WorkersData.json"));
        foreach(var workerInstanceData in workerInstanceDatas.data) {
            var prefab = Instantiate(workerInstancePrefab,parent.transform);
            var workerInstance = prefab.GetComponent<WorkerInstance>();
            workerInstance.InitInstance(mainView,workerInstanceData.price,workerInstanceData.price,workerInstanceData.unlockLevel);
        }
    }
}
