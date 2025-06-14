using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using static UnityEngine.EventSystems.EventTrigger;

public class CombatManager : MonoBehaviour {
	[SerializeField] public PlayerCombatManager playerCombatManager;
	[SerializeField] public CombatUIManager combatUIManager;
	[SerializeField] private CombatPresetSO currentCombatPreset;
	[SerializeField] private PlayerDataManager playerDataManager;
	[SerializeField] private GardenManager gardenManager;
	[SerializeField] private MapPlayer cameraManager;
    [SerializeField] private GameObject winScreen;
	[SerializeField] private GameObject explosionPrefab;
	public float endOfTurnWaitTime;
	public List<Enemy> Enemies { get; private set; }
    public List<Enemy> EnemiesSavedData { get; private set; }

    private bool isPlayerPaused = false;
	public BG_Music_Manager soundManager;
	// private int maxTargets; // Number of enemies to select
	// private bool isTargeting = false;

	public Material greenSky;
    public Material redSky;
    public Animator enemyAnimator;

	//Used to fix error on first round no showing the arrow damage indicators
	private bool IsTurn1;

    //Need to be removed after edge
    private bool isBoss = false;
	public GameObject ThanksForPlayingScreen;
	public Inventory inventory; 


    private void Awake ( ) {
		Enemies = new List<Enemy>( );
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
		if (Enemies.Count > 0 || currentCombatPreset != null)
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
        soundManager.Playcomabt();
		IsTurn1 = true;
        RenderSettings.skybox = redSky ;
        DynamicGI.UpdateEnvironment();

			isBoss = newCombat.isBoss;
		SaveGameState();
    }

    /// <summary>
    /// Set Up all starting enemies in an encounter
    /// </summary>
    private void SetUpEnemies ( ) {
		playerCombatManager.ResetEnergy();

        if (currentCombatPreset == null)
		{
			return;
		}
		combatUIManager.GameState = GameState.COMBAT;

		// Spawn enemy prefabs and place them in the backline or front line
		// Currently the game is limited to 3 enemies each
		for (int i = 0; i < currentCombatPreset.EnemyPrefabs.Count; i++) {
			Enemies.Add(Instantiate(currentCombatPreset.EnemyPrefabs[i]).GetComponent<Enemy>( ));
           
            Enemies[i].combatManager = this;
			Enemies[i].EnemyID = i;
			Enemies[i].gameObject.name = "Enemy " + i;
			combatUIManager.AddEnemyHealth(Enemies[i]);
			combatUIManager.UpdateHealth(Enemies[i]);


      
        }
		playerCombatManager.ResetWeapons();

        AllEnemiesStartRound( );
        SaveGameState();

    }

    public void AllEnemiesStartRound()
	{

		foreach (GardenTile gardenTile in playerDataManager.Garden)
		{
			gardenTile.IsHighlighted = false;

		}

		foreach (Enemy enemy in Enemies)
		{
			enemy.StartRound();
		}
	}



    public IEnumerator IUpdateTargetedEnemies (PlayerAttackSO mechPart) {
		// Get the targeted enemies based on the mech part
		List<Enemy> targetEnemies = GetTargets(mechPart);
		
		// Deselect all enemies
		foreach (Enemy enemy in Enemies) {
			enemy.GetComponent<SpriteRenderer>( ).color = Color.white;
		}

		// Select targeted enemies
		foreach (Enemy enemy in targetEnemies) {
			enemy.GetComponent<SpriteRenderer>( ).color = Color.red;
			enemy.Attacked(mechPart);
		}

		yield return new WaitForSeconds(endOfTurnWaitTime);

		// current_Attack = part;
		//attack_CostBox.text = current_Attack.manaCost.ToString();
		//attack_TextBox.text = current_Attack.attackText.ToString();
		//isTrageting = false;
	}

	//Used to damage the first enemy
    public void damageEnemy(int damage)
    {
		if (Enemies.Count == 0)
			return;
        // Get the targeted enemies based on the mech part
        Enemy targetEnemies = Enemies[0];

        // Deselect all enemies
        foreach (Enemy enemy in Enemies)
        {
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Select targeted enemies
        targetEnemies.GetComponent<SpriteRenderer>().color = Color.red;
        targetEnemies.Attacked(damage);
       
    }

	public void damageAllEnemies(int damage)
    {
		if (Enemies.Count == 0)
			return;
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
			Enemies[i].GetComponent<SpriteRenderer>().color = Color.red;
            Enemies[i].Attacked(damage);
        }

      
        
       
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
					if (Enemies.Count > i) {
						targetEnemies.Add(Enemies[i]);
					}
				}

