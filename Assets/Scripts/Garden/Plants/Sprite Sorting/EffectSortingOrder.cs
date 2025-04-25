using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSortingOrder : MonoBehaviour
{
    [SerializeField] SpriteRenderer effect;
    [SerializeField] SpriteRenderer parentSprite;

    public void SortEffectSprites(int order) {
        if (parentSprite == null) {
			parentSprite = transform.parent.GetComponentInChildren<SpriteRenderer>( );
		}

		effect.sortingOrder = parentSprite.sortingOrder + order;
        Debug.Log(parentSprite.sortingOrder);
    }
}
