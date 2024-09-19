using UnityEngine;
using UnityEngine.UI;

public class TradeMenu : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject tradeOptionPrefab;
    [SerializeField]
    private GameObject parent;
    [SerializeField]
    private TextAsset tradeOptionData;

    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        InitTradeOptions();
    }

    private void InitTradeOptions() {
        var tradeOptionInstanceDatas = JsonUtility.FromJson<InstanceWrapperDataJson<TradeOptionInstanceDataJson>>(tradeOptionData.text);
        foreach(var tradeOptionInstanceData in tradeOptionInstanceDatas.data) {
            var prefab = Instantiate(tradeOptionPrefab,parent.transform);
            var tradeOptionInstance = prefab.GetComponent<TradeOptionInstance>();
            tradeOptionInstance.InitInstance(referenceHub.inventoryMenu,tradeOptionInstanceData);
        }
    }
}
