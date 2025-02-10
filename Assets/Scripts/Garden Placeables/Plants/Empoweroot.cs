using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empoweroot : Plant {
    bool damageAdded = false;
    public override void OnGardenUpdated()
    {
        base.OnGardenUpdated();
        if(CountSurroundingPlants(1) >= 3 && !damageAdded)
        {
            damageAdded = true;
            Debug.Log("+1 Damage from " + Position);
        }
    }

}
