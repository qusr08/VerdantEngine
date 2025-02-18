using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
	[Header("References - InventoryItem")]
	[SerializeField] private PlayerData playerData;
	[Header("Properties - InventoryItem")]
	[SerializeField] private int _amount;

	/// <summary>
	/// The amount of this item in the inventory
	/// </summary>
	public int Amount {
		get => _amount;
		set {
			_amount = value;

			// If the amount is now 0, then remove this inventory item from the inventory
			if (_amount <= 0) {
				playerData.Inventory.Remove(this);
				Destroy(gameObject);
			}
		}
	}

	private void OnMouseDown ( ) {
		
	}

	private void OnMouseUp ( ) {

	}
}
