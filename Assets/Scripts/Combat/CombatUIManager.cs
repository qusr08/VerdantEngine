using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUIManager : MonoBehaviour {
	[SerializeField] private CombatManager combatManager;
	[SerializeField] private GameObject enemyHealthUIPrefab;
	[SerializeField] private Transform healthUIContainer;
	[SerializeField] private List<EnemyHealthUIObject> enemyHealthUIObjects = new List<EnemyHealthUIObject>( );

	public void AddEnemyHealth (Enemy enemy) {
		EnemyHealthUIObject enemyHealthUIObject = Instantiate(enemyHealthUIPrefab, healthUIContainer).GetComponent<EnemyHealthUIObject>( );
		enemyHealthUIObject.Enemy = enemy;
		enemyHealthUIObjects.Add(enemyHealthUIObject);
	}

	public void UpdateHealth (Enemy enemy) {
		foreach (EnemyHealthUIObject enemyHealthUIObject in enemyHealthUIObjects) {
			if (enemyHealthUIObject.Enemy.EnemyID == enemy.EnemyID) {
				enemyHealthUIObject.UpdateHealth( );
				break;
			}
		}
	}

	public void UpdateCooldown (Enemy enemy) {
		foreach (EnemyHealthUIObject enemyHealthUIObject in enemyHealthUIObjects) {
			if (enemyHealthUIObject.Enemy.EnemyID == enemy.EnemyID) {
				enemyHealthUIObject.UpdateCoolDown( );
				break;
			}
		}
	}

	public void KillEnemy (Enemy enemy) {
		foreach (EnemyHealthUIObject enemyHealthUIObject in enemyHealthUIObjects) {
			if (enemyHealthUIObject.Enemy == enemy) {
				enemyHealthUIObject.Kill( );
			}
		}
	}

    public void ResetEnemyHealth()
    {
        foreach (var item in enemyHealthUIObjects)
        {
			Destroy(item.gameObject);
        }
		enemyHealthUIObjects = new List<EnemyHealthUIObject>();
	}
}
