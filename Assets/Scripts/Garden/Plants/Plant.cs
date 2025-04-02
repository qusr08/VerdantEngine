using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum PlantType {
	NONE, POWER_FLOWER, SHIELDING_SHRUB, HARDY_HEDGE, BLAST_BLOOM, HEARTICHOKE, VAMPIRE_FLYTRAP, THORNY_THICKET
}

public enum Rarity { COMMON, UNCOMMON, RARE}

/// <summary>
/// This class holds all data for each plant that is placed in the garden
/// </summary>
public abstract class Plant : GardenPlaceable {
	[Header("Plant")]
	[SerializeField] private PlantType _plantType;
	[SerializeField] private Rarity _rarity;
	/// <summary>
	/// The type of this plant
	/// </summary>
	public PlantType PlantType => _plantType;
	public Rarity Rarity => _rarity;

	public override void OnKilled ( ) {
		base.OnKilled( );

		// Uproot the plant from the garden when it is killed
        gardenManager.UprootPlant(this);
	}
}
