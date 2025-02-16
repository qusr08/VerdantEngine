using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public Sprite icon;
    public int health;
    public List<EnemyAttack_SO> attacks;
    private EnemyAttack_SO currentAttack;
    [HideInInspector] public int enemyID;
    [HideInInspector] public List<GardenTile> currentAim = new List<GardenTile>();
    [HideInInspector] public List<GardenTile> FinalAim = new List<GardenTile>();
    public bool attacksAreRandom;
    [HideInInspector] public CombatManager manager;
    public GameObject iconHolder;

    void Start()
    {
        maxHealth = health;
        if (attacks.Count > 0)
            currentAttack = attacks[0];
    }

    public void attacked(Part_SO incomingAttack, Player_Combat_Manager player)
    {
        int totalDamage = incomingAttack.damage + player.GetAddedDamage();
        health -= totalDamage;
        manager.combatUIManager.SetHealth(this);
        Debug.Log($"Ouch, I just took {totalDamage}. Now I have {health} health");
        if (health <= 0)
            Die();
    }

    public void Die()
    {
        UnmarkTiles();
        Destroy(iconHolder);
        manager.EnemyDied(this);
    }

    public EnemyAttack_SO PlayTurn()
    {
        return currentAttack;
    }

    public void ChooseAttack()
    {
        if (attacks.Count > 0)
            currentAttack = attacks[0];
        MarkMapBeforeAttack();
    }

    public void MarkMapBeforeAttack()
    {
        if (currentAttack == null)
        {
            currentAttack = attacks[0];
        }

        // Clear previous markings
        UnmarkTiles();

        int gardenSize = manager.garden.PlayerData.GardenSize;
        int randomAim = UnityEngine.Random.Range(0, gardenSize);

        if (currentAttack.lineAttackIsVertical)
        {
            for (int i = gardenSize - 1; i >= 0; i--)
            {
                GardenTile tile = manager.player.Garden[randomAim, i];
                if (tile != null)
                    currentAim.Add(tile);
            }
        }
        else
        {
            for (int i = gardenSize - 1; i >= 0; i--)
            {
                GardenTile tile = manager.player.Garden[i, randomAim];
                if (tile != null)
                    currentAim.Add(tile);
            }
        }

        if (currentAim.Count == 0)
        {
            Debug.LogError("No valid tiles found for marking.");
            return;
        }

        // Set Icon for map
        if (iconHolder != null)
        {
            iconHolder.transform.SetParent(currentAim[0].transform);
            iconHolder.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogError("Icon holder is missing!");
        }

        // Set up the final aim for marking, stopping if there is a collision
        foreach (GardenTile tile in currentAim)
        {
            FinalAim.Add(tile);
            tile.IsAttacked = true;
            Debug.Log(gameObject.name+" is marking tile as attacked: " + tile.Position);
            if (tile.GardenPlaceable != null)
                break;
        }
    }

    public void UnmarkTiles()
    {
        foreach (GardenTile tile in FinalAim)
        {
            tile.IsAttacked = false;
        }
        FinalAim.Clear();
        currentAim.Clear();
    }
}
