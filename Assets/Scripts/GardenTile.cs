using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTile : MonoBehaviour {
	[Header("References")]
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private Inventory inventory;
	[Header("Properties")]
	[SerializeField] private Color[ ] basicColors;
	[SerializeField] private Color[ ] attackedColors;
	[SerializeField] private Color[ ] selectedColors;
	[SerializeField] private Vector2Int _position;
	[SerializeField] private GardenPlaceable _gardenPlaceable;
	[SerializeField] private bool _isAttacked;
	[SerializeField] private bool _isSelected;
	[Header("UI")]
	[SerializeField] private PlantHover _UIDisplay;

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
	/// Whether or not this tile is being selected by the mouse hovering over it
	/// </summary>
	public bool IsSelected {
		get => _isSelected;
		set {
			_isSelected = value;

			// Update the inventory's selected tile based on the new value
			if (_isSelected) {
				// Set the inventory's selected tile to this tile
				inventory.SelectedGardenTile = this;
			} else {
				// Only set the selected tile to null if it is still the current selected tile
				// It may be possible that another tile is selected before this one sets the selected tile to null, which would break the code
				if (inventory.SelectedGardenTile == this) {
					inventory.SelectedGardenTile = null;
				}
			}

			UpdateMaterial( );
		}
	}

	/// <summary>
	/// Whether or not the current tile is being attacked
	/// </summary>
	public PlantHover UIDisplay {
		get => _UIDisplay;
		set {
			_UIDisplay = value;
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

	private void Awake ( ) {
		inventory = FindObjectOfType<Inventory>( );
	}

	private void OnMouseEnter ( ) {
		if (GardenPlaceable != null) {
			UIDisplay.UpdateText(GardenPlaceable.gameObject.name, "Description");
			//Debug.Log(GardenPlaceable.gameObject.name);
		}

		IsSelected = true;
	}

	private void OnMouseExit ( ) {
		IsSelected = false;
	}

	/// <summary>
	/// Update this garden tile's material based on if it is attacked or not
	/// </summary>
	/// <param name="forceAttackedState">Whether or not to force the tile to appear as if it was attacked</param>
	private void UpdateMaterial (bool forceAttackedState = false) {
		// Set the material color of the ground tile
		Material tempMaterial = new Material(meshRenderer.material);
		// Make the colors of the ground tiles a checkerboard pattern
		if (IsSelected) {
			tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? selectedColors[0] : selectedColors[1]);
		} else if (IsAttacked) {
			tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? attackedColors[0] : attackedColors[1]);
		} else {
			tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? basicColors[0] : basicColors[1]);
		}
		meshRenderer.material = tempMaterial;
	}
}
