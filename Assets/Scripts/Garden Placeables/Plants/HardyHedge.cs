using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HardyHedge : Plant {
	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated();
		// Gains +1 HP if next to another Hardy Hedge
		int hardyHedgeCount = CountSurroundingPlants(1, new List<PlantType>( ) { PlantType.HARDY_HEDGE });
		HealthStat.SetModifier(hardyHedgeCount, this);
	}
}