				break;
			case PlayerTargetingType.ALL:
				foreach (Enemy item in Enemies) {
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
			gardenTile.AttackedDamage = 0;
            gardenTile.IsHighlighted = false;
			gardenTile.indicatorManager.StopIndicator();

        }

        // Select only the garden tiles that the enemy is going to attack
        foreach (Enemy enemy in Enemies) {
			enemy.MarkMapBeforeAttack( );
		}
	}

	public IEnumerator EnemyTurn ( ) {
        enemyAnimator.SetTrigger("Attack");

        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
			if (i > Enemies.Count-1)
				continue;
            if (Enemies[i].CurrentCooldown == 0)
            {
				foreach (GardenTile gardenTile in Enemies[i].FinalAim)

                {
					gardenTile.indicatorManager.StopIndicator();
                    
                    AnimationManager.Instance.AddExplosionToList(gardenTile.transform);
					gardenTile.ClearColorForAnimation();
                }
                yield return StartCoroutine( AnimationManager.Instance.TriggerExplosions());

                StartCoroutine(playerCombatManager.ApplyDamageToGarden(Enemies[i], Enemies[i].CurrentAttack));
            }
        }

        //	yield return new WaitForSeconds((float)playerCombatManager.enemyAttckSliderAnimation.director.duration);
        foreach (GardenTile gardenTile in playerDataManager.Garden)
        {


            gardenTile.AttackedDamage = 0;
        }

        yield return new WaitForSeconds(0.2f);
		isPlayerPaused = false;

		playerCombatManager.PlayerStartTurn();
		AllEnemiesStartRound();
        SaveGameState();

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
		for (int i = Enemies.Count - 1; i >= 0; i--) {
			if (Enemies[i] == enemy) {
				combatUIManager.KillEnemy(enemy);
				Enemies.Remove(enemy);
				Destroy(enemy.gameObject);
			}
		}

		if (Enemies.Count == 0) {
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
        combatUIManager.GameState = GameState.IDLE;
        cameraManager.scene = ActiveScene.Map;
		cameraManager.UpdateCameraPosition();
		soundManager.PlayGarden();
        RenderSettings.skybox = greenSky;
        DynamicGI.UpdateEnvironment();
		

    }
    private void WinGame()
    {
        if (isBoss)
        {
            //ThanksForPlayingScreen.SetActive(true);
            cameraManager.scene = ActiveScene.Win;
            cameraManager.UpdateCameraPosition();
            return;
        }

        playerDataManager.Money += currentCombatPreset.rewardMoeny;

		cameraManager.HideTutorial();
        winScreen.GetComponent<RewardManager>().moneyReward = currentCombatPreset.rewardMoeny;
		StartCoroutine(WinDelay());
        // NOTE: This should be replaced with end-screen rewards
        // Once the reward is chosen, then do the below code

        // Reset the player actions so the player can update their board in between combats
        // Also set the game state from COMBAT back to IDLE
        playerDataManager.CurrentActions = playerDataManager.MaxActions;

    }

	public IEnumerator WinDelay()
	{
		yield return new WaitForSeconds(1);
        winScreen.SetActive(true);
        playerDataManager.CurrentActions = playerDataManager.MaxActions;
		combatUIManager.GameState=GameState.IDLE;
    }


    public SaveGameState_SO saveGameState;

    public void SaveGameState()
    {
		saveGameState.PlacedArtifacts.Clear();
		saveGameState.PlacedPlants.Clear();


		
		saveGameState.Health = playerDataManager.CurrentHealth;
		
		gardenManager.SaveGardenState();
		saveGameState.SavedGardenState = gardenManager.SavedGardenState;

		saveGameState.energy = playerCombatManager.energy;
		saveGameState.actions = playerDataManager.CurrentActions;
    }
	public void ResetTurn()
	{

      playerDataManager.CurrentHealth = saveGameState.Health ;
         playerCombatManager.energy = saveGameState.energy;
		playerCombatManager.UpdateEnergy(0);

        playerDataManager.CurrentActions = saveGameState.actions;

    

 
        //Need to add inventory Reset
        foreach (var item in saveGameState.PlacedPlants)
		{
			inventory.AddPlant(item);

        }
        foreach (var item in saveGameState.PlacedArtifacts)
        {
            inventory.AddArtifact(item);

        }
        // add cisual fix to action  reset
        saveGameState.PlacedArtifacts.Clear();
        saveGameState.PlacedPlants.Clear();

        gardenManager.LoadGardenState();
		UpdateEnemyAttackVisuals();
        gardenManager.SaveGardenState();

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
