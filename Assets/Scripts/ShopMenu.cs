using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private MainView mainView;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject buyOptionPrefab;
    [SerializeField]
    private GameObject parent;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        InitBuyOptions();
    }

    private void InitBuyOptions() {
        var buyOptionInstanceDatas = JsonUtility.FromJson<InstanceWrapper<BuyOptionInstanceData>>(File.ReadAllText(Application.streamingAssetsPath + "/ShopData.json"));
        foreach(var buyOptionInstanceData in buyOptionInstanceDatas.data) {
            var prefab = Instantiate(buyOptionPrefab,parent.transform);
            var buyOptionInstance = prefab.GetComponent<BuyOptionInstance>();
            buyOptionInstance.InitInstance(mainView,buyOptionInstanceData.price,buyOptionInstanceData.power,buyOptionInstanceData.unlockLevel);
        }
    }
}
