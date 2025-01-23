using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The highest abstracted class for all objects that will be able to be placed in the garden. This includes artifacts and plants (so far)
/// </summary>
public class GardenPlaceable : MonoBehaviour {
	[Header("References - GardenPlaceable")]
	[SerializeField] protected GardenManager gardenManager;
	[Header("Properties - GardenPlaceable")]
	[SerializeField] private Vector2Int _position;

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
		gardenManager = FindObjectOfType<GardenManager>( );
	}

	/// <summary>
	/// Initialize this plant right after it has been created in the garden. Everything in this function needs to be called before Awake() but after it is instantiated in the garden.
	/// </summary>
	/// <param name="position">The position to set the the placeable to</param>
	public void Initialize (Vector2Int position) {
		Position = position;

		// Make sure the plants are always facing towards the camera
		transform.LookAt(-Camera.main.transform.position + transform.localPosition);
		transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
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
				Plant plant = gardenManager.PlayerData.Garden[checkX, checkY] as Plant;

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
				Artifact artifact = gardenManager.PlayerData.Garden[checkX, checkY] as Artifact;

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
}
