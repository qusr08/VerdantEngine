using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This class is used for all functions relating to the garden. This does not store the data of the garden, that is done within the PlayerData scriptable object
/// </summary>
public class GardenManager : MonoBehaviour {
	[SerializeField] private PlantPrefabDictionary _plantPrefabs;
	[SerializeField] private ArtifactPrefabDictionary _artifactPrefabs;
	[SerializeField] private GameObject groundTilePrefab;
	[SerializeField] public PlayerDataManager playerDataManager;
	[Space]
	[SerializeField] private GardenTile _selectedGardenTile;

	/// <summary>
	/// A list of all the plant prefabs that can be placed on the garden
	/// </summary>
	public PlantPrefabDictionary PlantPrefabs { get => _plantPrefabs; private set => _plantPrefabs = value; }

	/// <summary>
	/// A list of all the artifact prefabs that can be placed on the garden
	/// </summary>
	public ArtifactPrefabDictionary ArtifactPrefabs { get => _artifactPrefabs; private set => _artifactPrefabs = value; }

	/// <summary>
	/// A list of all the plants that are currently in the garden
	/// </summary>
	public List<Plant> Plants { get; private set; }

	/// <summary>
	/// A list of all the artifacts that are currently in the garden
	/// </summary>
	public List<Artifact> Artifacts { get; private set; }

	/// <summary>
	/// The garden tile that is currently selected
	/// </summary>
	public GardenTile SelectedGardenTile { get => _selectedGardenTile; set => _selectedGardenTile = value; }

	private void Awake ( ) {
		playerDataManager = FindObjectOfType<PlayerDataManager>( );

		Plants = new List<Plant>( );
		Artifacts = new List<Artifact>( );
	}

	private void Start ( ) {
		CreateGardenTiles( );

		/// TEST: Create test plants and move them around
		PlacePlant(PlantType.HARDY_HEDGE, 1, 3);
		PlacePlant(PlantType.EMPOWEROOT, 0, 2);
		PlacePlant(PlantType.POWER_FLOWER, 0, 3);

		//PlacePlant(PlantType.HARDY_HEDGE, 2, 0);
		//PlacePlant(PlantType.HARDY_HEDGE, 3, 1);
		//PlacePlant(PlantType.HARDY_HEDGE, 2, 2);
		//PlacePlant(PlantType.HARDY_HEDGE, 2, 3);
		//PlacePlant(PlantType.HARDY_HEDGE, 3, 4);
		//PlacePlant(PlantType.HARDY_HEDGE, 2, 5);

		//PlacePlant(PlantType.EMPOWEROOT, 1, 2);
		//PlacePlant(PlantType.EMPOWEROOT, 1, 3);

		//PlacePlant(PlantType.POWER_FLOWER, 0, 1);
		//PlacePlant(PlantType.POWER_FLOWER, 0, 2);
		//PlacePlant(PlantType.POWER_FLOWER, 0, 3);
		//PlacePlant(PlantType.POWER_FLOWER, 0, 4);

		//PlacePlant(PlantType.SHIELDING_SHRUB, 2, 1);
		//PlacePlant(PlantType.SHIELDING_SHRUB, 2, 4);


		//MovePlant(PlayerData.Garden[1, 1].GardenPlaceable as Plant, 1, 2);

		//UprootPlant(0, 0);

		// Debug.Log("Flower Count: " + CountPlants(exclusivePlantTypes: new List<PlantType>( ) { PlantType.FLOWER }));
		// Debug.Log("Cactus Count: " + CountPlants(exclusivePlantTypes: new List<PlantType>( ) { PlantType.CACTUS }));
	}

	/// <summary>
	/// Check to see if a position is within the bounds of the garden
	/// </summary>
	/// <param name="x">The x coordinate</param>
	/// <param name="y">The y coordinate</param>
	/// <returns>Whether or not the position is within the bounds of the garden</returns>
	public bool IsPositionWithinGarden (int x, int y) {
		return (x >= 0 && x < playerDataManager.GardenSize && y >= 0 && y < playerDataManager.GardenSize);
	}

