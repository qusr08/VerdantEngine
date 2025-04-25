using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// The highest abstracted class for all objects that will be able to be placed in the garden. This includes artifacts and plants (so far)
/// </summary>
public abstract class GardenPlaceable : MonoBehaviour {
	public bool isPlant = true;
	[Header("GardenPlaceable")]
	[SerializeField] protected GardenManager gardenManager;
	[SerializeField] protected PlayerDataManager playerDataManager;
	[SerializeField] protected CombatManager combatManager;
	[SerializeField] protected List<SpriteRenderer> spriteRenderers;
	[Space]
	[SerializeField, Min(0), Tooltip("The starting health of this garden placeable.")] private int _startingHealth;
	[SerializeField, Min(0), Tooltip("The cost of this garden placeable in the shop.")] private int _cost;
	[SerializeField, Tooltip("The name of this garden placeable.")] private string _name;
	[SerializeField, Multiline, Tooltip("The description of this garden placeable.")] private string _description;
	[SerializeField, Tooltip("The sprite that shows up in the inventory for this garden placeable.")] private Sprite _inventorySprite;
	[Space]
	[SerializeField, Tooltip("The garden tile that this plant is on.")] private GardenTile _gardenTile;
	[SerializeField, Tooltip("The garden placeables that are currently effected by this garden placeable's stat modifiers.")] protected List<GardenPlaceable> effectedGardenPlaceables;
	[SerializeField, Tooltip("The damage indector pop up.")] protected GameObject damageIndicatorPrefab;
	[SerializeField, Tooltip("The time between flashes when this garden placeable is on low health")] private float lowHealthColorFlashTime = 0.5f;
	[SerializeField, Tooltip("The time it takes for the color to fade when healing this garden placeable")] private float healColorTime = 1.5f;
	[SerializeField, Tooltip("The time in between flashes when this garden placeable is taking damage")] private float damageColorFlashTime = 0.1f;


	[Header("Heal/ Shield/ Damage effects")]
	[SerializeField] protected GameObject healEffectPrefab;
	[SerializeField] protected GameObject damageEffectPrefab;
	[SerializeField] protected GameObject shieldEffectPrefab;


	private GardenPlaceableStat _healthStat;
	private GardenPlaceableStat _shieldStat;

	public GameObject flowerVisuals;
	private Coroutine colorCoroutine = null;

	/// <summary>
	/// The starting health of this garden placeable
	/// </summary>
	public int StartingHealth => _startingHealth;

	/// <summary>
	/// The cost of this garden placeable in a shop
	/// </summary>
	public int Cost => _cost;

	/// <summary>
	/// The name of this garden placeable
	/// </summary>
	public string Name { get => _name; }

	/// <summary>
	/// The description of this garden placeable
	/// </summary>
	public string Description { get => _description; }

	/// <summary>
	/// The sprite that will show up in the inventory of this garden placeable
	/// </summary>
	public Sprite InventorySprite { get => _inventorySprite; }

	/// <summary>
	/// The current health stat of this garden placeable
	/// </summary>
	public GardenPlaceableStat HealthStat { get => _healthStat; private set => _healthStat = value; }

	/// <summary>
	/// The current shield stat of this garden placeable
	/// </summary>
	public GardenPlaceableStat ShieldStat { get => _shieldStat; private set => _shieldStat = value; }

	/// <summary>
	/// The position of this plant in the garden
	/// </summary>
	public Vector2Int Position => GardenTile.Position;

	/// <summary>
	/// The garden tile that this plant is on
	/// </summary>
	public GardenTile GardenTile {
		get => _gardenTile;
		set {
			// If the incoming value is equal to null or to the current garden tile value, then return
			if (_gardenTile == value || value == null) {
				return;
			}

			// Remove the reference of this garden placeable from the old garden tile
			if (_gardenTile != null) {
				_gardenTile.GardenPlaceable = null;
			}
			_gardenTile = value;
			_gardenTile.GardenPlaceable = this;

			// Should be changing the y value but because the garden tile is rotated, we need to set the z value instead
			transform.SetParent(_gardenTile.transform, false);
			transform.localPosition = new Vector3(0f, 0f, -0.5f);
		}
	}


