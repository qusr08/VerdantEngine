using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantType {
	NONE, POWER_FLOWER, SHIELDING_SHRUB, HARDY_HEDGE, EMPOWEROOT
}

/// <summary>
/// This class holds all data for each plant that is placed in the garden
/// </summary>
public abstract class Plant : GardenPlaceable {
	[Header("Properties - Plant")]
	[SerializeField] private PlantType _plantType;

	/// <summary>
	/// The type of this plant
	/// </summary>
	public PlantType PlantType => _plantType;
}
