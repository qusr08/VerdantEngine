using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat_Manager : MonoBehaviour
{
     List<Part_SO> parts;
    GardenManager garden;
    CombatManager combatManager;
     
    public void SetUp( PlayerData data, GardenManager garden, CombatManager combatManager)
    {
        this.combatManager = combatManager;
        this.garden = garden;
        parts = data.currentParts;
    }
    public IEnumerator PlayerTurn()
    {
        foreach (Part_SO part in parts)
        {
            part.coolDown--;
            if (part.coolDown <= 0)
            {
                
                yield return StartCoroutine(WaitForTargeting(part));
                
                part.coolDown = part.maxCoolDown;

                // Fire the part after targeting is complete (or immediately if no targeting needed)
            }
        }
    }

    private IEnumerator WaitForTargeting(Part_SO part)
    {
        bool targetingComplete = false;

        // Trigger the targeting UI or system and wait for confirmation
        StartCoroutine(combatManager.StartTargeting(part, () => targetingComplete = true));

        // Wait until targetingComplete is true
        yield return new WaitUntil(() => targetingComplete);
    }

  



}
