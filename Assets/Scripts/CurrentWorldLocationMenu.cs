using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWorldLocationMenu : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
    [SerializeField]
    private Button mainButton;
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
    private Button igniteDynamiteButton;
    private ulong clicks = 0;
    private ulong powerOfDynamite = 100;

    private void Awake() {
        mainButton.onClick.AddListener(OnMainButtonClick);
        levelUpButton.onClick.AddListener(OnLevelUpButtonClick);
        igniteDynamiteButton.onClick.AddListener(ExtinctTheWorld);
        referenceHub.inventoryMenu.Money.OnCountChanged += OnMoneyCountChanged;
        referenceHub.worldMenu.OnWorldLocationLeft += (WorldLocation location) => {
            if(location != null) {
                referenceHub.inventoryMenu.OreItemsSlots[location.MainResourceName].OnCountChanged -= OnMainResourceCountChanged;
                location.OnMainResourceAutoIncrementChange -= OnMainResourceAutoIncrementChange;
                location.OnLevelChange -= OnLevelChange;
                location.OnExperienceChange -= OnExperienceChange;
            }
        };
        referenceHub.worldMenu.OnWorldLocationEntered += (WorldLocation location) => {
            var resource = referenceHub.inventoryMenu.OreItemsSlots[location.MainResourceName];
            mainResourceIcon.sprite = resource.ItemTemplate.icon;
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
        referenceHub.saveSystem.LoadGame();
    }

    private void Update() {
        foreach(var location in referenceHub.worldMenu.WorldLocations) {
            location.mainResourceAutoIncrementTimer += Time.deltaTime;
            while(location.mainResourceAutoIncrementTimer >= 1.0f) {
                referenceHub.inventoryMenu.OreItemsSlots[location.MainResourceName].Count += location.MainResourceAutoIncrement();
                location.mainResourceAutoIncrementTimer -= 1.0f;
            }
        }
        referenceHub.factoryMenu.UpdateFactories();
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Tab)) {
            Debug.LogWarning("CHEATER!!!");
            foreach(var resource in referenceHub.inventoryMenu.OreItemsSlots.Values) {
                resource.Count += 1000000;
            }
        }
#endif
    }

    private void OnMoneyCountChanged(SafeUInteger value) {
        moneyCountText.text = value.ToString();
    }

    private void OnMainResourceCountChanged(SafeUInteger value) {
        mainResourceCountText.text = value.ToString();
    }

    private void OnMainResourceAutoIncrementChange(SafeUInteger value) {
        workersInfo.SetActive(value > 0);
        workerGainText.text = $"{value} / s";
    }

    private void OnLevelChange(ulong level) {
        levelText.text = level.ToString();
    }

    private void OnExperienceChange(double experience,double maxExperience) {
        expierienceSlider.value = (float)(experience / maxExperience);
        levelUpButton.gameObject.SetActive(Math.Abs(experience - maxExperience) < 0.0001);
    }

    private void OnMainButtonClick() {
        clicks++;
        var cwl = referenceHub.worldMenu.CurrentWorldLocation;
        var slots = referenceHub.inventoryMenu.OreItemsSlots;
        if(slots[cwl.MainResourceName].ItemTemplate.clicksToPop <= clicks) {
            slots[cwl.MainResourceName].Count += cwl.MainResourceClickIncrement();
            cwl.Experience += new System.Random().NextDouble(1.0,5.0);
            clicks = 0;
        }
    }

    private void OnLevelUpButtonClick() {
        var cwl = referenceHub.worldMenu.CurrentWorldLocation;
        cwl.Level += 1;
        cwl.maxExperience = cwl.Level * 20.0 + new System.Random().NextDouble(30.0,60.0);
        cwl.Experience = 0.0;
    }

    private void ExtinctTheWorld()
    {
        if (referenceHub.inventoryMenu.CanRemoveItems("Dynamite", 1))
        {
            referenceHub.inventoryMenu.RemoveItems("Dynamite", 1);
            var cwl = referenceHub.worldMenu.CurrentWorldLocation;
            var slots = referenceHub.inventoryMenu.OreItemsSlots;
            slots[cwl.MainResourceName].Count += powerOfDynamite/slots[cwl.MainResourceName].ItemTemplate.clicksToPop;
            clicks += powerOfDynamite % slots[cwl.MainResourceName].ItemTemplate.clicksToPop;
        }
    }
}
