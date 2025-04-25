using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Gnomeo : Artifact
{
    List<ArtifactType> gnomiet = new List<ArtifactType>() { ArtifactType.GNOMIET };
    List<Artifact> surroundingArtifact;
   
    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        surroundingArtifact = GetSurroundingArtifacts(1, gnomiet);
        if (surroundingArtifact != null && surroundingArtifact.Count > 0)
        {
            foreach (Plant plant in (gardenManager.Plants))
            {
                plant.Heal(1);
            }
        }
        else
        {
            combatManager.damageEnemy(1);
        }
    }
}
