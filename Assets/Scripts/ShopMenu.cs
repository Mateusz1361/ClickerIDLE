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
    private Button OpenShopButton;
    [SerializeField]
    private GameObject buyOptionPrefab;
    [SerializeField]
    public GameObject parent;
    [SerializeField]
    private TextAsset shopData;
    [SerializeField]
    private WorkerMenu workerMenu;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        OpenShopButton.onClick.AddListener(() => { parent.gameObject.SetActive(true); workerMenu.parent.gameObject.SetActive(false); });
        InitBuyOptions();
       
    }

    private void InitBuyOptions() {
        var buyOptionInstanceDatas = JsonUtility.FromJson<InstanceWrapper<BuyOptionInstanceData>>(shopData.text);
        foreach(var buyOptionInstanceData in buyOptionInstanceDatas.data) {
            var prefab = Instantiate(buyOptionPrefab,parent.transform);
            var buyOptionInstance = prefab.GetComponent<BuyOptionInstance>();
            buyOptionInstance.InitInstance(mainView,equipmentMenu,buyOptionInstanceData);
        }
    }
}
