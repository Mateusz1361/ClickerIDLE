using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TradeMenu : MonoBehaviour {
    [SerializeField]
    private MainView mainView;
    [SerializeField]
    private EquipmentMenu equipmentMenu;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject tradeOptionPrefab;
    [SerializeField]
    private GameObject parent;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        InitTradeOptions();
    }

    private void InitTradeOptions() {
        var tradeOptionInstanceDatas = JsonUtility.FromJson<InstanceWrapper<TradeOptionInstanceData>>(File.ReadAllText(Application.streamingAssetsPath + "/TradeOptionData.json"));
        foreach(var tradeOptionInstanceData in tradeOptionInstanceDatas.data) {
            var prefab = Instantiate(tradeOptionPrefab,parent.transform);
            var tradeOptionInstance = prefab.GetComponent<TradeOptionInstance>();

            var spriteInName = tradeOptionInstanceData.currencyIn.Equals("Dollar") ? "Images/DollarSign" :
                               tradeOptionInstanceData.currencyIn.Equals("Stone") ? "Images/Stone" :
                               $"Images/OreIcons/{tradeOptionInstanceData.currencyIn}Icon";
            var spriteOutName = tradeOptionInstanceData.currencyOut.Equals("Dollar") ? "Images/DollarSign" :
                                tradeOptionInstanceData.currencyIn.Equals("Stone") ? "Images/Stone" :
                                $"Images/OreIcons/{tradeOptionInstanceData.currencyOut}Icon";

            tradeOptionInstance.InitInstance(
                mainView,equipmentMenu,
                Resources.Load<Sprite>(spriteInName),
                Resources.Load<Sprite>(spriteOutName),
                tradeOptionInstanceData.price,
                tradeOptionInstanceData.gain,
                tradeOptionInstanceData.currencyIn,
                tradeOptionInstanceData.currencyOut
            );
        }
    }
}
