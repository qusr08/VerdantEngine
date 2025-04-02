using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornyThicket : Plant {
    public override int TakeDamage(Enemy enemy, int damage) {   
        // Get the acutal damage dealt to this plant
        int actualDamage = base.TakeDamage(enemy, damage);

        // If the actual damage is still greater than 0, then attack the enemy with equal damage
        if (actualDamage > 0) {
            enemy.Attacked(actualDamage);
		}

        return actualDamage;
    }
}
