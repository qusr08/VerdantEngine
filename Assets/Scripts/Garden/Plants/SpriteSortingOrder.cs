using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Assigning sorting orders to plant sprites based on position on the garden grid.
/// </summary>
public class SpriteSortingOrder : MonoBehaviour
{
    public int x;
    public int y;

    public int addToSortOrder;
    public int prevAddToSortOrder;

    List<string> spriteIDs = new List<string>();
    public void SortSprites(int x, int y)
    {
        this.x = x; 
        this.y = y;

        prevAddToSortOrder = addToSortOrder;
        addToSortOrder = x + (5 - y) + 1;

        foreach(SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
        {
            string spriteID = sprite.name + " " + x + y + " order : " + sprite.sortingOrder;

            if (!spriteIDs.Contains(spriteID))
            {
                //sprite.sortingOrder = sprite.sortingOrder - prevAddToSortOrder + addToSortOrder;

                if (prevAddToSortOrder != 0)
                {
                    sprite.sortingOrder = sprite.sortingOrder / prevAddToSortOrder;
                }
                sprite.sortingOrder = sprite.sortingOrder * addToSortOrder;

                spriteID = sprite.name + " " + x + y + " order : " + sprite.sortingOrder;
                spriteIDs.Add(spriteID);
            }
            
        }
    }
}
