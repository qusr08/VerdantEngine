using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomShroomColony : Plant
{

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        gardenManager.combatManager.playerCombatManager.inventory.AddPlant(PlantType.BOOM_SHROOM);
    }
    public override void OnKilled()
    {
        foreach (Plant item in gardenManager.Plants)
        {
            item.TakeDamage(null, 1);
        }
        foreach (Artifact item in gardenManager.Artifacts)
        {
            item.TakeDamage(null, 1);
        }
        gardenManager.playerDataManager.CurrentHealth--;
        combatManager.damageAllEnemies(1);
    }

}
