using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFlower : Plant {

    /// <summary>
	/// Generates 1 energy each turn
	/// </summary>
    public override void OnTurnStart()
    {
        Debug.Log("Generate 1 Energy");
        FindObjectOfType<GardenManager>().PlayerData.Energy += 1;
    }
}
