using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public Sprite icon;
    public int health;
    // Start is called before the first frame update
    public List<EnemyAttack_SO> attacks;
    private EnemyAttack_SO currentAttack;
    [HideInInspector] public int enemyID;
    [HideInInspector] public List<GardenTile> currentAim;
    [HideInInspector] public List<GardenTile> FinalAim;
    public bool attacksAreRandom;
    [HideInInspector] public CombatManager manager;
    public GameObject iconHolder;
    void Start()
    {
        maxHealth = health;
        currentAttack = attacks[0];
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void attacked(Part_SO incoingAttack, Player_Combat_Manager player)
    {
        
        int totalDamage  = incoingAttack.damage+ player.GetAddedDamage();
        health -= totalDamage;
        manager.combatUIManager.SetHealth(this);
        Debug.Log("Ouch, i just took " + totalDamage + player.GetAddedDamage() + ". Now I have " + health + " health");
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        manager.EnemyDied(this);
    }


    public EnemyAttack_SO PlayTurn()
    {
        return currentAttack;
    }
    public void ChooseAttack()
    {
        currentAttack = attacks[0];
        MarkMapBeforeAttack();
    }
    public void MarkMapBeforeAttack()
    {
        if(currentAttack==null)
        {
            ChooseAttack();
        }
        if (FinalAim == null)
        {
            FinalAim = new List<GardenTile>();
        }
        else
            UnmarkTiles();


        int randomAim = UnityEngine.Random.Range(0, manager.garden.PlayerData.GardenSize);

        if (currentAttack.lineAttackIsVertical)
        {
            
            for (int i = manager.garden.PlayerData.GardenSize-1; i >= 0; i--)
            {
                currentAim.Add(manager.player.Garden[randomAim, i]);
            }
        }
        else
        {
            for (int i = manager.garden.PlayerData.GardenSize - 1; i >= 0; i--)
            {
                currentAim.Add(manager.player.Garden[ i, randomAim]);
            }
        }

        //Set Icon for map
        iconHolder.transform.SetParent(currentAim[0].transform);
        iconHolder.transform.localPosition = Vector3.zero;
        
        //Set up the final aim for marking, stopping if there is a collision 

        foreach (GardenTile tile in currentAim)
        {
            FinalAim.Add(tile);
            tile.IsAttacked = true;
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
        FinalAim = new List<GardenTile>();
        currentAim = new List<GardenTile>();

    }
}