	private void Awake ( ) {
		gardenManager = FindObjectOfType<GardenManager>( );
		playerDataManager = FindObjectOfType<PlayerDataManager>( );
        combatManager = FindObjectOfType<CombatManager>( );
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>( ).ToList();

		effectedGardenPlaceables = new List<GardenPlaceable>();



    }

	private void Update ( ) {
		// Make sure the plants are always facing towards the camera
		transform.LookAt(-Camera.main.transform.position + transform.position);

		// Constantly flash the sprite red if it has low health
		// Low health is considered anything below 25% health, or at minimum 1 health
		// Also make sure the starting health is greater than 1 so that plants with 1 health do not blink all the time
		if (colorCoroutine == null && HealthStat.CurrentValue <= Mathf.Max(1, Mathf.FloorToInt(StartingHealth / 4f)) && StartingHealth > 1) {
			FlashColor(Color.red, lowHealthColorFlashTime, lowHealthColorFlashTime, 1);
		}
	}

	private void OnDisable ( ) {
		if (colorCoroutine != null) {
			StopCoroutine(colorCoroutine);
			colorCoroutine = null;
		}
	}

	/// <summary>
	/// Initialize this plant right after it has been created in the garden. Everything in this function needs to be called before Awake() but after it is instantiated in the garden.
	/// </summary>
	/// <param name="gardenTile">The garden tile that this plant is starting on</param>
	public void Initialize (GardenTile gardenTile) {
		GardenTile = gardenTile;

		// Set up stats
		HealthStat = new GardenPlaceableStat(StartingHealth);
		HealthStat.WhenZero += OnKilled;
		HealthStat.OnUpdateCurrentValue += OnTakeDamage;
		ShieldStat = new GardenPlaceableStat(0);

		// Make sure the plants are always facing towards the camera
		// transform.LookAt(-Camera.main.transform.position + transform.position);
		// transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
	}

	/// <summary>
	/// Get a list of specific plants that are surrounding this garden placeable within a certain radius
	/// </summary>
	/// <param name="radius">The radius around this plant to check for other plants</param>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final surrounding plants list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final surrounding plants list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>A list of all the surrounding plant objects around this garden placeable that match the exclusive and excluded plant types</returns>
	public List<Plant> GetSurroundingPlants (int radius, List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		List<Plant> surroundingPlants = new List<Plant>( );

		// Loop through all plants that are surrounding this garden placeable within a certain radius
		for (int x = -radius; x <= radius; x++) {
			for (int y = -radius; y <= radius; y++) {
				int checkX = Position.x + x;
				int checkY = Position.y + y;

				// If the position to check is not within the bounds of the garden, then continue to the next position
				// Also continue to the next position if the current plant being checked is this placeable
				if (!gardenManager.IsPositionWithinGarden(checkX, checkY) || (x == 0 && y == 0)) {
					continue;
				}

				// Get a reference to the plant at the current position being checked
				// If the garden has another object at this position that is not a plant, then the "as" operator will return null
				// https://www.geeksforgeeks.org/c-sharp-as-operator-keyword/
				Plant plant = playerDataManager.Garden[checkX, checkY].GardenPlaceable as Plant;

				// If there is currently no plant at the current position, then continue to the next position
				if (plant == null) {
					continue;
				}

				// If the current plant being checked has a plant type that is in the excluded plant types list, then ignore the plant and continue to the next position
				if (excludedPlantTypes != null && excludedPlantTypes.Contains(plant.PlantType)) {
					continue;
				}

				// If the current plant being checked has a plant type that is in the exclusive plant types list, then add that plant to the list of surrounding plants
				// If the exclusive plants list is null, then just add every type of plant
				if (exclusivePlantTypes == null || (exclusivePlantTypes != null && exclusivePlantTypes.Contains(plant.PlantType))) {
					surroundingPlants.Add(plant);
				}
			}
		}

		return surroundingPlants;
	}

