using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactInventoryBox : InventoryBox {
	[Header("Properties - ArtifactInventoryItem")]
	[SerializeField] private ArtifactType _artifactType;

	/// <summary>
	/// The artifact type of this inventory item
	/// </summary>
	public ArtifactType ArtifactType {
		get => _artifactType;
		set {
			_artifactType = value;

			// Set the artifact prefab based on the plant type
			Prefab = gardenManager.ArtifactPrefabs[_artifactType];

			// Update the inventory image with the prefab's sprite
			image.sprite = Prefab.GetComponent<GardenPlaceable>( ).InventorySprite;
		}
	}
}
