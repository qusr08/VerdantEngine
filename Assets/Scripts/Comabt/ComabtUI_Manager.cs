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

    public void setUp(CombatManager combatManager)
    {
        combat = combatManager;
        foreach (GameObject item in combat.currentComabt.enemies)
        {
            Enemy enemy = item.GetComponent<Enemy>();
            enemyHealthUIObjects.Add(Instantiate(enemyUIprefeb, healthUIperent).GetComponent<EnemyHealthUIObject>());

        }
    }
    public void SetHealth()
    {
        
    }
    public void KillEnemy()
    {

    }


}
