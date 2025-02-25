using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatPreset", menuName = "ScriptableObjects/CombatPreset", order = 1)]
public class CombatPresetSO : ScriptableObject {
	[SerializeField] private List<GameObject> _enemyPrefabs;
	[SerializeField] private bool _isEliteRewards;

	/// <summary>
	/// A list of all the enemies that will be spawned in this combat
	/// </summary>
	public List<GameObject> EnemyPrefabs => _enemyPrefabs;

	/// <summary>
	/// Whether or not the rewards from this combat is elite
	/// </summary>
	public bool IsEliteRewards => _isEliteRewards;
}
