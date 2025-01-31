using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds all of the players data that needs to travel between scenes. This data will be stored in a single scriptable object within the project that can be referenced within the code
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject {
	[Header("Properties")]
	[SerializeField] private int gardenSize;
	[SerializeField] public int playerMoney = 100;

	/// <summary>
	/// The size of the garden (both width and height)
	/// </summary>
	public int GardenSize => gardenSize;

	/// <summary>
	/// The grid of garden placeables that represent the layout of the garden
	/// </summary>
	public GardenPlaceable[ , ] Garden { get; private set; }

	// https://gamedev.stackexchange.com/questions/188224/scriptableobjects-events-execution-order
	private void OnEnable ( ) {
		Garden = new Plant[gardenSize, gardenSize];
	}
}
