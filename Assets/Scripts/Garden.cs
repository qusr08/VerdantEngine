using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for all functions
/// </summary>
public class Garden : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameObject plantPrefab;
	[SerializeField] private PlayerData _playerData;

	/// <summary>
	/// A reference to the player data scriptable object
	/// </summary>
	public PlayerData PlayerData => _playerData;

	private void Start ( ) {
		/// TEST: Create test plants
		PlaceNewPlant(PlantType.CACTUS, 6, 1);
		PlaceNewPlant(PlantType.FLOWER, 0, 0);
		PlaceNewPlant(PlantType.CACTUS, 1, 1);
		PlaceNewPlant(PlantType.CACTUS, 1, 0);
		PlaceNewPlant(PlantType.FLOWER, 2, 8);
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
	public bool PlaceNewPlant (PlantType plantType, int x, int y) {
		// Do not try to make a plant with a plant type of NONE
		if (plantType == PlantType.NONE) {
			return false;
		}

		// Return false if the position that the new plant is being placed at is out of the bounds of the garden
		if (!IsPositionWithinGarden(x, y)) {
			return false;
		}

		// Return false if there is already a plant at the position specified
		if (PlayerData.Grid[x, y] != null) {
			return false;
		}

		// Place the plant onto the grid and update its position
		Plant plant = Instantiate(plantPrefab, transform).GetComponent<Plant>( );
		plant.PlantType = plantType;
		plant.Position = new Vector2Int(x, y);
		plant.Initialize( );

		PlayerData.Grid[x, y] = plant;

		return true;
	}
}
