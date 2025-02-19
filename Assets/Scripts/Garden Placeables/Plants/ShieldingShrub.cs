using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldingShrub : Plant {
    /// <summary>
    /// All adjacent plants gain +1 max HP
    /// </summary>

    List<Plant> affectedPlants = new List<Plant>();

    public override void OnGardenUpdated()
    {
        List<Plant> surroundingPlants = GetSurroundingPlants(1);
        List<Plant> plantsEnteringEffect = new List<Plant>();
        List<Plant> plantsKeepingEffect = new List<Plant>();
        List<Plant> plantsLeavingEffect = new List<Plant>(affectedPlants);

        foreach (Plant plant in surroundingPlants)
        {
            if (affectedPlants.Contains(plant)) {
                plantsKeepingEffect.Add(plant);
            } else {
                plantsEnteringEffect.Add(plant);
            }
        }

        foreach (Plant plant in plantsKeepingEffect) plantsLeavingEffect.Remove(plant);

        // If plant is entering the effect, add 1 max health
        foreach (Plant plant in plantsEnteringEffect) {
            plant.MaxHealth += 1;
        }

        // If plant is exiting the effect, lose 1 max health
        foreach (Plant plant in plantsLeavingEffect) {
            plant.MaxHealth -= 1;
        }

        // Set affectedPlants to the newly entered plants + kept plants
        affectedPlants = new List<Plant>();
        foreach (Plant plant in plantsEnteringEffect) {
            affectedPlants.Add(plant);
        }
        foreach (Plant plant in plantsKeepingEffect) {
            affectedPlants.Add(plant);
        }
    }
}
