using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomShroomColony : Plant
{
    int counter = 0;

	public override void OnGardenUpdated ( ) {
		base.OnGardenUpdated( );

        // Update the effected tiles for this plant
        EffectedTiles.Clear( );
		foreach (Plant plant in gardenManager.Plants) {
            EffectedTiles.Add(plant.GardenTile);
		}
		foreach (Artifact artifact in gardenManager.Artifacts) {
			EffectedTiles.Add(artifact.GardenTile);
		}
	}

	public override void OnTurnEnd()
    {
        if (counter == 1)
        { counter = 0; 
        }
        else
        {
            counter++;
            return;
        }
        
        base.OnTurnEnd();
        gardenManager.combatManager.inventory.AddPlant(PlantType.BOOM_SHROOM);
    }
    public override void OnKilled()
    {
        gardenManager.Plants.Remove(this);

        for (int i = gardenManager.Plants.Count-1; i >= 0; i--)
        {
            if (i > gardenManager.Plants.Count - 1)
                continue;
            gardenManager.Plants[i].TakeDamage(null, 1);
        }
        for (int i = gardenManager.Artifacts.Count - 1; i >= 0; i--)

        {
            if (i > gardenManager.Artifacts.Count - 1)
                continue;
            gardenManager.Artifacts[i].TakeDamage(null, 1);

        }
        
        gardenManager.playerDataManager.CurrentHealth--;
        combatManager.damageAllEnemies(1);
        base.OnKilled();

    }

}