	/// <summary>
	/// Get a list of specific artifacts that are surrounding this garden placeable within a certain radius
	/// </summary>
	/// <param name="radius">The radius around this plant to check for other plants</param>
	/// <param name="exclusiveArtifactTypes">All artifact types within this list will exclusively be added to the final surrounding artifacts list. If this parameter is left null, all artifacts of any artifact type will be added</param>
	/// <param name="excludedArtifactTypes">All artifact types within this list will never be added to the final surrounding artifacts list. If this parameter is left null, no artifact types will be excluded</param>
	/// <returns>A list of all the surrounding artifact objects around this garden placeable that match the exclusive and excluded artifact types</returns>
	public List<Artifact> GetSurroundingArtifacts (int radius, List<ArtifactType> exclusiveArtifactTypes = null, List<ArtifactType> excludedArtifactTypes = null) {
		List<Artifact> surroundingArtifacts = new List<Artifact>( );

		// Loop through all artifacts that are surrounding this garden placeable within a certain radius
		for (int x = -radius; x <= radius; x++) {
			for (int y = -radius; y <= radius; y++) {
				int checkX = Position.x + x;
				int checkY = Position.y + y;

				// If the position to check is not within the bounds of the garden, then continue to the next position
				// Also continue to the next position if the current artifact being checked is this placeable
				if (!gardenManager.IsPositionWithinGarden(checkX, checkY) || (x == 0 && y == 0)) {
					continue;
				}

				// Get a reference to the artifact at the current position being checked
				Artifact artifact = playerDataManager.Garden[checkX, checkY].GardenPlaceable as Artifact;

				// If there is currently no artifact at the current position, then continue to the next position
				if (artifact == null) {
					continue;
				}

				// If the current artifact being checked has a plant type that is in the excluded artifact types list, then ignore the artifact and continue to the next position
				if (excludedArtifactTypes != null && excludedArtifactTypes.Contains(artifact.ArtifactType)) {
					continue;
				}

				// If the current artifact being checked has an artifact type that is in the exclusive artifact types list, then add that artifact to the list of surrounding artifacts
				// If the exclusive artifacts list is null, then just add every type of artifact
				if (exclusiveArtifactTypes == null || (exclusiveArtifactTypes != null && exclusiveArtifactTypes.Contains(artifact.ArtifactType))) {
					surroundingArtifacts.Add(artifact);
				}
			}
		}

		return surroundingArtifacts;
	}

	/// <summary>
	/// Count how many plants of a specific type are around this garden placeable within a certain radius
	/// </summary>
	/// <param name="radius">The radius around this plant to check for other plants</param>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final surrounding plants list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final surrounding plants list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>The number of plants around this garden placeable that match the exclusive and excluded plant types</returns>
	public int CountSurroundingPlants (int radius, List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		return GetSurroundingPlants(radius, exclusivePlantTypes, excludedPlantTypes).Count;
	}

	/// <summary>
	/// Count how many artifacts of a specific type are around this garden placeable within a certain radius
	/// </summary>
	/// <param name="radius">The radius around this plant to check for other plants</param>
	/// <param name="exclusiveArtifactTypes">All artifact types within this list will exclusively be added to the final surrounding artifacts list. If this parameter is left null, all artifacts of any artifact type will be added</param>
	/// <param name="excludedArtifactTypes">All artifact types within this list will never be added to the final surrounding artifacts list. If this parameter is left null, no artifact types will be excluded</param>
	/// <returns>A list of all the surrounding artifact objects around this garden placeable that match the exclusive and excluded artifact types</returns>
	public int CountSurroundingArtifacts (int radius, List<ArtifactType> exclusiveArtifactTypes = null, List<ArtifactType> excludedArtifactTypes = null) {
		return GetSurroundingArtifacts(radius, exclusiveArtifactTypes, excludedArtifactTypes).Count;
	}

