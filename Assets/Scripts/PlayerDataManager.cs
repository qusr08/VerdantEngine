using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private int _gardenSize;
	[SerializeField] public int cuurentHealth;
	[SerializeField] public int maxHealth;
	[SerializeField] public int money;
	[Space]
	[SerializeField] private Image mouseImage;
	[SerializeField] private RectTransform mouseImageTransform;
	[SerializeField] private Canvas guiCanvas;

	/// <summary>
	/// The size of the garden (both width and height)
	/// </summary>
	public int GardenSize { get => _gardenSize; private set => _gardenSize = value; }

	/// <summary>
	/// A reference to the mouse sprite that follows the mouse around. This can be used to show what plant is being placed
	/// </summary>
	public Sprite MouseSprite { 
		get => mouseImage.sprite;
		set {
			mouseImage.sprite = value;
			mouseImage.enabled = (value != null);
		}
	}

	/// <summary>
	/// The grid of garden placeables that represent the layout of the garden
	/// </summary>
	public GardenTile[ , ] Garden { get; private set; }

	/// <summary>
	/// A list of all the current parts on the mech
	/// </summary>
	public List<Part_SO> CurrentParts { get; private set; }

	private void Awake ( ) {
		CurrentParts = new List<Part_SO>( );
		Garden = new GardenTile[GardenSize, GardenSize];
	}

	private void Update ( ) {
		// Set the mouse image to match wherever the mouse is
		mouseImageTransform.anchoredPosition = Input.mousePosition / guiCanvas.scaleFactor;
	}
}
