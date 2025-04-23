using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarmicKalmia : Plant
{
    public override int TakeDamage(Enemy enemy, int damage)
    {
        int Actualdamage = base.TakeDamage(enemy, damage);

        if(Actualdamage>0)
        {
            gardenManager.combatManager.playerCombatManager.UpdateEnergy(Actualdamage);

        }

        return Actualdamage;
    }
   
}
