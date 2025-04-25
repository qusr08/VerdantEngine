using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Gnomiet : Artifact
{
    List<ArtifactType> gnomio = new List<ArtifactType>() { ArtifactType.GNOMEO };
    List<Artifact> surroundingArtifact;
    public override void OnTurnEnd()
    {

        base.OnMoved();
        surroundingArtifact = GetSurroundingArtifacts(1, gnomio);
        if (surroundingArtifact != null && surroundingArtifact.Count > 0)
        {
            combatManager.damageAllEnemies(1);

           
        }
        else
        {
            foreach (Plant plant in GetSurroundingPlants(1))
            {
                plant.Heal(1);
            }
        }
    }
}
