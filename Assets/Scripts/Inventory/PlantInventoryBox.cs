using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
