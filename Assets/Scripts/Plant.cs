using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantType {
	NONE, FLOWER, CACTUS
}

public class Plant : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Garden garden;
	[Header("Properties")]
	[SerializeField] private PlantType _plantType;
	[SerializeField] private Vector2Int _position;

	/// <summary>
	/// The type of this plant
	/// </summary>
	public PlantType PlantType {
		get => _plantType;
		set {
			_plantType = value;

			/// TO DO: Set the sprite/image of this plant when its plant type is updated
		}
	}

	/// <summary>
	/// The position of this plant in the garden
	/// </summary>
	public Vector2Int Position {
		get {
			return _position;
		}
		set {
			_position = value;
			transform.localPosition = new Vector3(_position.x, 0f, _position.y);
		}
	}

	private void Awake ( ) {
		garden = FindObjectOfType<Garden>( );
	}

	/// <summary>
	/// Initialize this plant right after it has been created in the garden. Everything in this function needs to be called before Awake() but after it is instantiated in the garden.
	/// </summary>
	public void Initialize ( ) {
		// Make sure the plants are always facing towards the camera
		transform.LookAt(-Camera.main.transform.position + transform.localPosition);
		transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
	}

	/// <summary>
	/// Get a list of specific plants that are surrounding this plant within a certain radius
	/// </summary>
	/// <param name="radius">The radius around this plant to check for other plants</param>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final surrounding plants list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final surrounding plants list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>A list of all the surrounding plant objects around this plant that match the exclusive and excluded plant types</returns>
	public List<Plant> GetSurroundingPlants (int radius, List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		List<Plant> surroundingPlants = new List<Plant>( );

		// Loop through all plants that are surrounding this plant within a certain radius
		for (int x = -radius; x <= radius; x++) {
			for (int y = -radius; y <= radius; y++) {
				int checkX = Position.x + x;
				int checkY = Position.y + y;

				// If the position to check is not within the bounds of the garden, then continue to the next position
				// Also continue to the next position if the current plant being checked is this plant
				if (!garden.IsPositionWithinGarden(checkX, checkY) || (x == 0 && y == 0)) {
					continue;
				}

				// Get a reference to the plant at the current position being checked
				Plant plant = garden.PlayerData.Grid[checkX, checkY];

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
	/// Count how many plants of a specific type are around this plant within a certain radius
	/// </summary>
	/// <param name="radius">The radius around this plant to check for other plants</param>
	/// <param name="exclusivePlantTypes">All plant types within this list will exclusively be added to the final surrounding plants list. If this parameter is left null, all plants of any plant type will be added</param>
	/// <param name="excludedPlantTypes">All plant types within this list will never be added to the final surrounding plants list. If this parameter is left null, no plant types will be excluded</param>
	/// <returns>The number of plants around this plant that match the exclusive and excluded plant types</returns>
	public int CountSurroundingPlants (int radius, List<PlantType> exclusivePlantTypes = null, List<PlantType> excludedPlantTypes = null) {
		return GetSurroundingPlants(radius, exclusivePlantTypes, excludedPlantTypes).Count;
	}
}
