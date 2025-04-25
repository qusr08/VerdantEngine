using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireFlytrap : Plant
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
            Heal(1);
            plant.TakeDamage(null, 1);
        }
    }
}
