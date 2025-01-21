using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for all functions
/// </summary>
public class Garden : MonoBehaviour {
	[Header("References")]
	[SerializeField] private GameObject plantPrefab;
	[Header("Properties")]
	[SerializeField] private int gardenSize;

	/// <summary>
	/// The grid of plants that represent the layout of the garden
	/// </summary>
	public Plant[ , ] Grid { get; private set; }

	private void Awake ( ) {
		Grid = new Plant[gardenSize, gardenSize];
	}

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
		return (x >= 0 && x < gardenSize && y >= 0 && y < gardenSize);
	}

	/// <summary>
	/// Get a plant object at a specific position on the board
	/// </summary>
	/// <param name="x">The x coordinate to check</param>
	/// <param name="y">The y coordinate to check</param>
	/// <returns>A plant object if one is at the position, null otherwise. Will also return null if the position is out of the bounds of the board</returns>
	public Plant GetPlantAt (int x, int y) {
		// Check to see if the position is within the bounds of the garden before trying to access the position in the grid
		if (!IsPositionWithinGarden(x, y)) {
			return null;
		}

		return Grid[x, y];
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
		if (GetPlantAt(x, y) != null) {
			return false;
		}

		// Place the plant onto the grid and update its position
		Plant plant = Instantiate(plantPrefab, transform).GetComponent<Plant>( );
		plant.PlantType = plantType;
		plant.Position = new Vector2Int(x, y);
		plant.Initialize( );

		Grid[x, y] = plant;

		return true;
	}
}
