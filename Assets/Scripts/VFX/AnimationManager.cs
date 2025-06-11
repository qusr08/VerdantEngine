using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    public List<Transform> attackedTileTransforms = new List<Transform>();
    public GameObject explosion_prefab;
    public float explosionDelay = 0.2f;

    private void Awake()
    {
        // Singleton logic
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Avoid duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    public void AddExplosionToList(Transform tileTransform)
    {
        if (!attackedTileTransforms.Contains(tileTransform))
        {
            attackedTileTransforms.Add(tileTransform);
        }
    }

    public IEnumerator TriggerExplosions()
    {
        foreach (Transform tileTransform in attackedTileTransforms)
        {
            if (tileTransform != null)
            {
                GameObject explosion = Instantiate(explosion_prefab, tileTransform.position, Quaternion.identity);
                yield return new WaitForSeconds(explosionDelay);
            }
        }
        attackedTileTransforms.Clear(); // Clear the list after triggering explosions
    }
}
