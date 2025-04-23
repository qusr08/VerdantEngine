
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "SaveGameState_SO", menuName = "ScriptableObjects/SaveGameState_SO", order = 2)]

public class SaveGameState_SO : ScriptableObject
{
    public GardenPlaceable[,] SavedGardenState =  new GardenPlaceable[6,6];
    public List <PlantType>PlacedPlants;
    public List <ArtifactType> PlacedArtifacts;
    public List<Enemy> EnemiesStates;
    public int Health;
    public int energy;
    public int actions;
}
