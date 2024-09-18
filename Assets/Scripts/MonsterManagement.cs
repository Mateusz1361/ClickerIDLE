using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MonsterManagement : MonoBehaviour
{
    [SerializeField] 
    private GameObject monsterIcon;
    [SerializeField]
    private TMP_Text monsterName;
    [SerializeField]
    private Slider healthBar;
    [SerializeField] private double timeToAttack = 5.0;
    [SerializeField] private Button attackButton;
    [SerializeField] private TMP_Text hpOfMonsterText;
    private float healthOfMonster = 5; 
    private double attackTimer = 0.0;
    private float maxHealthOfMonster = 5;

    private void Update()
    {
        if (!monsterIcon.activeSelf)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= timeToAttack)
            {
                monsterIcon.SetActive(true);
                healthOfMonster = maxHealthOfMonster;
                hpOfMonsterText.text = $"{healthOfMonster}/{maxHealthOfMonster} Hp";
                healthBar.value = 1;
            }
        }
        
    }

    private void Awake()
    {
        attackButton.onClick.AddListener(Attack);
        monsterIcon.SetActive(false);
    }

    private void Attack()
    {
        healthOfMonster -= 1;
        healthBar.value = healthOfMonster/maxHealthOfMonster;
        hpOfMonsterText.text = $"{healthOfMonster}/{maxHealthOfMonster} Hp";
        if (healthOfMonster <= 0)
        {
            monsterIcon.SetActive(false);
            attackTimer = 0;
        }
        
    }


}
