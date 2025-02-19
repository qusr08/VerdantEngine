using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HardyHedge : Plant {
    /// <summary>
	/// Gains +1 max HP if next to at least one other Hardy Hedge
	/// </summary>
    bool gained = false;
    uint gainedFromID = 0;
    public override void OnGardenUpdated()
    {
        List<PlantType> exclusivePlantTypes = new List<PlantType>();
        exclusivePlantTypes.Add(PlantType.HARDY_HEDGE); //Check only for neighboring Hardy Hedges
        List<Plant> plants = GetSurroundingPlants(1, exclusivePlantTypes);

        if (plants.Count > 0)
        {
            gainedFromID = plants[0].Id;

            if (!gained) {
                gained = true;
                MaxHealth += 1;
            }
        }
        else
        {
            gainedFromID = 0;
            if (gained) {
                MaxHealth -= 1;
                gained = false;
            }
        }
        
    }
}
