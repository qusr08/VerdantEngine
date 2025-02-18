using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactInventoryItem : InventoryItem {
	[Header("Properties - ArtifactInventoryItem")]
	[SerializeField] private ArtifactType _artifactType;

	/// <summary>
	/// The artifact type of this inventory item
	/// </summary>
	public ArtifactType ArtifactType { get => _artifactType; set => _artifactType = value; }
}
