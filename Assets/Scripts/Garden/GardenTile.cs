using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenTile : MonoBehaviour {
	[SerializeField] private MeshRenderer meshRenderer;
	[SerializeField] private PlayerDataManager playerDataManager;
	[SerializeField] private GardenManager gardenManager;
	[SerializeField] private CombatManager combatManager;
	[Space]
	[SerializeField] private Color[ ] basicColors;
	[SerializeField] private Color[ ] attackedColors;
	[SerializeField] private Color[ ] selectedColors;
	[SerializeField] private Vector2Int _position;
	[SerializeField] private GardenPlaceable _gardenPlaceable;
	[SerializeField] private bool _isAttacked;
	[SerializeField] private bool _isSelected;
	[Space]
	[SerializeField] private PlantHover _plantHoverDisplay;
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
				gardenManager.SelectedGardenTile = this;
			} else {
				// Only set the selected tile to null if it is still the current selected tile
				// It may be possible that another tile is selected before this one sets the selected tile to null, which would break the code
				if (gardenManager.SelectedGardenTile == this) {
					gardenManager.SelectedGardenTile = null;
				}
			}

			UpdateMaterial( );
		}
	}

	/// <summary>
	/// A reference to the plant hover display object
	/// </summary>
	public PlantHover PlantHoverDisplay {
		get => _plantHoverDisplay;
		set {
			_plantHoverDisplay = value;
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
		playerDataManager = FindObjectOfType<PlayerDataManager>( );
		gardenManager = FindObjectOfType<GardenManager>( );
		combatManager = FindObjectOfType<CombatManager>();
		PlantHoverDisplay = FindObjectOfType<PlantHover>( );
	}

	private void OnMouseEnter ( ) {
		if (GardenPlaceable != null) {
			PlantHoverDisplay.UpdateText(GardenPlaceable);
			//Debug.Log(GardenPlaceable.gameObject.name);
		}

		IsSelected = true;
	}

	private void OnMouseExit ( ) {
		IsSelected = false;
	}

	private void OnMouseDown ( ) {
		// If this tile has no garden placeable, then do not try to move it
		if (GardenPlaceable == null) {
			return;
		}

		playerDataManager.MouseSprite = GardenPlaceable.InventorySprite;
		GardenPlaceable.flowerVisuals.SetActive ( false);
	}

	private void OnMouseUp ( ) {
		// If this tile has no garden placeable, then do not try to move it
		if (GardenPlaceable == null) {
			return;
		}

		GardenPlaceable.flowerVisuals.SetActive(true);
		playerDataManager.MouseSprite = null;

		// If there is no selected garden tile, then also return
		if (gardenManager.SelectedGardenTile == null) {
			return;
		}

		// If there are no more actions remaining, then do not try to move it
		if (playerDataManager.CurrentActions <= 0 && combatManager.combatUIManager.GameState != GameState.IDLE) {
			return;
		}

		// Actually place the garden placeable on the new tile and decrease the action stat counter ONLY IF the move was successful
		if (gardenManager.MovePlant(GardenPlaceable as Plant, gardenManager.SelectedGardenTile)) {
			playerDataManager.CurrentActions--;
			combatManager.UpdateEnemyAttackVisuals( );
		}
	}

	/// <summary>
	/// Update this garden tile's material based on if it is attacked or not
	/// </summary>
	private void UpdateMaterial ( ) {
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
