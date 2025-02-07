using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTile : MonoBehaviour {
	[Header("References")]
	[SerializeField] private MeshRenderer meshRenderer;
	[Header("Properties")]
	[SerializeField] private Color[ ] basicColors;
	[SerializeField] private Color[ ] attackedColors;
	[SerializeField] private Vector2Int _position;
	[SerializeField] private GardenPlaceable _gardenPlaceable;
	[SerializeField] private bool _isAttacked;

	/// <summary>
	/// Whether or not the current tile is being attacked
	/// </summary>
	public bool IsAttacked {
		get => _isAttacked;
		set {
			_isAttacked = value;

			UpdateMaterial( );
		}
	}

	/// <summary>
	/// The garden placeable that is on this garden tile
	/// </summary>
	public GardenPlaceable GardenPlaceable { get => _gardenPlaceable; set => _gardenPlaceable = value; }

	/// <summary>
	/// The position of this garden tile within the garden
	/// </summary>
	public Vector2Int Position {
		get => _position;
		set {
			_position = value;
			transform.localPosition = new Vector3(_position.x, 0, _position.y);

			UpdateMaterial( );
		}
	}

	private void OnMouseEnter ( ) {
		/// TESTING
		IsAttacked = true;
	}

	private void OnMouseExit ( ) {
		/// TESTING
		IsAttacked = false;
	}

	/// <summary>
	/// Update this garden tile's material based on if it is attacked or not
	/// </summary>
	private void UpdateMaterial ( ) {
		// Set the material color of the ground tile
		Material tempMaterial = new Material(meshRenderer.material);
		// Make the colors of the ground tiles a checkerboard pattern
		if (IsAttacked) {
			tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? attackedColors[0] : attackedColors[1]);
		} else {
			tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? basicColors[0] : basicColors[1]);
		}
		meshRenderer.material = tempMaterial;
	}
}
