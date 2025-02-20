using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArtifactInventoryBox : InventoryBox {
	[Header("Properties - ArtifactInventoryItem")]
	[SerializeField] private ArtifactType _artifactType;

	/// <summary>
	/// The artifact type of this inventory item
	/// </summary>
	public ArtifactType ArtifactType {
		get => _artifactType;
		set {
			_artifactType = value;

			// Set the artifact prefab based on the plant type
			Prefab = gardenManager.ArtifactPrefabs[_artifactType];

			// Update the inventory image with the prefab's sprite
			image.sprite = Prefab.GetComponent<GardenPlaceable>( ).InventorySprite;
		}
	}

	public override void OnPointerUp (PointerEventData eventData) {
		// Reset the mouse image sprite
		inventory.MouseImage.gameObject.SetActive(false);
		inventory.MouseImage.sprite = null;

		// If there is no garden tile selected, then do nothing
		if (inventory.SelectedGardenTile == null) {
			return;
		}

		// If the selected garden tile already has something on it, then do nothing
		if (inventory.SelectedGardenTile.GardenPlaceable != null) {
			return;
		}

		// Place the artifact on the garden
		gardenManager.PlaceArtifact(ArtifactType, inventory.SelectedGardenTile.Position.x, inventory.SelectedGardenTile.Position.y);
		Amount--;
	}
}