	/// <summary>
	/// Set a new plant to a certain position in the garden
	/// </summary>
	/// <param name="plantType">The new plant object to place down</param>
	/// <param name="x">The x coordinate to place the plant at</param>
	/// <param name="y">The y coordinate to place the plant at</param>
	/// <returns>true if the plant was placed successfully, false otherwise. If a placeable is already at the position on the board, then the new plant is not placed and the function returns false</returns>
	public bool PlacePlant (PlantType plantType, int x, int y) {
		// Do not try to make a plant with a plant type of NONE
		if (plantType == PlantType.NONE) {
			return false;
		}

		// Return false if the position that the new plant is being placed at is out of the bounds of the garden
		if (!IsPositionWithinGarden(x, y)) {
			return false;
		}

		// Return false if there is already a placeable at the position specified
		if (playerDataManager.Garden[x, y].GardenPlaceable != null) {
			return false;
		}

		// Place the plant onto the grid and update its position
		Plant plant = Instantiate(PlantPrefabs[plantType], playerDataManager.Garden[x, y].transform).GetComponent<Plant>( );
		plant.Initialize(playerDataManager.Garden[x, y]);

		playerDataManager.Garden[x, y].GardenPlaceable = plant;
		Plants.Add(plant);
		UpdateGarden( );

		return true;
	}

	/// <summary>
	/// Set a new artifact to a certain position in the garden
	/// </summary>
	/// <param name="artifactType">The new artifact object to place down</param>
	/// <param name="x">The x coordinate to place the artifact at</param>
	/// <param name="y">The y coordinate to place the artifact at</param>
	/// <returns>true if the artifact was placed successfully, false otherwise. If a placeable is already at the position on the board, then the new artifact is not placed and the function returns false</returns>
	public bool PlaceArtifact (ArtifactType artifactType, int x, int y) {
		// Do not try to make an artifact with an artifact type of NONE
		if (artifactType == ArtifactType.NONE) {
			return false;
		}

		// Return false if the position that the new artifact is being placed at is out of the bounds of the garden
		if (!IsPositionWithinGarden(x, y)) {
			return false;
		}

		// Return false if there is already a placeable at the position specified
		if (playerDataManager.Garden[x, y].GardenPlaceable != null) {
			return false;
		}

		// Place the artifact onto the grid and update its position
		Artifact artifact = Instantiate(ArtifactPrefabs[artifactType], playerDataManager.Garden[x, y].transform).GetComponent<Artifact>( );
		artifact.Initialize(playerDataManager.Garden[x, y]);

		playerDataManager.Garden[x, y].GardenPlaceable = artifact;
		Artifacts.Add(artifact);
		UpdateGarden( );

		return true;
	}

	/// <summary>
	/// Remove a plant from the garden
	/// </summary>
	/// <param name="plant">The plant to remove</param>
	/// <returns>true if the plant was successfully removed from the garden</returns>
	public bool UprootPlant (Plant plant) {
		// If the plant object is null, then return false
		if (plant == null) {
			return false;
		}

		// Remove the plant from all lists and destroy it
		Plants.Remove(plant);
		playerDataManager.Garden[plant.Position.x, plant.Position.y].GardenPlaceable = null;
		Destroy(plant.gameObject);
		UpdateGarden( );

		return true;
	}

	/// <summary>
	/// Remove an artifact from the garden
	/// </summary>
	/// <param name="artifact">The artifact to remove</param>
	/// <returns>true if the artifact was successfully removed from the garden</returns>
	public bool UprootArtifact (Artifact artifact) {
		// If the artifact object is null, then return false
		if (artifact == null) {
			return false;
		}

		// Remove the artifact from all lists and destroy it
		Artifacts.Remove(artifact);
		playerDataManager.Garden[artifact.Position.x, artifact.Position.y].GardenPlaceable = null;
		Destroy(artifact.gameObject);
		UpdateGarden( );

		return true;
	}

