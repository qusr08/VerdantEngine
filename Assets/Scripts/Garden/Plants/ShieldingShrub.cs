using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldingShrub : Plant {
	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated( );

		// All adjacent plants gain +1 HP
		List<Plant> plants = GetSurroundingPlants(1);
		foreach (Plant plant in plants) {
			plant.HealthStat.SetModifier(1, this);
		}
	}
}
