using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Compost_Bin : Artifact
{
    public override void ActivateAction()
    {
        gardenManager.combatManager.playerCombatManager.UpdateEnergy(5);
        Debug.Log("Flower died so the compost gives you an extra energy");
    }

}
