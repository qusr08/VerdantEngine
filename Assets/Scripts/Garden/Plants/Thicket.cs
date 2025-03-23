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
    public override void OnGardenUpdated()
    {
        base.OnGardenUpdated();

        GardenTile tile = GetComponentInParent<GardenTile>();
        gameObject.GetComponentInChildren<SpriteSortingOrder>().SortSprites(tile.Position.x, tile.Position.y); //Setting the sorting order of each sprite based on tile position
    }
}
