using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantType {
	NONE, FLOWER, CACTUS
}

public class Plant : MonoBehaviour {
	[Header("References")]
	[SerializeField] private Garden garden;
	[Header("Properties")]
	[SerializeField] private PlantType _plantType;
	[SerializeField] private Vector2Int _position;

	/// <summary>
	/// The type of this plant
	/// </summary>
	public PlantType PlantType {
		get => _plantType;
		set {
			_plantType = value;

			/// TO DO: Set the sprite/image of this plant when its plant type is updated
		}
	}

	/// <summary>
	/// The position of this plant in the garden
	/// </summary>
	public Vector2Int Position {
		get {
			return _position;
		}
		set {
			_position = value;
			transform.localPosition = new Vector3(_position.x, 0f, _position.y);
		}
	}

	private void Awake ( ) {
		garden = FindObjectOfType<Garden>( );

	}

	/// <summary>
	/// Initialize this plant right after it has been created in the garden. Everything in this function needs to be called before Awake() but after it is instantiated in the garden.
	/// </summary>
	public void Initialize ( ) {
		// Make sure the plants are always facing towards the camera
		transform.LookAt(-Camera.main.transform.position + transform.localPosition);
		transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
	}
}
