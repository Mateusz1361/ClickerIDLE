using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class WorldMap : MonoBehaviour {
    [SerializeField]
    private EquipmentMenu equipmentMenu;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject worldLocationPrefab;
    [SerializeField]
    private Transform worldLocationParent;

    private void Awake() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        InstantiateWorldLocation("Stone Mine",0);
        InstantiateWorldLocation("Coal Mine",1000000);
        InstantiateWorldLocation("Gold Mine",100000000);
    }

    private void InstantiateWorldLocation(string name,BigInteger price) {
        var prefab = Instantiate(worldLocationPrefab,worldLocationParent);
        prefab.GetComponent<WorldLocation>().InitLocation(equipmentMenu,name,price);
    }
}
