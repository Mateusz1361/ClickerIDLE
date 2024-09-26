using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MonsterDrop {
    public string name;
    public ulong count;
}

public class Monster {
    public string name;
    public Sprite image;
    public double maxHealth;
    public MonsterDrop[] drops;
    public double hitPoints;
}

public class MonsterManagement : MonoBehaviour {
    [SerializeField]
    private ReferenceHub referenceHub;
    [SerializeField]
    private TextAsset monsterDataTextAsset;
    [SerializeField]
    private GameObject monsterObject;
    [SerializeField]
    private Image monsterImage;
    [SerializeField]
    private Button monsterAttackButton;
    [SerializeField]
    private TMP_Text monsterName;
    [SerializeField]
    private Slider monsterHealthBar;
    [SerializeField]
    private TMP_Text monsterHealthText;
    [SerializeField]
    private Slider playerHealthBar;
    [SerializeField]
    private TMP_Text playerHealthText;
    private double currentMonsterHealth = 0.0;
    private double maxPlayerHealth = 10.0;

    public Monster CurrentMonster { get; private set; } = null;

    private double monsterAppearTime = 5.0;
    private double monsterAppearTimer = 0.0;
    private double monsterAttackTime = 1.0;
    private double monsterAttackTimer = 0.0;
    
    private void Awake() {
        monsterAttackButton.onClick.AddListener(PlayerAttack);
        monsterObject.SetActive(false);
        PlayerHp = maxPlayerHealth;
        playerHealthBar.value = 1.0f;
        playerHealthText.text = $"{PlayerHp}/{maxPlayerHealth}";
    }

    private void Update() {
        if(CurrentMonster != null) {
            monsterAttackTimer += Time.deltaTime;
            if(monsterAttackTimer >= monsterAttackTime) {
                monsterAttackTimer = 0.0;
                MonsterAttack();
            }
            return;
        }
        monsterAppearTimer += Time.deltaTime;

        if(monsterAppearTimer >= monsterAppearTime) {
            monsterAppearTimer = 0.0;
            CurrentMonster = Monsters[UnityEngine.Random.Range(0,Monsters.Length)];
            monsterObject.SetActive(true);
            monsterName.text = CurrentMonster.name;
            monsterImage.sprite = CurrentMonster.image;
            currentMonsterHealth = CurrentMonster.maxHealth;
            monsterHealthBar.value = 1.0f;
            monsterHealthText.text = $"{currentMonsterHealth}/{CurrentMonster.maxHealth}";   
        }
    }

    public void PlayerAttack() {
        currentMonsterHealth -= referenceHub.inventoryMenu.GetDamage();
        if(currentMonsterHealth > 0.0) {
            monsterHealthBar.value = (float)(currentMonsterHealth / CurrentMonster.maxHealth);
            monsterHealthText.text = $"{currentMonsterHealth}/{CurrentMonster.maxHealth}";
        }
        else {
            var drop = CurrentMonster.drops[UnityEngine.Random.Range(0,CurrentMonster.drops.Length)];
            referenceHub.inventoryMenu.AddItems(drop.name,drop.count);
            CurrentMonster = null;
            monsterObject.SetActive(false);
            PlayerHp = maxPlayerHealth;
            playerHealthBar.value = (float)(PlayerHp / maxPlayerHealth);
            playerHealthText.text = $"{PlayerHp}/{maxPlayerHealth}";
        }
    }

    public void MonsterAttack() {
        if(currentMonsterHealth > 0.0) {
            PlayerHp -= Math.Clamp(CurrentMonster.hitPoints - referenceHub.inventoryMenu.GetDamageReduction(),0,double.MaxValue);
            playerHealthBar.value = (float)(PlayerHp / maxPlayerHealth);
            playerHealthText.text = $"{PlayerHp}/{maxPlayerHealth}";
        }

        if(PlayerHp <= 0) {
            PlayerHp = maxPlayerHealth;
            playerHealthBar.value = (float)(PlayerHp / maxPlayerHealth);
            playerHealthText.text = $"{PlayerHp}/{maxPlayerHealth}";
            Debug.LogWarning("DEAD");
            CurrentMonster = null;
            monsterAppearTimer = 0.0;
            monsterObject.SetActive(false);
        }
    }

    private Monster[] _monsters = null;
    public Monster[] Monsters {
        get {
            if(_monsters == null) {
                InitMonsters();
            }
            return _monsters;
        }
    }    

    public void InitMonsters() {
        var monsterDatas = JsonUtility.FromJson<InstanceWrapperDataJson<MonsterDataJson>>(monsterDataTextAsset.text);
        _monsters = new Monster[monsterDatas.data.Length];
        for(int i = 0;i < monsterDatas.data.Length;i += 1) {
            var monsterData = monsterDatas.data[i];
            _monsters[i] = new() {
                name = monsterData.name,
                image = Resources.Load<Sprite>("Images/" + monsterData.imagePath),
                maxHealth = monsterData.maxHealth,
                drops = new MonsterDrop[monsterData.drops.Length],
                hitPoints = monsterData.hitPoints
            };
            for(int j = 0;j < monsterData.drops.Length;j += 1) {
                var monsterDrop = monsterData.drops[j];
                _monsters[i].drops[j] = new() {
                    name = monsterDrop.name,
                    count = monsterDrop.count
                };
            }
        }
    }

    private double _playerHp;
    public double PlayerHp {
        get {
            return _playerHp;
        }
        set {
            _playerHp = value;
        }
    }
}
