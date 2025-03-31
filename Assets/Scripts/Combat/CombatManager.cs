using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class CombatManager : MonoBehaviour {
	[SerializeField] public PlayerCombatManager playerCombatManager;
	[SerializeField] public CombatUIManager combatUIManager;
	[SerializeField] private CombatPresetSO currentCombatPreset;
	[SerializeField] private PlayerDataManager playerDataManager;
	[SerializeField] private GardenManager gardenManager;
	[SerializeField] private MapPlayer cameraManager;
    [SerializeField] private GameObject winScreen;
	[SerializeField] private GameObject explosionPrefab;

	private List<Enemy> enemies;
	private bool isPlayerPaused = false;
	// private int maxTargets; // Number of enemies to select
	// private bool isTargeting = false;

	private void Awake ( ) {
		enemies = new List<Enemy>( );
	}

	private void Start ( ) {
		if(currentCombatPreset != null)
		{

			SetUpEnemies();

            playerCombatManager.PlayerStartTurn();
        }
		
	}

	public void NewCombat(CombatPresetSO newCombat)
	{
		if (enemies.Count > 0 || currentCombatPreset != null)
		{
			Debug.Log("New Combat Failed");
			return;
		}
		combatUIManager.GameState = GameState.COMBAT;
		playerDataManager.CurrentActions = playerDataManager.MaxActions;
		combatUIManager.PurgeList();
		currentCombatPreset = newCombat;
		SetUpEnemies();
		playerCombatManager.PlayerStartTurn();
		isPlayerPaused = false;



	}

    /// <summary>
    /// Set Up all starting enemies in an encounter
    /// </summary>
    private void SetUpEnemies ( ) {

		if(currentCombatPreset == null)
		{
			return;
		}
		combatUIManager.GameState = GameState.COMBAT;

		// Spawn enemy prefabs and place them in the backline or front line
		// Currently the game is limited to 3 enemies each
		for (int i = 0; i < currentCombatPreset.EnemyPrefabs.Count; i++) {
			enemies.Add(Instantiate(currentCombatPreset.EnemyPrefabs[i]).GetComponent<Enemy>( ));
			enemies[i].combatManager = this;
			enemies[i].EnemyID = i;
			enemies[i].gameObject.name = "Enemy " + i;
			combatUIManager.AddEnemyHealth(enemies[i]);
			combatUIManager.UpdateHealth(enemies[i]);
		}

		AllEnemiesStartRound( );
	}

	public void AllEnemiesStartRound ( ) {
		foreach (Enemy enemy in enemies) {
			enemy.StartRound( );
		}
	}

	public IEnumerator IUpdateTargetedEnemies (PlayerAttackSO mechPart) {
		// Get the targeted enemies based on the mech part
		List<Enemy> targetEnemies = GetTargets(mechPart);
		
		// Deselect all enemies
		foreach (Enemy enemy in enemies) {
			enemy.GetComponent<SpriteRenderer>( ).color = Color.white;
		}

		// Select targeted enemies
		foreach (Enemy enemy in targetEnemies) {
			enemy.GetComponent<SpriteRenderer>( ).color = Color.red;
			enemy.Attacked(mechPart);
		}

		yield return new WaitForSeconds(1);

		// current_Attack = part;
		//attack_CostBox.text = current_Attack.manaCost.ToString();
		//attack_TextBox.text = current_Attack.attackText.ToString();
		//isTrageting = false;
	}

	//Used to damage the first enemy
    public void damageEnemy(int damage)
    {
		if (enemies.Count == 0)
			return;
        // Get the targeted enemies based on the mech part
        Enemy targetEnemies = enemies[0];

        // Deselect all enemies
        foreach (Enemy enemy in enemies)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Select targeted enemies
        targetEnemies.GetComponent<SpriteRenderer>().color = Color.red;
        targetEnemies.Attacked(damage);
       
    }

    /// <summary>
    /// Get a list of targeted enemies based on the mech part
    /// </summary>
    /// <param name="mechPart">The mech part to calculate the targeted enemies of</param>
    /// <returns>A list of all the targeted enemies</returns>
    public List<Enemy> GetTargets (PlayerAttackSO mechPart) {
		List<Enemy> targetEnemies = new List<Enemy>( );

		switch (mechPart.PlayerTargetingType) {
			case PlayerTargetingType.TARGET:
				for (int i = 0; i < mechPart.TargetNumber; i++) {
					if (enemies.Count > i) {
						targetEnemies.Add(enemies[i]);
					}
				}

				break;
			case PlayerTargetingType.ALL:
				foreach (Enemy item in enemies) {
					targetEnemies.Add(item);
				}

				break;
		}

		return targetEnemies;
	}

	/// <summary>
	/// Update the garden tiles based on where the enemies are going to attack
	/// </summary>
	public void UpdateEnemyAttackVisuals ( ) {
		// Reset all of the garden tiles to not be attacked
		foreach (GardenTile gardenTile in playerDataManager.Garden) {
			gardenTile.IsAttacked = false;
		}

		// Select only the garden tiles that the enemy is going to attack
		foreach (Enemy enemy in enemies) {
			enemy.MarkMapBeforeAttack( );
		}
	}

	public IEnumerator EnemyTurn ( ) {
		foreach (Enemy enemy in enemies)
		{
			if (enemy.CurrentCooldown == 0)
			{
				StartCoroutine( playerCombatManager.ApplyDamageToGarden(enemy, enemy.CurrentAttack));
			}
		}
		yield return new WaitForSeconds((float)playerCombatManager.enemyAttckSliderAnimation.director.duration);
		foreach (GardenTile gardenTile in playerDataManager.Garden)
		{
			if (gardenTile.IsAttacked)
			{
				Instantiate(explosionPrefab, gardenTile.gameObject.transform.position, Quaternion.identity);
			}

			gardenTile.IsAttacked = false;
		}
		isPlayerPaused = false;

		playerCombatManager.PlayerStartTurn();
		AllEnemiesStartRound();
	}

	/// <summary>
	/// Called at the end of the player's turn
	/// </summary>
	public void EndPlayerTurn ( ) {
		if (!isPlayerPaused) {
			isPlayerPaused = true;

			// Reset the current actions for the player
			playerDataManager.CurrentActions = playerDataManager.MaxActions;

			StartCoroutine( playerCombatManager.PlayerTurn( ));
		}
	}

	/// <summary>
	/// Kill an enemy
	/// </summary>
	/// <param name="enemy">The enemy to kill</param>
	public void KillEnemy (Enemy enemy) {
		for (int i = enemies.Count - 1; i >= 0; i--) {
			if (enemies[i] == enemy) {
				combatUIManager.KillEnemy(enemy);
				enemies.Remove(enemy);
				Destroy(enemy.gameObject);
			}
		}

		if (enemies.Count == 0) {
			combatUIManager.PurgeList();
			WinGame();
		}
	}

	/// <summary>
	/// What happens when there are no enemies left
	/// </summary>
	public void Win()
	{
		currentCombatPreset = null;
		cameraManager.scene = ActiveScene.Map;
		cameraManager.UpdateCameraPosition();

	}
	private void WinGame()
	{
		playerDataManager.Money += currentCombatPreset.rewardMoeny;

		winScreen.GetComponent<RewardManager>().moneyReward = currentCombatPreset.rewardMoeny;
        winScreen.SetActive(true);

		// NOTE: This should be replaced with end-screen rewards
		// Once the reward is chosen, then do the below code

		// Reset the player actions so the player can update their board in between combats
		// Also set the game state from COMBAT back to IDLE
		playerDataManager.CurrentActions = playerDataManager.MaxActions;

	}
	///                ///
	/// Code Graveyard ///
	///                ///
	/*   public void SelectEnemy(Enemy enemy)
       {
           if(selectedEnemis.Count>=maxTargets)
           {
              selectedEnemis.Dequeue().GetComponent<SpriteRenderer>().color = Color.white;
           }
           selectedEnemis.Enqueue(enemy);
           enemy.GetComponent<SpriteRenderer>().color = Color.red;
       }

       //Used for selecting enemies, should probably find a better way of doing this
       void MouseClick()
       {
           Debug.Log("click");
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;

           if (Physics.Raycast(ray, out hit))
           {
               if (hit.collider != null && hit.collider.GetComponent<Enemy>() != null)
               {
                   Debug.Log("Right-clicked on: " + hit.collider.gameObject.name);
                   Enemy enemy = hit.collider.GetComponent<Enemy>();
                   SelectEnemy(enemy);
               }
           }
       }

       */

}
