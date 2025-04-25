using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerAttackMenuItem : MonoBehaviour {
	[SerializeField] public PlayerAttackSO _playerAttack;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI coolDownText;
	[SerializeField] private TextMeshProUGUI damageText;
	[SerializeField] private TextMeshProUGUI manaCostText;
	[SerializeField] private TextMeshProUGUI detailsText;
	[SerializeField] Image image;
    [SerializeField] Image coolDownicon;

    [SerializeField] private GameObject coolDownOverlay;

    [SerializeField] private TextMeshProUGUI coolDownOverlayText;
    [SerializeField] public InfoPopUp UIDisplay;

    /// <summary>
    /// The attack of this weapon menu item
    /// </summary>
    public PlayerAttackSO PlayerAttack {
		get => _playerAttack;
		set {
			_playerAttack = value;

			_playerAttack.Cooldown = _playerAttack.MaxCooldown;
			nameText.text = _playerAttack.Name;
			damageText.text = _playerAttack.Damage.ToString( );
			coolDownText.text = _playerAttack.MaxCooldown.ToString( );
            coolDownOverlayText.text = _playerAttack.MaxCooldown.ToString();

            manaCostText.text = _playerAttack.ManaCost.ToString( );
			detailsText.text = _playerAttack.Description;
			image.sprite = _playerAttack.Icon;

        }
	}

	public void UpdateCoolDown ( ) {
		coolDownText.text = PlayerAttack.Cooldown.ToString( );
        coolDownOverlayText.text = PlayerAttack.Cooldown.ToString();

    }
    public void GetOnCooldown()
	{
		coolDownOverlay.SetActive(true);

    }
    public void ReadyToFire()
    {
        coolDownOverlay.SetActive(false);


    }

    public void OnMouseEnter()
    {


        if (UIDisplay.gameObject != null)

        {
            UIDisplay.gameObject.SetActive(true);
            UIDisplay.SetUpWeapon(this);
        }
    }

    public void OnMouseExit()
    {
      
        if (UIDisplay.gameObject != null)
            UIDisplay.gameObject.SetActive(false);
    }
}
