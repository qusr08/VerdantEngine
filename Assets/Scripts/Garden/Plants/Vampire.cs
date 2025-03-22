using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Plant
{
    public override void OnTurnEnd()
    {
        foreach (Plant plant in GetSurroundingPlants(1))
        {
            Heal(1);
            plant.TakeDamage( 1);
        }
    }
}
