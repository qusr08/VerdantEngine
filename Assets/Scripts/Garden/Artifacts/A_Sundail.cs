using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Sundail : Artifact
{
    int counter = 0;
    public Sprite[] dialPhases; 
    // Start is called before the first frame update
    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        counter++;
        if(counter == 3)
        {
            counter = 0;
            foreach (Plant plant in gardenManager.Plants)
            {
                plant.OnTurnEnd();
            }
        }
        flowerVisuals.GetComponent<SpriteRenderer>().sprite = dialPhases[counter];
    }
}
