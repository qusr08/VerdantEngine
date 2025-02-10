using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HardyHedge : Plant {

    bool gained = false;
    public override void OnGardenUpdated()
    {
        List<PlantType> exclusivePlantTypes = new List<PlantType>();
        exclusivePlantTypes.Add(PlantType.HARDY_HEDGE);
        List<Plant> plants = GetSurroundingPlants(1, exclusivePlantTypes);

        if (plants.Count > 0 && !gained)
        {
            gained = true;
           // Debug.Log("I Hardy Gain " + Position);
            Health += plants.Count;
        }
    }
}
