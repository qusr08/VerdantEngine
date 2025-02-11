using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HardyHedge : Plant {
    /// <summary>
	/// Gains +1 HP if next to another Hardy Hedge
	/// </summary>
    bool gained = false; 
    public override void OnGardenUpdated()
    {
        List<PlantType> exclusivePlantTypes = new List<PlantType>();
        exclusivePlantTypes.Add(PlantType.HARDY_HEDGE); //Check only for neighboring Hardy Hedges
        List<Plant> plants = GetSurroundingPlants(1, exclusivePlantTypes);

        if (plants.Count > 0 && !gained)
        {
            gained = true;
            Health += plants.Count;
        }
    }
}