	/// <summary>
	/// Remove all modifiers from all the garden placeables that are effected by this garden placeable. This should be used before recalculating modifiers
	/// </summary>
	protected void RemoveModifiersFromEffectedGardenPlaceables ( ) {
		// Clear all modifiers from the previously effected garden placeables
		for (int i = effectedGardenPlaceables.Count - 1; i >= 0; i--) {
			effectedGardenPlaceables[i].HealthStat.RemoveModifiers(this);
			effectedGardenPlaceables[i].ShieldStat.RemoveModifiers(this);
			effectedGardenPlaceables.RemoveAt(i);
		}
	}

	/// <summary>
	/// Called when the garden is update in any way. This means that when a plant is placed or removed on the board, this function is called
	/// </summary>
	public virtual void OnGardenUpdated ( ) {
		// Since the garden was changed, all modifiers should be updated
		RemoveModifiersFromEffectedGardenPlaceables( );

		// Sort all of the sprites based on their position
		gameObject.GetComponentInChildren<SpriteSortingOrder>( ).SortSprites(GardenTile.Position.x, GardenTile.Position.y);
	}

	/// <summary>
	/// Called when the player's turn starts
	/// </summary>
	public virtual void OnTurnStart ( ) { }

	/// <summary>
	/// Called when the player's turn ends
	/// </summary>
	public virtual void OnTurnEnd ( ) { }

    ///<summary>
    ///Called when this object is moved in the garden
    ///</summary>
    public virtual void OnMoved() { }

    /// <summary>
    /// Called when this garden placeable takes damage
    /// </summary>
    public virtual void OnTakeDamage ( ) { }

	/// <summary>
	/// Called on the first drop of a garden Placeble
	/// </summary
	bool isFirst = true;
    public virtual void OnFirstPlanted()
	{
      
    }

	/// <summary>
	/// Called when this garden placeable is killed
	/// </summary>
	public virtual void OnKilled ( ) {
		// Since this garden placeable was killed, remove all modifiers it was giving out
		RemoveModifiersFromEffectedGardenPlaceables( );
		gardenManager.GlobalOnPlantDestoyed(this);

	}
	public virtual void Heal (int heal) {
		Debug.Log("Name:" + Name + "\n" +
			"CurrentValue:" + _healthStat.CurrentValue + "\n" +
			   "TotalModifier:" + _healthStat.TotalModifier + "\n" +
			   "BaseValue:" + _healthStat.BaseValue + "\n");

		if (_healthStat.CurrentValue < StartingHealth + _healthStat.TotalModifier) {
			if (_healthStat.BaseValue + heal >= StartingHealth + _healthStat.TotalModifier) {
				heal = StartingHealth + _healthStat.TotalModifier - _healthStat.BaseValue;

				_healthStat.BaseValue = StartingHealth;
			} else if (_healthStat.CurrentValue < StartingHealth) {
				_healthStat.BaseValue += heal;
			}
		} else {
			heal = 0;
		}

        if (heal > 0){
			SpawnHealIndicator(heal);
			GameObject effect = Instantiate(healEffectPrefab, transform.position, transform.rotation);
			effect.transform.parent = transform;
			Destroy(effect, 5f);
		}
            
		FlashColor(Color.green, healColorTime / 4f, healColorTime, 1);

		//triggered global on heal triggers for all garden placbles. used by artifacts
		if (heal > 0)
			gardenManager.GlobalOnHealedTrigger(this, heal);

	}

