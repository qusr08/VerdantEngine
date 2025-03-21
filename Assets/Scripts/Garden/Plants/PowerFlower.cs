using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFlower : Plant {
	public override void OnTurnStart ( ) {
		// Generates 1 energy each turn
		Debug.Log("Generate 1 Energy");
		//FindObjectOfType<GardenManager>().PlayerData.Energy += 1;
	}
    public override void OnGardenUpdated()
    {
        base.OnGardenUpdated();

        GardenTile tile = GetComponentInParent<GardenTile>();
        gameObject.GetComponentInChildren<SpriteSortingOrder>().SortSprites(tile.Position.x, tile.Position.y);
    }
}
