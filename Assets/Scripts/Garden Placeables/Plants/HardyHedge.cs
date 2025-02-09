using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HardyHedge : Plant {
    public override void OnGardenUpdated()
    {
        List<PlantType> exclusivePlantTypes = new List<PlantType>();
        exclusivePlantTypes.Add(PlantType.HARDY_HEDGE);
        List<Plant> plants = GetSurroundingPlants(1, exclusivePlantTypes);
        Health += plants.Count;

/*        foreach (var plant in plants)
        {
            Debug.Log(Position.ToString() + plant.Position.ToString());
        }*/


    }
}
