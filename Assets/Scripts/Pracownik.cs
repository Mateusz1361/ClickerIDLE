using System;
using System.IO;
using UnityEngine;

[Serializable]
public class WorkerInstanceData
{
    public int price;
    public int power;
    public string icon;
}

[Serializable]
public class WorkersData
{
    public WorkerInstanceData[] data;

}

public class Pracownik : MonoBehaviour
{
    [SerializeField] private ClickerManager clickerManager;
    [SerializeField]
    private GameObject workerPrefab;
    [SerializeField]
    private GameObject parent;

    private void Start()
    {
        
        var workersData = JsonUtility.FromJson<WorkersData>(File.ReadAllText(Application.streamingAssetsPath + "/WorkersData.json"));
        foreach (var workerData in workersData.data)
        {
            var icon = Resources.Load<Sprite>(workerData.icon);
            var worker = Instantiate(workerPrefab, parent.transform);
            var instance = worker.GetComponent<WorkerInstance>();
            instance.clickerManager = clickerManager;
            instance.iconOfWorker.sprite=icon;
            instance.Price = workerData.price;
            instance.Power = workerData.power;
            instance.Quantity = 0;
        }
    }
    
}

