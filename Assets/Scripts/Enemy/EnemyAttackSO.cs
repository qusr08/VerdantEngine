using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum EnemyTargetingType {
	LINE,
	SHAPE,
	ALL,
	ALLIES,
	SELF
}

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "ScriptableObjects/EnemyAttack", order = 1)]
public class EnemyAttackSO : ScriptableObject {
	[System.Serializable]
	private class Wrapper<T> { public T[ ] value; }
	private Wrapper<bool>[ ] attackGrid;
	private int attackSize = 5; // Change from static to instance variable

	[SerializeField] private string _name;
	[SerializeField] private EnemyTargetingType _enemyTargetingType;
	[SerializeField] private AttackType _attackType;
	[SerializeField] private int _damage;
	[SerializeField] private bool _isLineAttackHorizental;
	[SerializeField] private int _maxCooldown;

	/// <summary>
	/// The name of this enemy attack
	/// </summary>
	public string Name => _name;

	/// <summary>
	/// The targeting type of this enemy attack
	/// </summary>
	public EnemyTargetingType EnemyTargetingType => _enemyTargetingType;

	/// <summary>
	/// The attack type of this enemy attack
	/// </summary>
	public AttackType AttackType => _attackType;

	/// <summary>
	/// The damage of this enemy attack
	/// </summary>
	public int Damage => _damage;

	/// <summary>
	/// Whether or not this enemy attack goes horizontally across the garden grid
	/// </summary>
	public bool IsLineAttackHorizontal => _isLineAttackHorizental;

	/// <summary>
	/// The max cooldown of this enemy attack
	/// </summary>
	public int MaxCooldown => _maxCooldown;

	private void OnEnable ( ) {
		if (attackGrid == null || attackGrid.Length != attackSize) {
			ResetGrid( );
		}
	}

	public void ResetGrid ( ) {
		attackGrid = new Wrapper<bool>[attackSize];
		for (int i = 0; i < attackSize; i++) {
			attackGrid[i] = new Wrapper<bool>( );
			attackGrid[i].value = new bool[attackSize];
		}
	}
}

// Not working on my computer for some reason - Frankie
/*
#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAttackSO))]
public class EnemyAttackEditor : Editor {
	SerializedProperty attackGridProperty;
	SerializedProperty attackSizeProperty;

	private void OnEnable ( ) {
		attackGridProperty = serializedObject.FindProperty("attackGridProperty");
		attackSizeProperty = serializedObject.FindProperty("attackSizeProperty");
	}

	public override void OnInspectorGUI ( ) {
		base.OnInspectorGUI( );

		serializedObject.Update( );

		EnemyAttackSO script = (EnemyAttackSO) target;

		GUILayout.Label("Attack Grid Size");
		attackSizeProperty.intValue = EditorGUILayout.IntField(attackSizeProperty.intValue);

		if (GUILayout.Button("Reset Grid")) {
			script.ResetGrid( );
		}

		DrawGrid( );

		serializedObject.ApplyModifiedProperties( );
	}

	private void DrawGrid ( ) {
		try {
			if (attackGridProperty.arraySize != attackSizeProperty.intValue) {
				return; // Prevent accessing out-of-bounds elements
			}

			GUILayout.BeginVertical( );
			for (int i = 0; i < attackSizeProperty.intValue; i++) {
				SerializedProperty row = attackGridProperty.GetArrayElementAtIndex(i).FindPropertyRelative("value");

				if (row.arraySize != attackSizeProperty.intValue) {
					continue; // Prevent errors from uninitialized inner arrays
				}

				GUILayout.BeginHorizontal( );
				for (int j = 0; j < attackSizeProperty.intValue; j++) {
					SerializedProperty value = row.GetArrayElementAtIndex(j);
					bool element = value.boolValue;
					if (GUILayout.Button(element ? "X" : "O", GUILayout.MaxWidth(50))) {
						value.boolValue = !value.boolValue;
					}
				}

				GUILayout.EndHorizontal( );
			}

			GUILayout.EndVertical( );
		} catch (System.Exception e) {
			Debug.LogWarning(e);
		}
	}
}
#endif
*/