using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireFlytrap : Plant
{
    public override void OnTurnEnd()
    {
        foreach (Plant plant in GetSurroundingPlants(1))
        {
            Heal(1);
            plant.TakeDamage(null, 1);
        }
    }
}
