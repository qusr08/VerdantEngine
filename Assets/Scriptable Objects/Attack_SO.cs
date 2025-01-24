
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AttackType
{
    Heal,
    Electric,
    Fire,
    Posion,
    Frost
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Attack_SO", order = 1)]
public class Attack_SO : ScriptableObject
{
    public int damage;

    public int manaCost;
    public string attackText;
    public string attackName;


    public AttackType type;

    public bool isCoolDown;

    public int coolDown;

    public Sprite icon;
}

