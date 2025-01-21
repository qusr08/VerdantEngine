using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for all functions relating to the garden. This does not store the data of the garden, that is done within the PlayerData scriptable object
/// </summary>
public class GardenManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameObject plantPrefab;
	[SerializeField] private PlayerData _playerData;

	/// <summary>
	/// A reference to the player data scriptable object
	/// </summary>
	public PlayerData PlayerData => _playerData;

	/// <summary>
	/// A list of all the plants that are currently in the garden
	/// </summary>
	public List<Plant> Plants { get; private set; }

	private void Awake ( ) {
		Plants = new List<Plant>( );
	}

	private void Start ( ) {
		/// TEST: Create test plants and move them around
		PlacePlant(PlantType.CACTUS, 6, 1);
		PlacePlant(PlantType.FLOWER, 0, 0);
		PlacePlant(PlantType.CACTUS, 1, 1);
		PlacePlant(PlantType.CACTUS, 1, 0);
		PlacePlant(PlantType.FLOWER, 2, 8);

		MovePlant(PlayerData.Garden[1, 1], 1, 2);

		UprootPlant(0, 0);

		Debug.Log("Flower Count: " + CountPlants(exclusivePlantTypes: new List<PlantType>( ) { PlantType.FLOWER }));
		Debug.Log("Cactus Count: " + CountPlants(exclusivePlantTypes: new List<PlantType>( ) { PlantType.CACTUS }));
	}

	/// <summary>
	/// Check to see if a position is within the bounds of the garden
	/// </summary>
	/// <param name="x">The x coordinate</param>
	/// <param name="y">The y coordinate</param>
	/// <returns>Whether or not the position is within the bounds of the garden</returns>
	public bool IsPositionWithinGarden (int x, int y) {
		return (x >= 0 && x < PlayerData.GardenSize && y >= 0 && y < PlayerData.GardenSize);
	}

	/// <summary>
	/// Set a new plant to a certain position in the garden
	/// </summary>
	/// <param name="plantType">The new plant object to place down</param>
	/// <param name="x">The x coordinate to place the plant at</param>
	/// <param name="y">The y coordinate to place the plant at</param>
	/// <returns>true if the plant was placed successfully, false otherwise. If a plant is already at the position on the board, then the new plant is not placed and the function returns false</returns>
	public bool PlacePlant (PlantType plantType, int x, int y) {
		// Do not try to make a plant with a plant type of NONE
		if (plantType == PlantType.NONE) {
			return false;
		}

		// Return false if the position that the new plant is being placed at is out of the bounds of the garden
		if (!IsPositionWithinGarden(x, y)) {
			return false;
		}

		// Return false if there is already a plant at the position specified
		if (PlayerData.Garden[x, y] != null) {
			return false;
		}

		// Place the plant onto the grid and update its position
		Plant plant = Instantiate(plantPrefab, transform).GetComponent<Plant>( );
		plant.PlantType = plantType;
		plant.Position = new Vector2Int(x, y);
		plant.Initialize( );

		PlayerData.Garden[x, y] = plant;
		Plants.Add(plant);

		return true;
	}

	/// <summary>
	/// Remove a plant from the garden
	/// </summary>
	/// <param name="plant">The plant to remove</param>
	/// <returns>true always</returns>
	public bool UprootPlant (Plant plant) {
		// Remove the plant from all lists and destroy it
		Plants.Remove(plant);
		PlayerData.Garden[plant.Position.x, plant.Position.y] = null;
		Destroy(plant.gameObject);

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
		Plant plant = PlayerData.Garden[x, y];

		// Return false if there is no plant at the position specified
		if (plant == null) {
			return false;
		}

		// Remove the plant from all lists and destroy it
		Plants.Remove(plant);
		PlayerData.Garden[x, y] = null;
		Destroy(plant.gameObject);

		return true;
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
	/// Count how many plants of a specific plant type are in the garden
	/// </summary>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>The number of plants that have been placed in the garden that match the specified exclusive and excluded plant types</returns>
	public int CountPlants (List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		return GetFilteredPlants(exclusivePlantTypes, excludedPlantTypes).Count;
	}

	/// <summary>
	/// Move a plant from its current position to another one
	/// </summary>
	/// <param name="plant">The plant that will be moved</param>
	/// <param name="x">The x coordinate to move the plant to</param>
	/// <param name="y">The y coordinate to move the plant to</param>
	/// <returns>true if the plant was successfully moved, false otherwise. Also returns false if the position that the plant was going to move to is out of the bounds of the garden or there was already a plant at that position</returns>
	public bool MovePlant (Plant plant, int x, int y) {
		// Return false if the position that the plant is being move to is out of the bounds of the garden
		if (!IsPositionWithinGarden(x, y)) {
			return false;
		}

		// Return false if there is already a plant at the position specified
		if (PlayerData.Garden[x, y] != null) {
			return false;
		}

		// Remove the reference to the plant from its current position and add it to the position it is being moved to
		// Also update the position of the plant
		PlayerData.Garden[plant.Position.x, plant.Position.y] = null;
		plant.Position = new Vector2Int(x, y);
		PlayerData.Garden[plant.Position.x, plant.Position.y] = plant;

		return true;
	}
}
