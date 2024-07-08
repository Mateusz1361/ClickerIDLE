using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public static class RandomExtensions {
    public static double NextDouble(this System.Random random,double min,double max) {
        return random.NextDouble() * (max - min) + min;
    }
}

public class MainView : MonoBehaviour {
    [SerializeField]
    private Button mainButton;
    [SerializeField]
    private EquipmentMenu equipmentMenu;
    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private Button levelUpButton;
    [SerializeField]
    private Slider expierienceSlider;
    [SerializeField]
    private TMP_Text stoneCountText;
    [SerializeField]
    private TMP_Text moneyCountText;
    [SerializeField]
    private GameObject workerInfo;
    [SerializeField]
    private TMP_Text workerGainText;

    public ulong StoneIncrement { get; set; }

    private readonly int MaxLevel = 999;

    private int _level;
    public int Level {
        get {
            return _level;
        }
        set {
            _level = Mathf.Clamp(value,0,MaxLevel);
            levelText.text = _level.ToString();
        }
    }

    private double maxExperience;
    private double experienceIncrementMin;
    private double experienceIncrementMax;

    private double _experience;
    public double Experience {
        get {
            return _experience;
        }
        set {
            _experience = Math.Clamp(value,0.0,(_level < MaxLevel) ? maxExperience : 0.0);
            expierienceSlider.value = (float)(_experience / maxExperience);
            levelUpButton.gameObject.SetActive(Math.Abs(_experience - maxExperience) < 0.0001);
        }
    }

    private ulong _automaticStoneGain;
    public ulong AutomaticStoneGain {
        get {
            return _automaticStoneGain;
        }
        set {
            _automaticStoneGain = value;
            workerInfo.SetActive(_automaticStoneGain != 0);
            workerGainText.text = $"{NumberFormat.ShortForm(_automaticStoneGain)} / s";
        }
    }

    private void Awake() {
        StoneIncrement = 1;
        mainButton.onClick.AddListener(OnMainButtonClick);
        levelUpButton.onClick.AddListener(OnLevelUpButtonClick);
        Level = 0;
        maxExperience = 30.0;
        Experience = 0.0f;
        experienceIncrementMin = 1.0;
        experienceIncrementMax = 3.5;
        AutomaticStoneGain = 0;
        equipmentMenu.Stone.Count = 0;
        equipmentMenu.Money.Count = 0;
    }

    private void Start() {
        equipmentMenu.Stone.OnCountChanged += OnStoneCountChanged;
        equipmentMenu.Money.OnCountChanged += OnMoneyCountChanged;
    }

    private void OnStoneCountChanged(Rational value) {
        stoneCountText.text = NumberFormat.ShortForm(value);
    }

    private void OnMoneyCountChanged(Rational value) {
        moneyCountText.text = NumberFormat.ShortForm(value);
    }

    private float automaticStoneGainTimer = 0.0f;
    private void Update() {
        if(AutomaticStoneGain > 0) {
            automaticStoneGainTimer += Time.deltaTime;
            while(automaticStoneGainTimer >= 1.0f) {
                equipmentMenu.Stone.Count += AutomaticStoneGain;
                automaticStoneGainTimer -= 1.0f;
            }
        }
    }

    private void OnMainButtonClick() {
        float chance = UnityEngine.Random.Range(1.01f,10.00f);
        if(chance >= 1.01f && chance <= 1.02f) {
            equipmentMenu.Money.Count += 1;
        }

        bool noOreDropped = true;
        foreach(var resourceInstance in equipmentMenu.ResourceInstances.Values) {
            if(chance >= resourceInstance.MinDropChance && chance <= resourceInstance.MaxDropChance) {
                resourceInstance.Count += 1;
                noOreDropped = false;
                break;
            }
        }

        if(noOreDropped && chance >= 2.01f && chance <= 10.0f) {
            var random = new System.Random();
            var increment = StoneIncrement;
            if(increment > 10) {
                increment += (ulong)random.Next(-1,1) * (StoneIncrement / 3);
            }
            equipmentMenu.Stone.Count += increment;
            Experience += new System.Random().NextDouble(experienceIncrementMin,experienceIncrementMax);
        }
    }

    private void OnLevelUpButtonClick() {
        Level += 1;
        maxExperience = Level * 20.0 + new System.Random().NextDouble(30.0,60.0);
        Experience = 0.0;
    }
}
