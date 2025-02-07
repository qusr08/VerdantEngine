using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUIObject : MonoBehaviour
{
    public Slider healthSlider;
    public Enemy enemy; 
    public Image image;
    // Start is called before the first frame update
  
    public void SetUp(Enemy enemy)
    {
        this.enemy = enemy;
        image.sprite = enemy.icon;
        healthSlider.value = 1;
    }
    public void HealthUpdate()
    {
        healthSlider.value = (enemy.MaxHealth / enemy.health);
        
    }
    public void Kill()
    {
        image.color = Color.gray;
        healthSlider.value = 0;
    }

}
