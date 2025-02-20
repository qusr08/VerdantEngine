using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantInventoryBox : InventoryBox {
	[Header("Properties - PlantInventoryItem")]
	[SerializeField] private PlantType _plantType;

	/// <summary>
	/// The plant type of this inventory item
	/// </summary>
	public PlantType PlantType {
		get => _plantType;
		set {
			_plantType = value;

			// Set the plant prefab based on the plant type
			Prefab = gardenManager.PlantPrefabs[_plantType];

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

		// Place the selected plant type in the garden
		gardenManager.PlacePlant(PlantType, inventory.SelectedGardenTile.Position.x, inventory.SelectedGardenTile.Position.y);
		Amount--;
	}
}
