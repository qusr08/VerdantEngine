using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void OnEnable()
    {
        PlantSetup();
        //OnTurnStart();
    }
	private void PlantSetup()
	{
		switch (PlantType)
		{
            case PlantType.NONE:
                Health = 0;
                break;
            case PlantType.POWER_FLOWER:
                Health = 1;
                break;
            case PlantType.SHIELDING_SHRUB:
                Health = 2;
                break;
            case PlantType.HARDY_HEDGE:
                Health = 3;
                break;
            case PlantType.EMPOWEROOT:
                Health = 1;
                break;
        }
	}
}