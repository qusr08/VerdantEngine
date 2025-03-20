using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thicket : Plant
{
    int lastAttack;
    public override void OnTakeDamage()
    {
        if(LastEnemyWhichDamagedPlaceble)
        {
            LastEnemyWhichDamagedPlaceble.Attacked(lastAttack);
        }

    }
    public override int TakeDamage(int damage)
    {    damage = base.TakeDamage(damage);
        if (damage > 0)
            lastAttack = damage;
        else
        {
            lastAttack = 0;
        }
        return damage;

    }
}
