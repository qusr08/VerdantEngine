using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComabtUI_Manager : MonoBehaviour
{
    public CombatManager combat;
    public GameObject enemyUIprefeb;
    public Transform healthUIperent;
    public List<EnemyHealthUIObject> enemyHealthUIObjects = new List<EnemyHealthUIObject>();

    public void AddEnemyHealth(Enemy enemy)
    {
        EnemyHealthUIObject uiPanel = Instantiate(enemyUIprefeb, healthUIperent).GetComponent<EnemyHealthUIObject>();
        uiPanel.SetUp( enemy);
        enemyHealthUIObjects.Add(uiPanel);
    }
    public void SetHealth(Enemy enemy)
    {
        foreach (EnemyHealthUIObject item in enemyHealthUIObjects)
        {
            if(item.enemy.enemyID == enemy.enemyID)
            {
                item.HealthUpdate();
                break;
            }
        }
    }

    public void SetCoolDown(Enemy enemy)
    {
        foreach (EnemyHealthUIObject item in enemyHealthUIObjects)
        {
            if (item.enemy.enemyID == enemy.enemyID)
            {
                item.UpdateCoolDown();
                break;
            }
        }
    }
    public void KillEnemy(Enemy enemy)
    {
        foreach (EnemyHealthUIObject item in enemyHealthUIObjects)
        {
            if (item.enemy == enemy)
            {
                item.Kill();
            }
        }
    }


}
