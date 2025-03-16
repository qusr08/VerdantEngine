using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// The highest abstracted class for all objects that will be able to be placed in the garden. This includes artifacts and plants (so far)
/// </summary>
public abstract class GardenPlaceable : MonoBehaviour {
	[Header("GardenPlaceable")]
	[SerializeField] protected GardenManager gardenManager;
	[SerializeField] protected PlayerDataManager playerDataManager;
	[Space]
	[SerializeField, Min(0), Tooltip("The starting health of this garden placeable.")] private int _startingHealth;
	[SerializeField, Min(0), Tooltip("The cost of this garden placeable in the shop.")] private int _cost;
	[SerializeField, Tooltip("The name of this garden placeable.")] private string _name;
	[SerializeField, Multiline, Tooltip("The description of this garden placeable.")] private string _description;
	[SerializeField, Tooltip("The sprite that shows up in the inventory for this garden placeable.")] private Sprite _inventorySprite;
	[Space]
	[SerializeField, Tooltip("The garden tile that this plant is on.")] private GardenTile _gardenTile;
	[SerializeField, Tooltip("The garden placeables that are currently effected by this garden placeable's stat modifiers.")] protected List<GardenPlaceable> effectedGardenPlaceables;

	private GardenPlaceableStat _healthStat;
	private GardenPlaceableStat _shieldStat;
	[HideInInspector] public Enemy LastEnemyWhichDamagedPlaceble;
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

		effectedGardenPlaceables = new List<GardenPlaceable>( );
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
		transform.LookAt(-Camera.main.transform.position + transform.position);
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
	}

	/// <summary>
	/// Called when the player's turn starts
	/// </summary>
	public virtual void OnTurnStart ( ) { }

	/// <summary>
	/// Called when the player's turn ends
	/// </summary>
	public virtual void OnTurnEnd ( ) { }

	/// <summary>
	/// Called when this garden placeable takes damage
	/// </summary>
	public virtual void OnTakeDamage ( ) { }

	/// <summary>
	/// Called when this garden placeable is killed
	/// </summary>
	public virtual void OnKilled ( ) {
		// Since this garden placeable was killed, remove all modifiers it was giving out
		RemoveModifiersFromEffectedGardenPlaceables( );
	}
	public virtual void Heal(int heal)
    {
		if(_healthStat.CurrentValue<StartingHealth)
        {
			_healthStat.BaseValue += heal;
			if (_healthStat.BaseValue > StartingHealth)
				_healthStat.BaseValue = StartingHealth;
		}
    }
	public virtual void TakeDamage(int damage)
    {
		HealthStat.BaseValue -= damage;

	}
}
