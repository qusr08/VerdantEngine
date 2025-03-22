using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thicket : Plant
{
    int lastAttack;
    public override void OnTakeDamage()
    {
        base.OnTakeDamage();
        if (LastEnemyWhichDamagedPlaceble!=null)
        {
            LastEnemyWhichDamagedPlaceble.Attacked(lastAttack);
        }
        LastEnemyWhichDamagedPlaceble = null;

    }
    public override int TakeDamage(int damage)
    {    damage = base.TakeDamage(damage);
        if (damage > 0)
            lastAttack = damage;
        else
        {
            LastEnemyWhichDamagedPlaceble = null;
            lastAttack = 0;
        }
        return damage;

    }
}
