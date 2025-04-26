using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour {
	[Header("Properties")]
	[SerializeField] private int _gardenSize;
	[SerializeField] private int _currentHealth;
	[SerializeField] private int _maxHealth;
	[SerializeField] private int _money;
	[SerializeField] private List<PlayerAttackSO> _playerAttacks;
	[Space]
	[SerializeField] private Image mouseImage;
	[SerializeField] private RectTransform mouseImageTransform;
	[SerializeField] private Canvas guiCanvas;
	[Space]
	[SerializeField] private int _currentActions;
	[SerializeField] private int _maxActions;
	[SerializeField] private TextMeshProUGUI actionsText;
	[SerializeField] private Slider hpSlider;
	[SerializeField] private GameObject youLose;
	[HideInInspector] public int actionCountModifier = 0;

	
	/// <summary>
	/// The max health of the player
	/// </summary>
	public int MaxHealth => _maxHealth;

	/// <summary>
	/// The current money that the player has
	/// </summary>
	public int Money { get => _money; set => _money = value; }

	/// <summary>
	/// The size of the garden (both width and height)
	/// </summary>
	public int GardenSize { get => _gardenSize; private set => _gardenSize = value; }

	public int CurrentHealth {
		get => _currentHealth;
		set {

			_currentHealth = value;
			if (_currentHealth > MaxHealth)
				_currentHealth = MaxHealth;

            hpSlider.value = (float) _currentHealth / (float) MaxHealth;
			if (_currentHealth <= 0) {
				youLose.SetActive(true);

			}
		}
	}

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
	/// The maximum actions that a player can take during their turn
	/// </summary>
	public int MaxActions => _maxActions+actionCountModifier;

	/// <summary>
	/// The current remaining actions for the player during their turn
	/// </summary>
	public int CurrentActions {
		get => _currentActions;
		set {
			_currentActions = value;

			actionsText.text = $"{_currentActions} / {MaxActions}";
		}
	}

	/// <summary>
	/// The grid of garden placeables that represent the layout of the garden
	/// </summary>
	public GardenTile[ , ] Garden { get; private set; }

	/// <summary>
	/// A list of all the current attacks on this mech
	/// </summary>
	public List<PlayerAttackSO> PlayerAttacks { get => _playerAttacks; private set => _playerAttacks = value; }

	private void Awake ( ) {
		Garden = new GardenTile[GardenSize, GardenSize];
		CurrentHealth = MaxHealth;
		hpSlider.value = CurrentHealth / MaxHealth;
	}

	private void Update ( ) {
		// Set the mouse image to match wherever the mouse is
		mouseImageTransform.anchoredPosition = Input.mousePosition / guiCanvas.scaleFactor;
	}
}
