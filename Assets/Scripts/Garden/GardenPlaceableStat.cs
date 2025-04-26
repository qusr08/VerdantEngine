using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Logic Loop:
- plant gets moved when it has no base health and only modifier health
- plant calls OnGardenUpdate
- modifiers are removed from plant
- plant calls OnKilled

 */

[System.Serializable]
public class GardenPlaceableStat {
	public delegate void StatFunction ( );
	public StatFunction WhenZero;
	public StatFunction OnUpdateCurrentValue;

	/// <summary>
	/// The current value of this stat. This is equal to the base value plus the total modifiers
	/// </summary>
	public int CurrentValue => BaseValue + TotalModifier;

	/// <summary>
	/// The total modifier value of this stat
	/// </summary>
	public int TotalModifier {
		get {
			// Remove the base value from the total modifier when getting it to better show what the final value is
			if (BaseValue < 0) {
				return _totalModifier - BaseValue;
			} else {
				return _totalModifier;
			}
		}
		private set {
			// If the value is being set to the same number, do not update
			if (_totalModifier == value) {
				return;
			}

			_totalModifier = value;

			OnUpdateCurrentValue?.Invoke( );

			// This includes the total modifier when checking against the min value
			// For instance, if the min value was 0, current value was -2, and the total modifier was 3, then this would not be called
			// As soon as the modifier is removed, though, this function would immediately be called
			if (CurrentValue <= 0) {
				WhenZero?.Invoke( );
			}
		}
	}
	private int _totalModifier;

	/// <summary>
	/// The base value of this stat
	/// </summary>
	public int BaseValue {
		get => _baseValue;
		set {
			// If the value is being set to the same number, do not update
			if (_baseValue == value) {
				return;
			}

			_baseValue = value;

			OnUpdateCurrentValue?.Invoke( );

			// This includes the total modifier when checking against the min value
			// For instance, if the min value was 0, current value was -2, and the total modifier was 3, then this would not be called
			// As soon as the modifier is removed, though, this function would immediately be called
			if (CurrentValue <= 0) {
				WhenZero?.Invoke( );
			}
		}
	}
	private int _baseValue;

	private int startingValue;
	private List<GardenPlaceableStatModifier> modifiers;

	public GardenPlaceableStat (int startingValue) {
		this.startingValue = startingValue;
		BaseValue = startingValue;
		
		TotalModifier = 0;
		modifiers = new List<GardenPlaceableStatModifier>( );
		Reset();

    }

	/// <summary>
	/// Reset this stat back to its starting value and remove all modifiers
	/// </summary>
	public void Reset ( ) {
		BaseValue = startingValue;
		modifiers.Clear( );
		TotalModifier = 0;
	}

	/// <summary>
	/// Add a stat modifier to this stat
	/// </summary>
	/// <param name="value">The value of the modifier</param>
	/// <param name="fromGardenPlaceable">The garden placeable that the modifier came from</param>
	public void AddModifier (int value, GardenPlaceable fromGardenPlaceable) {
		// Add the new modifier to the modifier list
		GardenPlaceableStatModifier newModifier = new GardenPlaceableStatModifier(value, fromGardenPlaceable);
		TotalModifier += newModifier.Value;
		modifiers.Add(newModifier);
	}

	/// <summary>
	/// Remove all stat modifiers from a specific garden placeable
	/// </summary>
	/// <param name="fromGardenPlaceable">The garden placeable to check for when removing modifiers</param>
	public void RemoveModifiers (GardenPlaceable fromGardenPlaceable = null) {
		// Loop through all modifiers to check and see if a modifier from the new garden placeable is already in the modifier list
		for (int i = modifiers.Count - 1; i >= 0; i--) {
			// Remove all modifiers that reference the new garden placeable
			if (fromGardenPlaceable == null || modifiers[i].FromGardenPlaceable == fromGardenPlaceable) {
				TotalModifier -= modifiers[i].Value;
				modifiers.RemoveAt(i);
			}
		}
	}
}
