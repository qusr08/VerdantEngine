
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public enum AttackType
{
    Heal,
    Electric,
    Fire,
    Posion,
    Frost
}
public enum TargetingType
{
    
    Self,
    Graden,
    traget,
    all
}

[CreateAssetMenu(fileName = "Part", menuName = "ScriptableObjects/Part", order = 1)]
public class Part_SO : ScriptableObject
{
    public int damage;

    public int manaCost;
    public string attackText;
    public string attackName;


    public AttackType type;
    public TargetingType targetingType;
    public int maxCoolDown;

    public int coolDown;

    public Sprite icon;

    //Used for target attacks
    public int targetNum;

}


