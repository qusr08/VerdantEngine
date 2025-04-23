using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[SerializeField] private PlayerCombatManager playerCombatManager;
	[SerializeField] private CombatUIManager combatUIManager;
	[SerializeField] private PlayerDataManager playerDataManager;
	[SerializeField] public CombatManager combatManager;
	[SerializeField] private GameObject arrowObject;
    public string enemyName;

    [SerializeField] public int EnemyID;
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _currentHealth;
	[SerializeField] private int _currentCooldown;
	[SerializeField] private Sprite _icon;
	[SerializeField] private List<GardenTile> _finalAim = new List<GardenTile>( );
	[SerializeField] private List<EnemyAttackSO> attacks;

    private AttackDirection currentAttackDiraction;
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


            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                combatUIManager.UpdateHealth(this);
                UnmarkGardenTiles();
                combatManager.KillEnemy(this);
            }
            else
            {
                combatUIManager.UpdateHealth(this);
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
	private int main_randomAim;
    private int secondery_randomAim;
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
		Debug.Log($"Ouch, I just took {totalDamage} from {attack.name}. Now I have {CurrentHealth} health");
	}
    public void AttackedByArtifact(int damage)
    {
        int totalDamage = damage;
        CurrentHealth -= totalDamage;
        Debug.Log($"Ouch, I just took {totalDamage} from an Artifact. Now I have {CurrentHealth} health");
    }
    public void Attacked(int totalDamage)
    {

		CurrentHealth -= totalDamage;
		Debug.Log($"Ouch, I just took {totalDamage} from a plant/artifact. Now I have {CurrentHealth} health");
	}

	public void StartRound ( ) {
		arrowObject.SetActive(false);
        UnMarkMapForIndictor();

        isInitialized = false;
		CurrentCooldown--;
		combatUIManager.UpdateCooldown(this);
	}

    public void MarkMapBeforeAttack()
    {
        if (CurrentCooldown > 0)
        {
            return;
        }

        FinalAim.Clear();
        currentAim.Clear();
        if (CurrentAttack == null)
        {
            CurrentAttack = attacks[0];
        }


        //// I need to add the width logic here, limiting the enemies from aiming at the edges in case the width is bigger then 1
        if (CurrentAttack.EnemyTargetingType == EnemyTargetingType.LINE)
        {
            LineAim();
        }
        //attack a square
        else if (CurrentAttack.EnemyTargetingType == EnemyTargetingType.SHAPE)
        {
            AreaAim();
            currentAttackDiraction = AttackDirection.Area;
        }

        if (currentAim.Count == 0)
        {
            Debug.LogError("No valid tiles found for marking.");
            return;
        }

        // Set up arrow icon if needed Icon for map
        if (CurrentAttack.EnemyTargetingType == EnemyTargetingType.LINE && arrowObject != null)
        {
            arrowObject.SetActive(true);
            arrowObject.transform.SetParent(currentAim[0].transform);

            // What are these rotations for? - Frankie
            if (!CurrentAttack.IsLineAttackHorizontal)
            {
                currentAttackDiraction = AttackDirection.Foward;

                arrowObject.transform.localPosition = Vector3.zero + new Vector3(2, -1, -1);
                arrowObject.transform.rotation = new Quaternion(0.00228309655f, -0.707103133f, 0.707103133f, -0.00228309655f);
            }
            else if (randomAttackDirection)
            {
                currentAttackDiraction = AttackDirection.Right;

                //right
                arrowObject.transform.localPosition = Vector3.zero + new Vector3(0.75f, 0f, -1f);
                arrowObject.transform.localEulerAngles = new Vector3(0, 180, 270);
            }
            else
            {
                currentAttackDiraction = AttackDirection.Left;

                //left
                arrowObject.transform.localPosition = Vector3.zero + new Vector3(1, -2, -1);
                arrowObject.transform.localEulerAngles = new Vector3(0, 0, 90);
            }

        }
        else if (arrowObject == null)
        {
            Debug.LogError("Icon holder is missing!");
        }
        else
        {
            arrowObject.SetActive(false);
        }

        // Set up the final aim for marking, stopping if there is a collision

        foreach (GardenTile tile in currentAim)
        {
            tile.AttackedDamage += CurrentAttack.Damage;
            FinalAim.Add(tile);
            Debug.Log(gameObject.name + " is marking tile as attacked: " + tile.Position);
            if (tile.GardenPlaceable != null && CurrentAttack.EnemyTargetingType == EnemyTargetingType.LINE)
            {
                break;
            }
        }
        MarkMapForIndictor();
    }

    public void UnmarkGardenTiles ( ) {
		Debug.LogWarning(gameObject.name + " Unmarked his targets");
		foreach (GardenTile gardenTile in FinalAim) {
			gardenTile.AttackedDamage = 0;
            gardenTile.indicatorManager.StopIndicator();

        }

		FinalAim.Clear( );
		currentAim.Clear( );
		arrowObject.SetActive(false);
	}

    private void LineAim()
    {
        if (!isInitialized)
        {
            isInitialized = true;
            randomAttackDirection = (UnityEngine.Random.value <= 0.5f);

            if (CurrentAttack.IsLineAttackHorizontal)
            {
                // Randomly choose a row with at least one GardenPlacable
                main_randomAim = GetRandomRowWithGardenPlacable();
            }
            else
            {
                // Randomly choose a column with no GardenPlacables
                main_randomAim = GetRandomColumnWithoutGardenPlacable();
            }
        }

        if (CurrentAttack.IsLineAttackHorizontal)
        {
            int start = randomAttackDirection ? playerDataManager.GardenSize - 1 : 0;
            int end = randomAttackDirection ? -1 : playerDataManager.GardenSize;
            int step = randomAttackDirection ? -1 : 1;

            for (int i = start; i != end; i += step)
            {
                GardenTile tile = playerDataManager.Garden[main_randomAim, i];
                if (tile != null)
                {
                    currentAim.Add(tile);
                }
            }
        }
        else
        {
            for (int i = playerDataManager.GardenSize - 1; i >= 0; i--)
            {
                GardenTile tile = playerDataManager.Garden[i, main_randomAim];
                if (tile != null)
                {
                    currentAim.Add(tile);
                }
            }
        }
    }

    private int GetRandomRowWithGardenPlacable()
    {
        List<int> validRows = new List<int>();
        for (int row = 0; row < playerDataManager.GardenSize; row++)
        {
            for (int col = 0; col < playerDataManager.GardenSize; col++)
            {
                if (playerDataManager.Garden[row, col] != null && playerDataManager.Garden[row, col].GardenPlaceable != null)
                {
                    validRows.Add(row);
                    break;
                }
                else
                {
                    int randomNumber = UnityEngine.Random.Range(1, 6); // Generates a number from 1 to 5
                    if (randomNumber == 5)
                    {
                        validRows.Add(col);
                    }
                }

            }
        }
        return validRows.Count > 0 ? validRows[UnityEngine.Random.Range(0, validRows.Count)] : UnityEngine.Random.Range(0, playerDataManager.GardenSize);
    }

    private int GetRandomColumnWithoutGardenPlacable()
    {
        List<int> validColumns = new List<int>();
        for (int col = 0; col < playerDataManager.GardenSize; col++)
        {
            bool hasPlacable = false;
            for (int row = 0; row < playerDataManager.GardenSize; row++)
            {
                if (playerDataManager.Garden[row, col] != null && playerDataManager.Garden[row, col].GardenPlaceable!=null)
                {
                    hasPlacable = true;
                    break;
                }
              
            }
            if (!hasPlacable)
            {
                validColumns.Add(col);
            }
            else
            {
                int randomNumber = UnityEngine.Random.Range(1, 6); // Generates a number from 1 to 5
                if (randomNumber == 5)
                {
                    validColumns.Add(col);
                }
            }
        }
        return validColumns.Count > 0 ? validColumns[UnityEngine.Random.Range(0, validColumns.Count)] : UnityEngine.Random.Range(0, playerDataManager.GardenSize);
    }


    private void AreaAim()
    {
        //choose start point
        int range = CurrentAttack.AttackWidth - 1;
        if (!isInitialized)
        {
            main_randomAim = UnityEngine.Random.Range(0, playerDataManager.GardenSize - range);
            secondery_randomAim = UnityEngine.Random.Range(0, playerDataManager.GardenSize - range);
            randomAttackDirection = (UnityEngine.Random.value <= 0.5f);
            isInitialized = true;
        }
        for (int x = 0; x < CurrentAttack.AttackWidth; x++)
        {
            for (int y = 0; y < CurrentAttack.AttackWidth; y++)
            {
                GardenTile tile = playerDataManager.Garden[main_randomAim + x, secondery_randomAim + y];
                if (tile != null)
                {
                    currentAim.Add(tile);

                }

            }

        }
    }

    void MarkMapForIndictor()
    {
        foreach (GardenTile tile in FinalAim)
        {
            tile.indicatorManager.StartIndicator(currentAttackDiraction);
        }
    }
    void UnMarkMapForIndictor()
    {
        foreach (GardenTile tile in FinalAim)
        {
            tile.indicatorManager.StopIndicator();
        }
    }


}
