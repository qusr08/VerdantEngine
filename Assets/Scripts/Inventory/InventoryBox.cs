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
	[SerializeField] protected GardenManager gardenManager;
	[SerializeField] protected TextMeshProUGUI amountText;
	[SerializeField] protected Image image;
	[Header("Properties - InventoryItem")]
	[SerializeField] private int _amount;
	[SerializeField] protected GameObject _prefab;

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

	private void OnValidate ( ) {
		inventory = FindObjectOfType<Inventory>( );
		gardenManager = FindObjectOfType<GardenManager>( );
	}

	private void Awake ( ) {
		OnValidate( );
	}

	public void OnPointerDown (PointerEventData eventData) {
		inventory.MouseImage.sprite = Prefab.GetComponent<GardenPlaceable>( ).InventorySprite;
		inventory.MouseImage.gameObject.SetActive(true);
	}

	public virtual void OnPointerUp (PointerEventData eventData) {
		throw new NotImplementedException( );
		/// NOTE: This function should be overridden in child classes to implement placeable type specific code
	}
}
