using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	[Header("References - Inventory")]
	[SerializeField] private List<InventoryBox> _inventoryBoxes;
	[SerializeField] private Transform itemBoxesContainer;
	[SerializeField] private GameObject plantInventoryBoxPrefab;
	[SerializeField] private GameObject artifactInventoryBoxPrefab;
	[Space]
	[SerializeField] private GardenTile _selectedGardenTile;
	[SerializeField] private Image mouseImage;
	[SerializeField] private RectTransform mouseImageTransform;
	[SerializeField] private Canvas guiCanvas;

	/// <summary>
	/// A list of all the inventory boxes
	/// </summary>
	public List<InventoryBox> InventoryBoxes { get => _inventoryBoxes; private set => _inventoryBoxes = value; }

	/// <summary>
	/// The garden tile that is currently selected
	/// </summary>
	public GardenTile SelectedGardenTile { get => _selectedGardenTile; set => _selectedGardenTile = value; }

	/// <summary>
	/// A reference to the mouse image that follows the mouse around. This can be used to show what plant is being placed
	/// </summary>
	public Image MouseImage { get => mouseImage; private set => mouseImage = value; }

	private void Start ( ) {
		/// TESTING
		AddPlant(PlantType.POWER_FLOWER);
		AddPlant(PlantType.HARDY_HEDGE);
		AddPlant(PlantType.HARDY_HEDGE);
		RemovePlant(PlantType.HARDY_HEDGE);
	}

	private void Update ( ) {
		// Set the mouse image to match wherever the mouse is
		mouseImageTransform.anchoredPosition = Input.mousePosition / guiCanvas.scaleFactor;
	}

	/// <summary>
	/// Add a plant type to the inventory
	/// </summary>
	/// <param name="plantType">The plant type to add</param>
	public void AddPlant (PlantType plantType) {
		// Loop through all items in the inventory currently
		foreach (InventoryBox box in InventoryBoxes) {
			if (box is not PlantInventoryBox) {
				continue;
			}

			// If the current inventory box is holding a plant and the plant type matches, then just increment the item's amount by 1
			if ((box as PlantInventoryBox).PlantType == plantType) {
				box.Amount += 1;
				return;
			}
		}

		// If no box was found that holds the item, create a new one
		PlantInventoryBox newInventoryBox = Instantiate(plantInventoryBoxPrefab, itemBoxesContainer).GetComponent<PlantInventoryBox>( );
		newInventoryBox.Amount = 1;
		newInventoryBox.PlantType = plantType;
		InventoryBoxes.Add(newInventoryBox);
	}

	/// <summary>
	/// Add an artifact type to the inventory
	/// </summary>
	/// <param name="artifactType">The artifact type to add</param>
	public void AddArtifact (ArtifactType artifactType) {
		// Loop through all items in the inventory currently
		foreach (InventoryBox box in InventoryBoxes) {
			if (box is not ArtifactInventoryBox) {
				continue;
			}

			// If the current inventory box is holding a plant and the artifact type matches, then just increment the item's amount by 1
			if ((box as ArtifactInventoryBox).ArtifactType == artifactType) {
				box.Amount += 1;
				return;
			}
		}

		// If no box was found that holds the item, create a new one
		ArtifactInventoryBox newInventoryBox = Instantiate(artifactInventoryBoxPrefab, itemBoxesContainer).GetComponent<ArtifactInventoryBox>( );
		newInventoryBox.Amount = 1;
		newInventoryBox.ArtifactType = artifactType;
		InventoryBoxes.Add(newInventoryBox);
	}

	/// <summary>
	/// Remove a plant type from the inventory
	/// </summary>
	/// <param name="plantType">The plant type to remove 1 from</param>
	/// <returns>Whether or not the removal was successfull</returns>
	public bool RemovePlant (PlantType plantType) {
		// Loop through all items in the inventory currently
		foreach (InventoryBox box in InventoryBoxes) {
			if (box is not PlantInventoryBox) {
				continue;
			}

			// If the current inventory box is holding a plant and the plant type matches, then just increment the item's amount by 1
			if ((box as PlantInventoryBox).PlantType == plantType) {
				box.Amount -= 1;
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Remove an artifact type from the inventory
	/// </summary>
	/// <param name="artifactType">The artifact type to remove 1 from</param>
	/// <returns>Whether or not the removal was successfull</returns>
	public bool RemoveArtifact (ArtifactType artifactType) {
		// Loop through all items in the inventory currently
		foreach (InventoryBox box in InventoryBoxes) {
			if (box is not ArtifactInventoryBox) {
				continue;
			}

			// If the current inventory box is holding a plant and the artifact type matches, then just increment the item's amount by 1
			if ((box as ArtifactInventoryBox).ArtifactType == artifactType) {
				box.Amount -= 1;
				return true;
			}
		}

		return false;
	}
}