	/// <summary>
	/// Have this garden placeable take damage from a specific enemy
	/// </summary>
	/// <param name="enemy">The enemy that is dealing the damage. Can be null if the plant is taking damage from a source that is not an enemy</param>
	/// <param name="damage">The damage that is being dealt</param>
	/// <returns>The actual damage that was dealt to the health of the plant. This takes into account the shield that the plant has</returns>
	public virtual int TakeDamage (Enemy enemy, int damage) {
		// Subtract the current shield of this garden placeable from the damage that is trying to be dealt
		if(ShieldStat.CurrentValue>0)
		damage -= ShieldStat.CurrentValue;

		if(damage <= 0)
        {
			GameObject effect = Instantiate(shieldEffectPrefab, transform.position, transform.rotation);
			effect.transform.parent = transform;
			Destroy(effect, 5f);
        }
		// If the damage is still greater than 0, then decrease the placeable's health
		if (damage > 0) {
			HealthStat.BaseValue -= damage;
			SpawnDamageIndicator(damage);

			Debug.Log("Name:" + Name + "\n" +
			"CurrentValue:" + _healthStat.CurrentValue + "\n" +
			"TotalModifier:" + _healthStat.TotalModifier + "\n" +
			"BaseValue:" + _healthStat.BaseValue + "\n");

			// Flash the sprite a certain color when it takes damage
			FlashColor(Color.red, damageColorFlashTime / 2f, damageColorFlashTime, 2);

			GameObject effect = Instantiate(damageEffectPrefab, transform.position, transform.rotation);
			effect.transform.parent = transform;
			Destroy(effect, 5f);

			return damage;
		}

		SpawnDamageIndicator(0);
		return 0;
	}

	/// <summary>
	/// Flash the placeable sprite a certain color
	/// </summary>
	/// <param name="color">The color to flash</param>
	/// <param name="fadeInTime">How long it takes to fade to the color</param>
	/// <param name="fadeOutTime">How long it takes to fade back to the original color</param>
	/// <param name="flashCount">The number of color flashes to do</param>
	private void FlashColor (Color color, float fadeInTime, float fadeOutTime, int flashCount) {
		// If the current color coroutine is not null, stop it so a new color can be flashed
		if (colorCoroutine != null) {
			StopCoroutine(colorCoroutine);
		}

		// Start a new color flash
		colorCoroutine = StartCoroutine(IFlashColor(color, fadeInTime, fadeOutTime, flashCount));
	}

	private IEnumerator IFlashColor (Color color, float fadeInTime, float fadeOutTime, int flashCount) {
		float t = 0f;

		// Repeat the flash a certain amount of times
		for (int i = 0; i < flashCount; i++) {
			// Fade to the color
			while (t < fadeInTime) {
				t = Mathf.Min(fadeInTime, t + Time.deltaTime);

				// Since there are multiple sprite renderers on each garden placeable, loop through all of them and tint their color
				foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
					spriteRenderer.color = Color.Lerp(Color.white, color, t / fadeInTime);
				}

				yield return null;
			}

			t = 0f;

			// Fade back to the original color
			while (t < fadeOutTime) {
				t = Mathf.Min(fadeOutTime, t + Time.deltaTime);

				// Since there are multiple sprite renderers on each garden placeable, loop through all of them and tint their color
				foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
					spriteRenderer.color = Color.Lerp(color, Color.white, t / fadeOutTime);
				}

				yield return null;
			}

			yield return null;
		}

		// Clear the reference to the color coroutine because it has finished
		colorCoroutine = null;
	}

	public void SpawnDamageIndicator (int damageafterShiled) {
		DamageIndicator indicator = Instantiate(damageIndicatorPrefab, transform.position, transform.rotation).GetComponent<DamageIndicator>( );
		indicator.SetDamage(damageafterShiled);
	}

	public void SpawnHealIndicator (int heal) {
		DamageIndicator indicator = Instantiate(damageIndicatorPrefab, transform.position, transform.rotation).GetComponent<DamageIndicator>( );
		indicator.SetHeal(heal);
	}
}