	/// <summary>
	/// Remove a plant from the garden
	/// </summary>
	/// <param name="x">The x coordinate of the plant to remove</param>
	/// <param name="y">The y coordinate of the plant to remove</param>
	/// <returns>true if there was a plant successfully removed, false otherwise. Also returns false if the position specified was out of the bounds of the garden or there was no plant at the specified position to remove</returns>
	public bool UprootPlant (int x, int y) {
		// Return false if the position that the new plant is being placed at is out of the bounds of the garden
		if (!IsPositionWithinGarden(x, y)) {
			return false;
		}

		// Get the plant at the specified position
		Plant plant = playerDataManager.Garden[x, y].GardenPlaceable as Plant;

		// Return false if there is no plant at the position specified
		if (plant == null) {
			return false;
		}

		// Remove the plant from all lists and destroy it
		Plants.Remove(plant);
		playerDataManager.Garden[x, y].GardenPlaceable = null;
		Destroy(plant.gameObject);
		UpdateGarden( );

		return true;
	}

	/// <summary>
	/// Move a plant from its current position to another one
	/// </summary>
	/// <param name="plant">The plant that will be moved</param>
	/// <param name="x">The x coordinate to move the plant to</param>
	/// <param name="y">The y coordinate to move the plant to</param>
	/// <returns>true if the plant was successfully moved, false otherwise. Also returns false if the position that the plant was going to move to is out of the bounds of the garden or there was already a plant at that position</returns>
	public bool MovePlant (Plant plant, int x, int y) {
		// If the plant that was going to be moved is null, then return false
		if (plant == null) {
			return false;
		}

		// Return false if the position that the plant is being move to is out of the bounds of the garden
		if (!IsPositionWithinGarden(x, y)) {
			return false;
		}

		// Return false if there is already a plant at the position specified
		if (playerDataManager.Garden[x, y].GardenPlaceable != null) {
			return false;
		}

		// Remove the reference to the plant from its current position and add it to the position it is being moved to
		// Also update the position of the plant
		playerDataManager.Garden[plant.Position.x, plant.Position.y].GardenPlaceable = null;
		plant.GardenTile = playerDataManager.Garden[x, y];
		playerDataManager.Garden[plant.Position.x, plant.Position.y].GardenPlaceable = plant;
		UpdateGarden( );

		return true;
	}

	/// <summary>
	/// Update all garden placeables currently on the garden
	/// </summary>
	private void UpdateGarden ( ) {
		// Loop through all garden tiles and update the garden placeables on them
		foreach (GardenTile gardenTile in playerDataManager.Garden) {
			gardenTile.GardenPlaceable?.OnGardenUpdated( );
		}
	}

	/// <summary>
	/// Set up the garden and create all of the garden tiles
	/// </summary>
	private void CreateGardenTiles ( ) {
		// Create all of the ground tiles for the garden
		for (int x = 0; x < playerDataManager.GardenSize; x++) {
			for (int y = 0; y < playerDataManager.GardenSize; y++) {
				// Create the ground tile object and set its position
				GardenTile gardenTile = Instantiate(groundTilePrefab, transform).GetComponent<GardenTile>( );
				gardenTile.Position = new Vector2Int(x, y);
				playerDataManager.Garden[x, y] = gardenTile;
				gardenTile.UIDisplay = FindObjectOfType<PlantHover>();

            }
		}
	}

	/// <summary>
	/// Get plants that have been placed in the garden that match the exclusive and excluded plant types specified. Filters through the current plants and only includes/excludes certain types
	/// </summary>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>A list of all the plants that match the plant types specified</returns>
	public List<Plant> GetFilteredPlants (List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		List<Plant> filteredPlants = new List<Plant>( );

		// Loop through all of the plants that have been placed into the garden
		for (int i = 0; i < Plants.Count; i++) {
			// If the current plant being checked has a plant type that is in the excluded plant types list, then ignore the plant and continue to the next position
			if (excludedPlantTypes != null && excludedPlantTypes.Contains(Plants[i].PlantType)) {
				continue;
			}

			// If the current plant being checked has a plant type that is in the exclusive plant types list, then add that plant to the list of surrounding plants
			// If the exclusive plants list is null, then just add every type of plant
			if (exclusivePlantTypes == null || (exclusivePlantTypes != null && exclusivePlantTypes.Contains(Plants[i].PlantType))) {
				filteredPlants.Add(Plants[i]);
			}
		}

		return filteredPlants;
	}

