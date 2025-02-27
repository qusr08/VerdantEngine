using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[SerializeField] private PlayerCombatManager playerCombatManager;
	[SerializeField] private CombatUIManager combatUIManager;
	[SerializeField] private PlayerDataManager playerDataManager;
	[SerializeField] public CombatManager combatManager;
	[SerializeField] private GameObject arrowObject;
	[SerializeField] public int EnemyID;
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _currentHealth;
	[SerializeField] private int _currentCooldown;
	[SerializeField] private Sprite _icon;
	[SerializeField] private List<GardenTile> _finalAim = new List<GardenTile>( );
	[SerializeField] private List<EnemyAttackSO> attacks;

	public List<GardenTile> FinalAim { get => _finalAim; private set => _finalAim = value; }

	/// <summary>
	/// The sprite icon of this enemy
	/// </summary>
	public Sprite Icon => _icon;

	/// <summary>
	/// The current health of this enemy
	/// </summary>
	public int CurrentHealth {
		get => _currentHealth;
		private set {
			_currentHealth = value;
			combatUIManager.UpdateHealth(this);

			if (_currentHealth <= 0) {
				UnmarkGardenTiles( );
				combatManager.KillEnemy(this);
			}
		}
	}

	/// <summary>
	/// The max health of this enemy
	/// </summary>
	public int MaxHealth { get => _maxHealth; private set => _maxHealth = value; }

	/// <summary>
	/// The current attack of this enemy
	/// </summary>
	public EnemyAttackSO CurrentAttack { get => _currentAttack; private set => _currentAttack = value; }
	private EnemyAttackSO _currentAttack;

	/// <summary>
	/// The current cooldown of this enemy
	/// </summary>
	public int CurrentCooldown { 
		get => _currentCooldown;
		private set {
			_currentCooldown = value;

			// Just as a note, if you mess with the order of the below if statements, enemies do not properly update their attacked tiles
			// I am not sure why, just wanted to write this for the future - Frankie

			if (attacks.Count > 0 && needNewAttack) {
				CurrentAttack = attacks[0];
				_currentCooldown = CurrentAttack.MaxCooldown;
				needNewAttack = false;
			}

			if (_currentCooldown == 0) {
				MarkMapBeforeAttack( );
				needNewAttack = true;
			}
		}
	}

	// private bool attacksAreRandom;
	private bool needNewAttack = true;
	private int randomAim;
	private bool isInitialized = false;
	private bool randomAttackDirection = true;
	private List<GardenTile> currentAim = new List<GardenTile>( );

	private void Awake ( ) {
		playerCombatManager = FindObjectOfType<PlayerCombatManager>( );
		playerDataManager = FindObjectOfType<PlayerDataManager>( );
		combatManager = FindObjectOfType<CombatManager>( );
		combatUIManager = FindObjectOfType<CombatUIManager>( );
	}

	void Start ( ) {
		MaxHealth = CurrentHealth;

		if (attacks.Count > 0) {
			CurrentAttack = attacks[0];
		}
	}

	public void Attacked (PlayerAttackSO attack) {
		int totalDamage = attack.Damage + playerCombatManager.GetAddedDamage( );
		CurrentHealth -= totalDamage;
		Debug.Log($"Ouch, I just took {totalDamage}. Now I have {CurrentHealth} health");
	}

	public void StartRound ( ) {
		isInitialized = false;
		CurrentCooldown--;
		combatUIManager.UpdateCooldown(this);
	}

	public void MarkMapBeforeAttack ( ) {
		if (CurrentCooldown > 0) {
			return;
		}

		FinalAim.Clear( );
		currentAim.Clear( );
		if (CurrentAttack == null) {
			CurrentAttack = attacks[0];
		}

		// Clear previous markings
		//   UnmarkTiles();

		if (!isInitialized) {
			randomAim = UnityEngine.Random.Range(0, playerDataManager.GardenSize);
			randomAttackDirection = (UnityEngine.Random.value <= 0.5f);
			isInitialized = true;
		}

		if (CurrentAttack.IsLineAttackHorizontal) {
			if (randomAttackDirection) {
				for (int i = playerDataManager.GardenSize - 1; i >= 0; i--) {
					GardenTile tile = playerDataManager.Garden[randomAim, i];

					if (tile != null) {
						currentAim.Add(tile);
					}
				}
			} 
			else {
				for (int i = 0; i < playerDataManager.GardenSize; i++) {
					GardenTile tile = playerDataManager.Garden[randomAim, i];
					if (tile != null) {
						currentAim.Add(tile);
					}
				}
			}
		} else {
			for (int i = playerDataManager.GardenSize - 1; i >= 0; i--) {
				GardenTile tile = playerDataManager.Garden[i, randomAim];
				if (tile != null) {
					currentAim.Add(tile);
				}
			}
		}

		if (currentAim.Count == 0) {
			Debug.LogError("No valid tiles found for marking.");
			return;
		}

		// Set Icon for map
		if (arrowObject != null) {
			arrowObject.SetActive(true);
			arrowObject.transform.SetParent(currentAim[0].transform);

			// What are these rotations for? - Frankie
			if (!CurrentAttack.IsLineAttackHorizontal)
			{
					arrowObject.transform.localPosition = Vector3.zero + new Vector3(2, -1, -1);
					arrowObject.transform.rotation = new Quaternion(0.00228309655f, -0.707103133f, 0.707103133f, -0.00228309655f);
			}
			else if (randomAttackDirection)
			{
				//right
				arrowObject.transform.localPosition = Vector3.zero + new Vector3(0.75f, 0f, -1f);
				arrowObject.transform.localEulerAngles =  new Vector3(0, 180, 270);
			}
			else
			{
				//left
				arrowObject.transform.localPosition = Vector3.zero + new Vector3(1, -2, -1);
				arrowObject.transform.localEulerAngles = new Vector3(0, 0, 90);
			}

			}
		else {
			Debug.LogError("Icon holder is missing!");
		}

		// Set up the final aim for marking, stopping if there is a collision
		foreach (GardenTile tile in currentAim) {
			tile.IsAttacked = true;
			FinalAim.Add(tile);
			Debug.Log(gameObject.name + " is marking tile as attacked: " + tile.Position);
			if (tile.GardenPlaceable != null) {
				break;
			}
		}
	}

	public void UnmarkGardenTiles ( ) {
		Debug.LogWarning(gameObject.name + " Unmarked his targets");
		foreach (GardenTile gardenTile in FinalAim) {
			gardenTile.IsAttacked = false;
		}

		FinalAim.Clear( );
		currentAim.Clear( );
		arrowObject.SetActive(false);
	}
}
