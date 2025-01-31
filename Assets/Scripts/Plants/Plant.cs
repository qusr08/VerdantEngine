using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantType {
	NONE, FLOWER, CACTUS
}

/// <summary>
/// This class holds all data for each plant that is placed in the garden
/// </summary>
public class Plant : GardenPlaceable {
	[Header("Properties - Plant")]
	[SerializeField] private PlantType _plantType;
	[SerializeField] public int cost;
	/// <summary>
	/// The type of this plant
	/// </summary>
	public PlantType PlantType => _plantType;

    private void Start()
    {
		PlantSetup();
    }
	private void PlantSetup()
    {
        switch (PlantType)
        {
			case PlantType.NONE:
				cost = 0;
				break;

			case PlantType.FLOWER:
				cost = 5;
				break;

			case PlantType.CACTUS:
				cost = 10;
				break;
        }
    }
}
