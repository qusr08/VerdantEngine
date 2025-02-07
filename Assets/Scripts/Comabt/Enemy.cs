using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHealth;
    public int health;
    // Start is called before the first frame update
    public List<EnemyAttack_SO> attacks;

    public Sprite icon;

    public bool attacksAreRandom;
    void Start()
    {
        MaxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void attacked(Part_SO incoingAttack)
    {
        health -= incoingAttack.damage;
        Debug.Log("Ouch, i just took " + incoingAttack.damage + ". Now I have " + health + " health");
    }
    public EnemyAttack_SO PlayTurn()
    {
        return attacks[0];
    }
}
