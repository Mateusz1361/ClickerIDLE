using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour {
    [SerializeField]
    private MainView mainView;
    [SerializeField]
    private EquipmentMenu equipmentMenu;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject buyOptionPrefab;
    [SerializeField]
    private Transform itemOptionsParent;
    [SerializeField]
    private Transform workerOptionsParent;
    [SerializeField]
    private Button itemShowButton;
    [SerializeField]
    private Button workerShowButton;
    [SerializeField]
    private TextAsset shopData;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        itemShowButton.onClick.AddListener(() => {
            itemOptionsParent.gameObject.SetActive(true);
            workerOptionsParent.gameObject.SetActive(false);
        });
        workerShowButton.onClick.AddListener(() => {
            itemOptionsParent.gameObject.SetActive(false);
            workerOptionsParent.gameObject.SetActive(true);
        });
        InitBuyOptions();
    }

    private void OnEnable() {
        itemOptionsParent.gameObject.SetActive(true);
        workerOptionsParent.gameObject.SetActive(false);
    }

    private void InitBuyOptions() {
        var buyOptionInstanceDatas = JsonUtility.FromJson<InstanceWrapper<BuyOptionInstanceData>>(shopData.text);
        foreach(var buyOptionInstanceData in buyOptionInstanceDatas.data) {
            GameObject prefab = null;
            if(buyOptionInstanceData.result.type == "Power") {
                prefab = Instantiate(buyOptionPrefab,itemOptionsParent);
            }
            else if(buyOptionInstanceData.result.type == "Worker") {
                prefab = Instantiate(buyOptionPrefab,workerOptionsParent);
            }
            if(prefab != null) {
                var buyOptionInstance = prefab.GetComponent<BuyOptionInstance>();
                buyOptionInstance.InitInstance(mainView,equipmentMenu,buyOptionInstanceData);
            }
        }
    }
}
