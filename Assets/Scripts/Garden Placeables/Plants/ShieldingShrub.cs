using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldingShrub : Plant {
    public override void OnGardenUpdated()
    {
        base.OnGardenUpdated();
        List<Plant> plants = GetSurroundingPlants(1);
        foreach (Plant plant in plants)
        {
            plant.Health += 1;
        }
    }
}
