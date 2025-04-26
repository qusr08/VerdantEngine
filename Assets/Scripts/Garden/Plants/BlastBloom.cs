using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastBloom : Plant {
	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated( );

		// Update the effected tiles of this plant
		EffectedTiles.Clear( );
		EffectedTiles.AddRange(GetSurroundingGardenTiles(1));
	}
}
