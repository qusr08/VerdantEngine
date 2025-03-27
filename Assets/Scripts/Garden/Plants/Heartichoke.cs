using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartichoke : Plant
{
	public override void OnTurnEnd()
	{
        foreach (Plant plant in GetSurroundingPlants(1))
        {
            plant.Heal(1);
        } 
	}
    public override void OnGardenUpdated()
    {
        base.OnGardenUpdated();

        GardenTile tile = GetComponentInParent<GardenTile>();
        gameObject.GetComponentInChildren<SpriteSortingOrder>().SortSprites(tile.Position.x, tile.Position.y); //Setting the sorting order of each sprite based on tile position
    }
}
