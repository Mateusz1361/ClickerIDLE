using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//TODO zapis gry cos jest nie tak w upgradach
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
    private TMP_Text dynamiteCountText;
    [SerializeField]
    private Image dynamiteIcon;
    [SerializeField] 
    private Button dynamiteIgniteButton;

    private ulong clicks = 0;
    
    private MutablePair<SafeUDecimal,ItemTemplate> bestDynamiteToUse = null;

    private void Awake() {
        mainButton.onClick.AddListener(OnMainButtonClick);
        levelUpButton.onClick.AddListener(OnLevelUpButtonClick);
        dynamiteIgniteButton.onClick.AddListener(ExtinctTheWorld);
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

    private void OnEnable() {
        UpdateDynamiteStatus();
    }

    public void UpdateDynamiteStatus() {
        Dictionary<string,MutablePair<SafeUDecimal,ItemTemplate>> dynamiteItems = new();
        foreach(var slot in referenceHub.inventoryMenu.itemSlots) {
            if(slot.ItemTemplate == null || !slot.ItemTemplate.name.Contains("Dynamite")) continue;
            if(!dynamiteItems.ContainsKey(slot.ItemTemplate.name)) {
                dynamiteItems[slot.ItemTemplate.name] = new() {
                    first = 0,
                    second = slot.ItemTemplate
                };
            }
            dynamiteItems[slot.ItemTemplate.name].first += slot.Count;
        }
        bestDynamiteToUse = null;
        foreach(var pair in dynamiteItems.Values) {
            if(bestDynamiteToUse == null || pair.second.powerOfDynamite > bestDynamiteToUse.second.powerOfDynamite) {
                bestDynamiteToUse = pair;
            }
        }
        dynamiteIcon.sprite = (bestDynamiteToUse != null) ? bestDynamiteToUse.second.icon : referenceHub.inventoryMenu.ItemTemplates["Dynamite"].icon;
        dynamiteCountText.text = (bestDynamiteToUse != null) ? NumberFormat.ShortForm(bestDynamiteToUse.first) : "0";
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

    private void OnMoneyCountChanged(SafeUDecimal value) {
        moneyCountText.text = NumberFormat.ShortForm(value);
    }

    private void OnMainResourceCountChanged(SafeUDecimal value) {
        mainResourceCountText.text = NumberFormat.ShortForm(value);
    }

    private void OnMainResourceAutoIncrementChange(SafeUDecimal value) {
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
        clicks += referenceHub.inventoryMenu.GetClickPower();
        var cwl = referenceHub.worldMenu.CurrentWorldLocation;
        var slots = referenceHub.inventoryMenu.OreItemsSlots;
        if(slots[cwl.MainResourceName].ItemTemplate.clicksToPop <= clicks) {
            slots[cwl.MainResourceName].Count += cwl.MainResourceClickIncrement() * (clicks / slots[cwl.MainResourceName].ItemTemplate.clicksToPop);
            clicks %= slots[cwl.MainResourceName].ItemTemplate.clicksToPop;
        }
        cwl.Experience += new System.Random().NextDouble(1.0,5.0);
    }

    private void OnLevelUpButtonClick() {
        var cwl = referenceHub.worldMenu.CurrentWorldLocation;
        cwl.Level += 1;
        cwl.maxExperience = cwl.Level * 20.0 + new System.Random().NextDouble(30.0,60.0);
        cwl.Experience = 0.0;
    }

    private void ExtinctTheWorld() {
        if(bestDynamiteToUse == null) return;
        var bestDynamiteName = bestDynamiteToUse.second.name;
        if(referenceHub.inventoryMenu.CanRemoveItems(bestDynamiteName,1)) {
            referenceHub.inventoryMenu.RemoveItems(bestDynamiteName,1);
            var cwl = referenceHub.worldMenu.CurrentWorldLocation;
            var slots = referenceHub.inventoryMenu.OreItemsSlots;
            clicks += referenceHub.inventoryMenu.ItemTemplates[bestDynamiteName].powerOfDynamite;
            slots[cwl.MainResourceName].Count += clicks / slots[cwl.MainResourceName].ItemTemplate.clicksToPop;
            clicks = referenceHub.inventoryMenu.ItemTemplates[bestDynamiteName].powerOfDynamite % slots[cwl.MainResourceName].ItemTemplate.clicksToPop;
            UpdateDynamiteStatus();
        }
    }
}
