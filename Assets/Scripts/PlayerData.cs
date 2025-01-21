using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject {
	[Header("Properties")]
	[SerializeField] private int gardenSize;

	/// <summary>
	/// The size of the garden (both width and height)
	/// </summary>
	public int GardenSize => gardenSize;

	/// <summary>
	/// The grid of plants that represent the layout of the garden
	/// </summary>
	public Plant[ , ] Garden { get; private set; }

	// https://gamedev.stackexchange.com/questions/188224/scriptableobjects-events-execution-order
	private void OnEnable ( ) {
		Garden = new Plant[gardenSize, gardenSize];
	}
}
