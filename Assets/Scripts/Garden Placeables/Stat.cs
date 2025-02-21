using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat {
	public delegate void OnZero ( );
	public OnZero WhenZero;

	/// <summary>
	/// The current value of this stat
	/// </summary>
	public int CurrentValue {
		get => _currentValue + totalModifier;
		set {
			_currentValue = Mathf.Min(_currentValue, MaxValue);
			
			if (CurrentValue <= 0) {
				WhenZero?.Invoke( );
			}
		}
	}
	private int _currentValue;

	/// <summary>
	/// The max value of this stat
	/// </summary>
	public int MaxValue {
		get => _maxValue;
		set {
			_maxValue = value;

			// Make sure the current value always stays under the max value
			CurrentValue = _currentValue;
		}

	}
	private int _maxValue;

	private List<StatModifier> modifiers;
	private int totalModifier;

	public Stat (int currentValue, int maxValue = 99999999) {
		CurrentValue = currentValue;
		MaxValue = maxValue;

		modifiers = new List<StatModifier>( );
	}

	/// <summary>
	/// Add a stat modifier to this stat
	/// </summary>
	/// <param name="value">The value of the modifier</param>
	/// <param name="fromGardenPlaceable">The garden placeable that the modifier came from</param>
	public void SetModifier (int value, GardenPlaceable fromGardenPlaceable) {
		// Need to remove the stat modifier to make sure stat modifiers are not added multiple times
		RemoveModifier(fromGardenPlaceable);

		// Add the new modifier to the modifier list
		StatModifier newModifier = new StatModifier(value, fromGardenPlaceable);
		totalModifier += newModifier.Value;
		modifiers.Add(newModifier);
	}

	/// <summary>
	/// Remove a stat modifier from this stat
	/// </summary>
	/// <param name="fromGardenPlaceable">The garden placeable to check for when removing modifiers</param>
	public void RemoveModifier (GardenPlaceable fromGardenPlaceable) {
		// Loop through all modifiers to check and see if a modifier from the new garden placeable is already in the modifier list
		for (int i = modifiers.Count - 1; i >= 0; i--) {
			// Remove all modifiers that reference the new garden placeable
			if (modifiers[i].FromGardenPlaceable == fromGardenPlaceable) {
				totalModifier -= modifiers[i].Value;
				modifiers.RemoveAt(i);
				return;
			}
		}
	}
}
