using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldingShrub : Plant {
	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated( );

		// All adjacent plants get +1 shield
		foreach (Plant plant in GetSurroundingPlants(1)) {
			effectedGardenPlaceables.Add(plant);
			plant.ShieldStat.AddModifier(1, this);
		}
		
        foreach (Artifact artifact in GetSurroundingArtifacts(1))
        {
            effectedGardenPlaceables.Add(artifact);
            artifact.ShieldStat.AddModifier(1, this);
        }
	}
}
