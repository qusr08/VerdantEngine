using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heartichoke : Plant
{
	public override void OnTurnEnd()
	{
        foreach (Plant plant in GetSurroundingPlants(1))
        {
            plant.Heal(1);
        } 
	}
}
