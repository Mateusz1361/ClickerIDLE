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
    private GameObject parent;
    [SerializeField]
    private TextAsset shopData;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        InitBuyOptions();
    }

    private void InitBuyOptions() {
        var buyOptionInstanceDatas = JsonUtility.FromJson<InstanceWrapper<BuyOptionInstanceData>>(shopData.text);
        foreach(var buyOptionInstanceData in buyOptionInstanceDatas.data) {
            var prefab = Instantiate(buyOptionPrefab,parent.transform);
            var buyOptionInstance = prefab.GetComponent<BuyOptionInstance>();
            buyOptionInstance.InitInstance(mainView,equipmentMenu,buyOptionInstanceData.price,buyOptionInstanceData.power,buyOptionInstanceData.unlockLevel);
        }
    }
}
