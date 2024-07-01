using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

[Serializable]
public class WorkerInstanceData
{
    public int price;
    public int power;
    public int unlocklevel;
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

    public void Workers()
    {
        List<int> quantities = new List<int>();
        foreach (var prefab in parent.GetComponentsInChildren<ShopInstance>())
        {
            quantities.Add(prefab.Quantity);
        }
        while (parent.transform.childCount > 0)
        {
            DestroyImmediate(parent.transform.GetChild(0).gameObject);
        }
        int index = 0;
        var workersData = JsonUtility.FromJson<WorkersData>(File.ReadAllText(Application.streamingAssetsPath + "/WorkersData.json"));
        foreach (var workerData in workersData.data)
        {
            if (workerData.unlocklevel <= clickerManager.poziom)
            {
                var icon = Resources.Load<Sprite>(workerData.icon);
                var worker = Instantiate(workerPrefab, parent.transform);
                var instance = worker.GetComponent<WorkerInstance>();
                instance.clickerManager = clickerManager;
                instance.iconOfWorker.sprite = icon;
                
                instance.Power = workerData.power;
                
                if (index < quantities.Count)
                {
                    instance.Quantity = quantities[index];
                }
                else instance.Quantity = 0;
                if (instance.Quantity > 0)
                {
                    instance.Price = (int)(workerData.price * Math.Pow(2, instance.Quantity));
                }
                else instance.Price = workerData.price;
                index +=1;
            }
        }
    }
    
}

