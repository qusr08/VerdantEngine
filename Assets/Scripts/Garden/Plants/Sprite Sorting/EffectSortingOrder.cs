using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSortingOrder : MonoBehaviour
{
    [SerializeField] SpriteRenderer effect;
    [SerializeField] SpriteRenderer parentSprite;

    private void Start()
    {
        parentSprite = transform.parent.GetComponentInChildren<SpriteRenderer>();
    }
    public void SortEffectSprites(int order)
    {
        effect.sortingOrder = parentSprite.sortingOrder + order;
        Debug.Log(parentSprite.sortingOrder);
    }
}
