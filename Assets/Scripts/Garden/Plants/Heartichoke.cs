using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartichoke : Plant
{
	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated( );

		// Update the effected tiles of this plant
		EffectedTiles.Clear( );
		EffectedTiles.AddRange(GetSurroundingGardenTiles(1));
	}

	public override void OnTurnEnd()
	{
        foreach (Plant plant in GetSurroundingPlants(1))
        {
            plant.Heal(1);
        }
        foreach (Artifact artifact in GetSurroundingArtifacts(1))
        {
            artifact.Heal(1);
        }
    }
}