	/// <summary>
	/// Get artifacts that have been placed in the garden that match the exclusive and excluded artifact types specified. Filters through the current artifacts and only includes/excludes certain types
	/// </summary>
	/// <param name="exclusiveArtifactTypes">All artifact types within this list will exclusively be added to the final list. If this parameter is left null, all artifacts of any artifact type will be added</param>
	/// <param name="excludedArtifactTypes">All artifact types within this list will never be added to the final list. If this parameter is left null, no artifact types will be excluded</param>
	/// <returns>A list of all the artifacts that match the artifact types specified</returns>
	public List<Artifact> GetFilteredArtifacts (List<ArtifactType> exclusiveArtifactTypes = null, List<ArtifactType> excludedArtifactTypes = null) {
		List<Artifact> filteredArtifacts = new List<Artifact>( );

		// Loop through all of the artifacts that have been placed into the garden
		for (int i = 0; i < Artifacts.Count; i++) {
			// If the current artifact being checked has an artifact type that is in the excluded artifact types list, then ignore the artifact and continue to the next position
			if (excludedArtifactTypes != null && excludedArtifactTypes.Contains(Artifacts[i].ArtifactType)) {
				continue;
			}

			// If the current artifact being checked has an artifact type that is in the exclusive artifact types list, then add that artifact to the list of surrounding artifacts
			// If the exclusive artifacts list is null, then just add every type of artifact
			if (exclusiveArtifactTypes == null || (exclusiveArtifactTypes != null && exclusiveArtifactTypes.Contains(Artifacts[i].ArtifactType))) {
				filteredArtifacts.Add(Artifacts[i]);
			}
		}

		return filteredArtifacts;
	}

	/// <summary>
	/// Get plants that have been placed in the garden at the specified positions that match the exclusive and excluded plant types
	/// </summary>
	/// <param name="positions">A list of the positions to check for plants at</param>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>A list of all the plants that are on the positions specified that match the exclusive and excluded plant types</returns>
	public List<Plant> GetFilteredPlantsAtPositions (List<Vector2Int> positions, List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		List<Plant> filteredPlants = new List<Plant>( );

		// Loop through all positions specified for this function
		foreach (Vector2Int position in positions) {
			// If the position to check is not within the bounds of the garden, then continue to the next position
			if (!IsPositionWithinGarden(position.x, position.y)) {
				continue;
			}

			// Get a reference to the plant at the current position being checked
			Plant plant = playerDataManager.Garden[position.x, position.y].GardenPlaceable as Plant;

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
				filteredPlants.Add(plant);
			}
		}

		return filteredPlants;
	}

	/// <summary>
	/// Count how many plants of a specific plant type are in the garden
	/// </summary>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>The number of plants that have been placed in the garden that match the specified exclusive and excluded plant types</returns>
	public int CountPlants (List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		return GetFilteredPlants(exclusivePlantTypes, excludedPlantTypes).Count;
	}

	/// <summary>
	/// Count how many artifacts of a specific artifact type are in the garden
	/// </summary>
	/// <param name="exclusiveArtifactTypes">All artifact types within this list will exclusively be added to the final list. If this parameter is left null, all artifacts of any artifact type will be added</param>
	/// <param name="excludedArtifactTypes">All artifact types within this list will never be added to the final list. If this parameter is left null, no artifact types will be excluded</param>
	/// <returns>The number of artifacts that have been placed in the garden that match the specified exclusive and excluded artifact types</returns>
	public int CountArtifacts (List<ArtifactType> exclusiveArtifactTypes = null, List<ArtifactType> excludedArtifactTypes = null) {
		return GetFilteredArtifacts(exclusiveArtifactTypes, excludedArtifactTypes).Count;
	}
}
