using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empoweroot : Plant {
    public override void OnGardenUpdated()
    {
        base.OnGardenUpdated();
        if(CountSurroundingPlants(1) >= 3)
        {
            Debug.Log("+1 Damage from " + Position);
        }
    }

}
