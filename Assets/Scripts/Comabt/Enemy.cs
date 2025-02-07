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

    public bool attacksAreRandom;
    [HideInInspector] public CombatManager manager;
    void Start()
    {
        maxHealth = health;
        currentAttack = attacks[0];
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void attacked(Part_SO incoingAttack)
    {
        health -= incoingAttack.damage;
        manager.combatUIManager.SetHealth(this);
        Debug.Log("Ouch, i just took " + incoingAttack.damage + ". Now I have " + health + " health");
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
    public void MarkMapBeforeAttack()
    {
        currentAttack = attacks[0];
        int randomAim = UnityEngine.Random.Range(0, manager.garden.PlayerData.GardenSize);
        currentAttack.currentAim = new List<Vector2>();

        if (currentAttack.lineAttackIsVertical)
        {

            for (int i = 0; i < manager.garden.PlayerData.GardenSize; i++)
            {
                currentAttack.currentAim.Add(new Vector2(randomAim, i));
            }
        }
        else
        {
            for (int i = 0; i < manager.garden.PlayerData.GardenSize; i++)
            {
                currentAttack.currentAim.Add(new Vector2(i, randomAim));
            }
        }
    }
}
