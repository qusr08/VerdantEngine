using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldingShrub : Plant {
	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated( );

		// All adjacent plants get +1 shield
		RemoveModifiersFromEffectedGardenPlaceables( );
		foreach (Plant plant in GetSurroundingPlants(1)) {
			effectedGardenPlaceables.Add(plant);
			plant.ShieldStat.AddModifier(1, this);
		}
	}
}
