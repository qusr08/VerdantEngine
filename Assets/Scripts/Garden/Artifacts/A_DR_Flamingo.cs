using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_DR_Flamingo : Artifact
{
    public override void ActivateAction(int Value)
    {
        gardenManager.combatManager.damageEnemy(Value);
        Debug.Log("Dr Flamingo dealt " + Value + " damage after a plant got healed");
    }
}
