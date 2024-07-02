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
    private TMP_Text dollarCountText;
    [SerializeField]
    private GameObject workerInfo;
    [SerializeField]
    private TMP_Text workerGainText;

    private ulong _stoneIncrement;
    public ulong StoneIncrement {
        get {
            return _stoneIncrement;
        }
        set {
            _stoneIncrement = value;
            stoneCountText.text = $"{_stoneCount} (+{_stoneIncrement})";
        }
    }

    private ulong _stoneCount;
    public ulong StoneCount {
        get {
            return _stoneCount;
        }
        set {
            _stoneCount = value;
            stoneCountText.text = $"{_stoneCount} (+{_stoneIncrement})";
        }
    }

    private ulong _dollarCount;
    public ulong DollarCount {
        get {
            return _dollarCount;
        }
        set {
            _dollarCount = value;
            dollarCountText.text = _dollarCount.ToString();
        }
    }

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
            workerGainText.text = $"{_automaticStoneGain} / s";
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
    }

    private float automaticStoneGainTimer = 0.0f;
    private void Update() {
        if(AutomaticStoneGain > 0) {
            automaticStoneGainTimer += Time.deltaTime;
            while(automaticStoneGainTimer >= 1.0f) {
                StoneCount += AutomaticStoneGain;
                automaticStoneGainTimer -= 1.0f;
            }
        }
    }

    private void OnMainButtonClick() {
        float chance = UnityEngine.Random.Range(1.01f,10.00f);
        if(chance >= 1.01f && chance <= 1.02f) {
            DollarCount += 1;
        }

        bool noOreDropped = true;
        foreach(var oreInstance in equipmentMenu.OreInstances.Values) {
            if(chance >= oreInstance.minDrop && chance <= oreInstance.maxDrop) {
                oreInstance.Count += 1;
                noOreDropped = false;
                break;
            }
        }

        if(noOreDropped && chance >= 2.01f && chance <= 10.0f) {
            StoneCount += StoneIncrement;
            Experience += new System.Random().NextDouble(experienceIncrementMin,experienceIncrementMax);
        }
    }

    private void OnLevelUpButtonClick() {
        Level += 1;
        maxExperience = Level * 20.0 + new System.Random().NextDouble(30.0,60.0);
        Experience = 0.0;
    }
}
