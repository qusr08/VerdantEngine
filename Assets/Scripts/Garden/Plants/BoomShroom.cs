using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomShroom : Plant
{
    public override void OnKilled()
    {
        foreach (Plant plant in GetSurroundingPlants(1))
        {
            plant.TakeDamage(null, 1);
        }
        gardenManager.combatManager.damageEnemy(1);
        //Add anim tirgger here
        base.OnKilled();
        
    }
}
