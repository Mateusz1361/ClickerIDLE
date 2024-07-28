using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WorldMenu : MonoBehaviour {
    [SerializeField]
    private InventoryMenu inventoryMenu;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private GameObject worldLocationPrefab;
    [SerializeField]
    private RectTransform worldMap;
    [SerializeField]
    private TextAsset worldMapDataAsset;
    [SerializeField]
    private ScrollRect scrollRect;

    private List<WorldLocation> _worldLocations;
    public List<WorldLocation> WorldLocations {
        get {
            if(_worldLocations == null) {
                InitWorldLocations();
            }
            return _worldLocations;
        }
    }

    public event Action<WorldLocation> OnWorldLocationLeft;
    public event Action<WorldLocation> OnWorldLocationEntered;

    private WorldLocation _currentWorldLocation;
    public WorldLocation CurrentWorldLocation {
        get {
            return _currentWorldLocation;
        }
        set {
            if(_currentWorldLocation != value) {
                OnWorldLocationLeft?.Invoke(_currentWorldLocation);
                _currentWorldLocation = value;
                OnWorldLocationEntered?.Invoke(_currentWorldLocation);
            }
            gameObject.SetActive(false);
        }
    }

    public void Init() {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        scrollRect.horizontalNormalizedPosition = 0.0f;
        scrollRect.verticalNormalizedPosition = 1.0f;
        if(_worldLocations == null) {
            InitWorldLocations();
        }
    }

    private void InitWorldLocations() {
        _worldLocations = new();
        var worldMapData = JsonUtility.FromJson<WorldMapData>(worldMapDataAsset.text);
        worldMap.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,worldMapData.sizeX);
        worldMap.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,worldMapData.sizeY);
        
        foreach(var worldLocationData in worldMapData.data) {
            var prefab = Instantiate(worldLocationPrefab,worldMap);
            prefab.GetComponent<RectTransform>().anchoredPosition = new(worldLocationData.posX,-worldLocationData.posY);
            var component = prefab.GetComponent<WorldLocation>();
            component.InitLocation(this,inventoryMenu,worldLocationData);
            if(worldLocationData.price == 0) {
                CurrentWorldLocation = component;
            }
            _worldLocations.Add(component);
        }
    }
}
