using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFlower : Plant {
    public override void OnTurnStart()
    {
        Debug.Log("Generate 1 Energy");
        FindObjectOfType<GardenManager>().PlayerData.Energy += 1;
    }
}
