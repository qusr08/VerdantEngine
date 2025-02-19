using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empoweroot : Plant {

    /// <summary>
	/// When the Empoweroot is adjacent to 3 other plants, all weapons deal +1 damage
	/// </summary>
    bool damageAdded = false;
    public override void OnGardenUpdated()
    {
        if(CountSurroundingPlants(1) >= 3)
        {
            if (!damageAdded) {
                Debug.Log("Player began receiving +1 Damage from " + Position);
                damageAdded = true;
            }
        } else
        {
            if (damageAdded) {
                Debug.Log("Player stopped receiving +1 Damage from " + Position);
            }
            damageAdded = false;
        }
    }
}
