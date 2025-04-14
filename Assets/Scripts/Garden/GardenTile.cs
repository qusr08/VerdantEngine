using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GardenTile : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private GardenManager gardenManager;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private TextMeshPro damageText;
    [Space]
    [SerializeField] private Color[] basicColors;
    [SerializeField] private Color[] attackedColors;
    [SerializeField] private Color[] selectedColors;
    [SerializeField] private Color[] highLightColors;

    [SerializeField] private Vector2Int _position;
    [SerializeField] private GardenPlaceable _gardenPlaceable;
    [SerializeField] private bool _isAttacked;
    [SerializeField] private int _attackedDamage;
    [SerializeField] private bool _isSelected;
    [SerializeField] private bool _isHighlighted;

    [Space]
    [SerializeField] private InfoPopUp _PopUpDisplay;

    /// <summary>
    /// The current amount of damage that this tile is being attacked for
    /// </summary>
    public int AttackedDamage
    {
        get => _attackedDamage;
        set
        {
            _attackedDamage = value;

            UpdateTile();
        }
    }

    /// <summary>
    /// Whether or not this tile is being highlighted by the player
    /// </summary>
    public bool IsHighlighted
    {
        get => _isHighlighted;
        set
        {

            _isHighlighted = value;

            UpdateTile();
        }
    }

    public InfoPopUp PopUpDisplay
    {
        get => _PopUpDisplay;
        set
        {

            _PopUpDisplay = value;
        }
    }

    /// <summary>
    /// Whether or not this tile is being selected by the mouse hovering over it
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;

            // Update the inventory's selected tile based on the new value
            if (_isSelected)
            {
                // Set the inventory's selected tile to this tile
                gardenManager.SelectedGardenTile = this;
            }
            else
            {
                // Only set the selected tile to null if it is still the current selected tile
                // It may be possible that another tile is selected before this one sets the selected tile to null, which would break the code
                if (gardenManager.SelectedGardenTile == this)
                {
                    gardenManager.SelectedGardenTile = null;
                }
            }

            UpdateTile();
        }
    }

    

    /// <summary>
    /// The garden placeable that is on this garden tile
    /// </summary>
    public GardenPlaceable GardenPlaceable { get => _gardenPlaceable; set => _gardenPlaceable = value; }

    /// <summary>
    /// The position of this garden tile within the garden
    /// </summary>
    public Vector2Int Position
    {
        get => _position;
        set
        {
            _position = value;
            transform.localPosition = new Vector3(_position.x, 0, _position.y);

            UpdateTile();
        }
    }

    private void Awake()
    {
        playerDataManager = FindObjectOfType<PlayerDataManager>();
        gardenManager = FindObjectOfType<GardenManager>();
        combatManager = FindObjectOfType<CombatManager>();
        _PopUpDisplay = FindObjectOfType<InfoPopUp>();
    }

    private void Update()
    {
        // Have the damage text look at the camera
        damageText.transform.LookAt(-Camera.main.transform.position + transform.position);
        //float angleRadians = damageText.transform.localEulerAngles.y * Mathf.Deg2Rad;
        //damageText.transform.localPosition = new Vector3(Mathf.Cos(-angleRadians) * 0.5f, Mathf.Sin(angleRadians) * 0.5f, damageText.transform.localPosition.z);
    }

    private void OnMouseDown()
    {
        // If this tile has no garden placeable, then do not try to move it
        if (GardenPlaceable == null)
        {
            return;
        }

        playerDataManager.MouseSprite = GardenPlaceable.InventorySprite;
		foreach (SpriteRenderer spriteRenderer in GardenPlaceable.SpriteRenderers) {
			spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
		}
		PopUpDisplay.gameObject.SetActive(false);
    }

    private void OnMouseUp()
    {
        // If this tile has no garden placeable, then do not try to move it
        if (GardenPlaceable == null)
        {
            return;
        }

		foreach (SpriteRenderer spriteRenderer in GardenPlaceable.SpriteRenderers) {
			spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
		}
		playerDataManager.MouseSprite = null;

        // If there is no selected garden tile, then also return
        if (gardenManager.SelectedGardenTile == null)
        {
            return;
        }

        // If there are no more actions remaining, then do not try to move it
        if (playerDataManager.CurrentActions <= 0 && combatManager.combatUIManager.GameState != GameState.IDLE)
        {
            return;
        }

        // Actually place the garden placeable on the new tile and decrease the action stat counter ONLY IF the move was successful
        if (gardenManager.MovePlant(GardenPlaceable as Plant, gardenManager.SelectedGardenTile))
        {
            playerDataManager.CurrentActions--;
            combatManager.UpdateEnemyAttackVisuals();
        }
        else if (gardenManager.MoveArtifact(GardenPlaceable as Artifact, gardenManager.SelectedGardenTile))
        {
            playerDataManager.CurrentActions--;
            combatManager.UpdateEnemyAttackVisuals();
        }

    }

    /// <summary>
    /// Update this garden tile's material based on if it is attacked or not
    /// </summary>
    private void UpdateTile()
    {
        // Make sure the attack damage is only showing when it is actually being damaged
        if (AttackedDamage > 0)
        {
            damageText.gameObject.SetActive(true);
            damageText.text = AttackedDamage.ToString();
        }
        else
        {
            damageText.gameObject.SetActive(false);
        }

        // Set the material color of the ground tile
        Material tempMaterial = new Material(meshRenderer.material);
        // Make the colors of the ground tiles a checkerboard pattern
        if (IsHighlighted)
        {
            tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? highLightColors[0] : highLightColors[1]);
        }
        else if (IsSelected)
        {
            tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? selectedColors[0] : selectedColors[1]);
        }
        else if (AttackedDamage > 0)
        {
            tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? attackedColors[0] : attackedColors[1]);
        }
        else
        {
            tempMaterial.color = ((_position.x + _position.y) % 2 == 0 ? basicColors[0] : basicColors[1]);
        }
        meshRenderer.material = tempMaterial;
    }
    private void OnMouseEnter()
    {
        // Only show the placeable info pop-up if there is a garden placeable on this tile and there is not a plant currently being moved
        if (GardenPlaceable != null && playerDataManager.MouseSprite == null) {
			PopUpDisplay.SetUpPlant(GardenPlaceable);
			PopUpDisplay.gameObject.SetActive(true);
            //Debug.Log(GardenPlaceable.gameObject.name);
        }

        IsSelected = true;
    }

    private void OnMouseExit()
    {
        IsSelected = false;

        PopUpDisplay.gameObject.SetActive(false);
    }
}