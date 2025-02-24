using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier {
	/// <summary>
	/// The value of this stat modifier
	/// </summary>
	public int Value { get; private set; }

	/// <summary>
	/// The garden placeable that this stat modifier came from
	/// </summary>
	public GardenPlaceable FromGardenPlaceable { get ; private set; }

	public StatModifier (int value, GardenPlaceable fromGardenPlaceable) {
		Value = value;
		FromGardenPlaceable = fromGardenPlaceable;
	}
}
