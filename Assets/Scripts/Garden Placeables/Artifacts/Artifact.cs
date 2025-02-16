using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArtifactType {
	NONE
}

/// <summary>
/// This class holds all data for each artifact that is placed in the garden
/// </summary>
public abstract class Artifact : GardenPlaceable {
	[Header("Properties - Artifact")]
	[SerializeField] private ArtifactType _artifactType;

	/// <summary>
	/// The type of this artifact
	/// </summary>
	public ArtifactType ArtifactType => _artifactType;
}
