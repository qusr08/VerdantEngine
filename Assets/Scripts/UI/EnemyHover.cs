using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHover : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyNameText;
    [SerializeField] private TMP_Text enemyDescriptionText;
    [SerializeField] private TMP_Text enemyHealthText;

    [SerializeField] private Image enemyImage;
    [SerializeField] private Slider HealthSlider;

    [SerializeField] private TMP_Text enemy_Damage;
    [SerializeField] private TMP_Text enemy_CoolDown;

    public void UpdateText(Enemy enemy)
    {
        enemyNameText.text = enemy.enemyName;
        enemyDescriptionText.text = enemy.CurrentAttack.info;
        enemyImage.sprite = enemy.Icon;
        enemyHealthText.text = enemy.CurrentHealth + "/" + enemy.MaxHealth;
        HealthSlider.maxValue = enemy.MaxHealth;
        HealthSlider.value = enemy.CurrentHealth;
        enemy_Damage.text = enemy.CurrentAttack.Damage.ToString();
        enemy_CoolDown.text = enemy.CurrentCooldown.ToString();

    }
}
