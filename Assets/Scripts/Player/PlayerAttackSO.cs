
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public enum AttackType {
	HEAL,
	ELECTRIC,
	FIRE,
	POISON,
	FROST
}

public enum PlayerTargetingType {
	SELF,
	GARDEN,
	TARGET,
	ALL
}

[CreateAssetMenu(fileName = "MechPart", menuName = "ScriptableObjects/MechPart", order = 1)]
public class PlayerAttackSO : ScriptableObject {
	[SerializeField] private int _damage;
	[SerializeField] private int _manaCost;
	[SerializeField] private string _description;
	[SerializeField] private string _name;
	[SerializeField] private AttackType _attackType;
	[SerializeField] private PlayerTargetingType _playerTargetingType;
	[SerializeField] private int _targetNumber;
	[SerializeField] private int _maxCooldown;
	[SerializeField] private Sprite _icon;

	/// <summary>
	/// The damage of this mech part's attack
	/// </summary>
	public int Damage => _damage;

	/// <summary>
	/// The mana cost of this mech part's attack
	/// </summary>
	public int ManaCost => _manaCost;

	/// <summary>
	/// This mech part's description text
	/// </summary>
	public string Description => _description;

	/// <summary>
	/// The name of this mech part's attack
	/// </summary>
	public string Name => _name;

	/// <summary>
	/// This mech part's type of attack
	/// </summary>
	public AttackType AttackType => _attackType;

	/// <summary>
	/// This mech part's targeting type
	/// </summary>
	public PlayerTargetingType PlayerTargetingType => _playerTargetingType;

	/// <summary>
	/// How many targets this mech part can have
	/// </summary>
	public int TargetNumber => _targetNumber;

	/// <summary>
	/// The max cooldown of this mech part
	/// </summary>
	public int MaxCooldown => _maxCooldown;

	/// <summary>
	/// The sprite icon of this mech part
	/// </summary>
	public Sprite Icon => _icon;

	/// <summary>
	/// The current cooldown of this mech part
	/// </summary>
	public int Cooldown;
}


