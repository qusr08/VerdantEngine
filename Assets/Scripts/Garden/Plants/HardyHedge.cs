using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HardyHedge : Plant {
	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated( );

		// Gains +1 HP if next to another Hardy Hedge
		RemoveModifiersFromEffectedGardenPlaceables( );
		List<Plant> surroundingHardyHedges = GetSurroundingPlants(1, new List<PlantType>( ) { PlantType.HARDY_HEDGE });
		Debug.Log(surroundingHardyHedges.Count);
		foreach (Plant hardyHedge in surroundingHardyHedges) {
			effectedGardenPlaceables.Add(hardyHedge);
			hardyHedge.HealthStat.AddModifier(1, this);
		}
	}
}
