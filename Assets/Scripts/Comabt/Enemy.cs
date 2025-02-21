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
    [HideInInspector] public int currentCoolDown;
    private bool needNewAttack = true;

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

    public void StartRound()
    {
        
            currentCoolDown--;
        if (attacks.Count > 0 && needNewAttack)
        {
            currentAttack = attacks[0];
            currentCoolDown = currentAttack.maxCoolDown;
            needNewAttack = false;
        }
        if (currentCoolDown == 0)
        { MarkMapBeforeAttack();
            needNewAttack = true;
        }
       
        manager.combatUIManager.SetCoolDown(this);


    }

    public void MarkMapBeforeAttack()
    {
        if ((currentCoolDown!=0))
        {
            return;
        }
        FinalAim.Clear();
        currentAim.Clear();
        if (currentAttack == null)
        {
            currentAttack = attacks[0];
        }

        // Clear previous markings
     //   UnmarkTiles();

        int gardenSize = manager.garden.playerDataManager.GardenSize;
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
            iconHolder.SetActive(true);
            iconHolder.transform.SetParent(currentAim[0].transform);
            if (currentAttack.lineAttackIsVertical)
            {
                iconHolder.transform.localPosition = Vector3.zero + new Vector3(0, 2, -1);
                iconHolder.transform.rotation = new Quaternion(0.506545067f, -0.493368179f, 0.493368179f, -0.506545067f);
            }
            else
            {
                iconHolder.transform.localPosition = Vector3.zero + new Vector3(2, -1, -1);
                iconHolder.transform.rotation = new Quaternion(0.00228309655f, -0.707103133f, 0.707103133f, -0.00228309655f);

            }
        }
        else
        {
            Debug.LogError("Icon holder is missing!");
        }

        // Set up the final aim for marking, stopping if there is a collision
        foreach (GardenTile tile in currentAim)
        {
            tile.IsAttacked = true;
            FinalAim.Add(tile);
            Debug.Log(gameObject.name+" is marking tile as attacked: " + tile.Position);
            if (tile.GardenPlaceable != null)
                break;
        }
    }

    public void UnmarkTiles()
    {
        Debug.LogWarning(gameObject.name + " Unmarked his targets");
        foreach (GardenTile tile in FinalAim)
        {
            tile.IsAttacked = false;
        }
        FinalAim.Clear();
        currentAim.Clear();
        iconHolder.SetActive(false);
    }
}
