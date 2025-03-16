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
    public override void TakeDamage(int damage)
    {   base.TakeDamage(damage);
        lastAttack = damage;

    }
}
