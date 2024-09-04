using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
//TOOOOOOOOOOOOOOO DOOOOOOOOOOOOOOOOOOOOOOOOOOOOO Comments
public class CurrentWorldLocationMenu : MonoBehaviour {
    [SerializeField]
    private Button mainButton;
    [SerializeField]
    private InventoryMenu inventoryMenu;
    [SerializeField]
    private WorldMenu worldMenu;
    [SerializeField]
    private FactoryMenu factoryMenu;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private Button levelUpButton;
    [SerializeField]
    private Slider expierienceSlider;
    [SerializeField]
    private TMP_Text mainResourceCountText;
    [SerializeField]
    private Image mainResourceIcon;
    [SerializeField]
    private TMP_Text moneyCountText;
    [SerializeField]
    private TMP_Text workerGainText;
    [SerializeField]
    private GameObject workersInfo;
    [SerializeField]
    private ViewSelection viewSelection;
    [SerializeField]
    private SaveSystem saveSystem;
    private int clicks = 0;

    private void Awake() {
        mainButton.onClick.AddListener(OnMainButtonClick);
        levelUpButton.onClick.AddListener(OnLevelUpButtonClick);
        inventoryMenu.Money.OnCountChanged += OnMoneyCountChanged;
        worldMenu.OnWorldLocationLeft += (WorldLocation location) => {
            if(location != null) {
                inventoryMenu.ResourceInstances[location.MainResourceName].OnCountChanged -= OnMainResourceCountChanged;
                location.OnMainResourceAutoIncrementChange -= OnMainResourceAutoIncrementChange;
                location.OnLevelChange -= OnLevelChange;
                location.OnExperienceChange -= OnExperienceChange;
            }
        };
        worldMenu.OnWorldLocationEntered += (WorldLocation location) => {
            var resource = inventoryMenu.ResourceInstances[location.MainResourceName];
            mainResourceIcon.sprite = resource.Icon;
            resource.OnCountChanged += OnMainResourceCountChanged;
            OnMainResourceCountChanged(resource.Count);
            location.OnMainResourceAutoIncrementChange += OnMainResourceAutoIncrementChange;
            OnMainResourceAutoIncrementChange(location.MainResourceAutoIncrement());
            location.OnLevelChange += OnLevelChange;
            OnLevelChange(location.Level);
            location.OnExperienceChange += OnExperienceChange;
            OnExperienceChange(location.Experience,location.maxExperience);
            clicks = 0;
        };
        viewSelection.Init();
        saveSystem.LoadGame();
    }

    private void Update() {
        foreach(var location in worldMenu.WorldLocations) {
            location.mainResourceAutoIncrementTimer += Time.deltaTime;
            while(location.mainResourceAutoIncrementTimer >= 1.0f) {
                inventoryMenu.ResourceInstances[location.MainResourceName].Count += location.MainResourceAutoIncrement();
                location.mainResourceAutoIncrementTimer -= 1.0f;
            }
        }
        factoryMenu.UpdateFactories();
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Tab)) {
            Debug.LogWarning("CHEATER!!!");
            foreach(var resource in inventoryMenu.ResourceInstances.Values) {
                resource.Count += 1000000;
            }
        }
#endif
    }

    private void OnMoneyCountChanged(Rational value) {
        moneyCountText.text = NumberFormat.ShortForm(value);
    }

    private void OnMainResourceCountChanged(Rational value) {
        mainResourceCountText.text = NumberFormat.ShortForm(value);
    }

    private void OnMainResourceAutoIncrementChange(Rational value) {
        workersInfo.SetActive(value > 0);
        workerGainText.text = $"{NumberFormat.ShortForm(value)} / s";
    }

    private void OnLevelChange(ulong level) {
        levelText.text = NumberFormat.ShortForm(level);
    }

    private void OnExperienceChange(double experience,double maxExperience) {
        expierienceSlider.value = (float)(experience / maxExperience);
        levelUpButton.gameObject.SetActive(Math.Abs(experience - maxExperience) < 0.0001);
    }

    private void OnMainButtonClick() {
        clicks++;
        var cwl = worldMenu.CurrentWorldLocation;
        if(inventoryMenu.ResourceInstances[cwl.MainResourceName].ClicksToPop == clicks) {
            inventoryMenu.ResourceInstances[cwl.MainResourceName].Count += cwl.MainResourceClickIncrement();
            cwl.Experience += new System.Random().NextDouble(1.0,5.0);
            clicks = 0;
        }
    }

    private void OnLevelUpButtonClick() {
        var cwl = worldMenu.CurrentWorldLocation;
        cwl.Level += 1;
        cwl.maxExperience = cwl.Level * 20.0 + new System.Random().NextDouble(30.0,60.0);
        cwl.Experience = 0.0;
    }
}
