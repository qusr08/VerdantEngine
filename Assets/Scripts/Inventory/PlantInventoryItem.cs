using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInventoryItem : InventoryItem {
	[Header("Properties - PlantInventoryItem")]
	[SerializeField] private PlantType _plantType;

	/// <summary>
	/// The plant type of this inventory item
	/// </summary>
	public PlantType PlantType { get => _plantType; set => _plantType = value; }
}
