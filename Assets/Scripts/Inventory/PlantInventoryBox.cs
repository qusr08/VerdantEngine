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
		playerDataManager.MouseSprite = null;

		// If there is no garden tile selected, then do nothing
		if (gardenManager.SelectedGardenTile == null) {
			return;
		}

		// If the selected garden tile already has something on it, then do nothing
		if (gardenManager.SelectedGardenTile.GardenPlaceable != null) {
			return;
		}

		// If there are no actions remaining, then do not place a new plant
		if (playerDataManager.CurrentActions <= 0) {
			return;
		}

		// Place the selected plant type in the garden
		gardenManager.PlacePlant(PlantType, gardenManager.SelectedGardenTile.Position.x, gardenManager.SelectedGardenTile.Position.y);
		playerDataManager.CurrentActions--;
		Amount--;
		combatManager.SetEnemyAttackVisuals();
	}
}
