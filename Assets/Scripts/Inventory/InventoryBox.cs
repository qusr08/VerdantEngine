using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventoryBox : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	[Header("References - InventoryItem")]
	[SerializeField] protected Inventory inventory;
	[SerializeField] protected PlayerDataManager playerDataManager;
	[SerializeField] protected GardenManager gardenManager;
	[SerializeField] protected TextMeshProUGUI amountText;
	[SerializeField] protected Image image;
	[Header("Properties - InventoryItem")]
	[SerializeField] private int _amount;
	[SerializeField] protected GameObject _prefab;
	[SerializeField] protected CombatManager combatManager;
    [SerializeField] private PlantHover _UIDisplay;

    /// <summary>
    /// The prefab associated with this inventory box. Use this to get information about the item stored
    /// </summary>
    public GameObject Prefab { get => _prefab; protected set => _prefab = value; }

	/// <summary>
	/// The amount of this item in the inventory
	/// </summary>
	public int Amount {
		get => _amount;
		set {
			_amount = value;

			// If the amount is now 0, then remove this inventory item from the inventory
			if (_amount <= 0) {
				inventory.InventoryBoxes.Remove(this);
				Destroy(gameObject);
			}

			// Update the UI element for the text
			amountText.text = _amount.ToString( );
		}
	}

    /// <summary>
    /// Whether or not the current tile is being hovered
    /// </summary>
    public PlantHover UIDisplay
    {
        get => _UIDisplay;
        set
        {
            _UIDisplay = value;
        }
    }

    private void Awake ( ) {
		inventory = FindObjectOfType<Inventory>( );
		gardenManager = FindObjectOfType<GardenManager>( );
		playerDataManager = FindObjectOfType<PlayerDataManager>( );
		combatManager = FindObjectOfType<CombatManager>();
        UIDisplay = FindObjectOfType<PlantHover>();

    }

    public void OnPointerDown (PointerEventData eventData) {
		playerDataManager.MouseSprite = Prefab.GetComponent<GardenPlaceable>( ).InventorySprite;
	}

	public virtual void OnPointerUp (PointerEventData eventData) {
		throw new NotImplementedException( );
		/// NOTE: This function should be overridden in child classes to implement placeable type specific code
	}

    public void OnMouseEnter()
    {
        UIDisplay.UpdateText(Prefab.GetComponent<GardenPlaceable>().Name, Prefab.GetComponent<GardenPlaceable>().Description,
            Prefab.GetComponent<GardenPlaceable>().MaxHealth, Prefab.GetComponent<GardenPlaceable>().MaxHealth, Prefab.GetComponent<GardenPlaceable>().InventorySprite);

    }
}
