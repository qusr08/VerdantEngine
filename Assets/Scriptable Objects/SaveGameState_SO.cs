
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "SaveGameState_SO", menuName = "ScriptableObjects/SaveGameState_SO", order = 2)]

public class SaveGameState_SO : ScriptableObject
{
    public GardenPlaceable[,] SavedGardenState;

}
