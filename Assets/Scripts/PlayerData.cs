using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int gardenSize;
    public int cuurentHealth;
    public int maxHealth;
    public int money;

    /// <summary>
    /// The size of the garden (both width and height)
    /// </summary>
    public int GardenSize => gardenSize;

    /// <summary>
    /// The grid of garden placeables that represent the layout of the garden
    /// </summary>
    public GardenTile[,] Garden { get; private set; }

    public List<Part_SO> currentParts;



    // https://gamedev.stackexchange.com/questions/188224/scriptableobjects-events-execution-order
    private void OnEnable()
    {
        Garden = new GardenTile[gardenSize, gardenSize];
    }

}
