using TMPro;
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

    private double currentMonsterHealth = 0.0;

    public Monster CurrentMonster { get; private set; } = null;

    private double monsterAppearTime = 5.0;
    private double monsterAppearTimer = 0.0;

    private void Awake() {
        monsterAttackButton.onClick.AddListener(Attack);
        monsterObject.SetActive(false);
    }

    private void Update() {
        if(CurrentMonster != null) return;
        monsterAppearTimer += Time.deltaTime;
        if(monsterAppearTimer >= monsterAppearTime) {
            monsterAppearTimer = 0.0;
            CurrentMonster = Monsters[Random.Range(0,Monsters.Length)];
            monsterObject.SetActive(true);
            monsterName.text = CurrentMonster.name;
            monsterImage.sprite = CurrentMonster.image;
            currentMonsterHealth = CurrentMonster.maxHealth;
            monsterHealthBar.value = 1.0f;
            monsterHealthText.text = $"{currentMonsterHealth}/{CurrentMonster.maxHealth}";
        }
    }

    public void Attack() {
        currentMonsterHealth -= 1.0;
        if(currentMonsterHealth > 0.0) {
            monsterHealthBar.value = (float)(currentMonsterHealth / CurrentMonster.maxHealth);
            monsterHealthText.text = $"{currentMonsterHealth}/{CurrentMonster.maxHealth}";
        }
        else {
            var drop = CurrentMonster.drops[Random.Range(0,CurrentMonster.drops.Length)];
            referenceHub.inventoryMenu.AddItems(drop.name,drop.count);
            CurrentMonster = null;
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
        var monsterDatas = JsonUtility.FromJson<InstanceWrapperData<MonsterData>>(monsterDataTextAsset.text);
        _monsters = new Monster[monsterDatas.data.Length];
        for(int i = 0;i < monsterDatas.data.Length;i += 1) {
            var monsterData = monsterDatas.data[i];
            _monsters[i] = new() {
                name = monsterData.name,
                image = Resources.Load<Sprite>("Images/" + monsterData.imagePath),
                maxHealth = monsterData.maxHealth,
                drops = new MonsterDrop[monsterData.drops.Length]
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
}
