using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class PlayerAttackMenuItem : MonoBehaviour {
	[SerializeField] public PlayerAttackSO _playerAttack;
	[SerializeField] private TextMeshProUGUI nameText;
	[SerializeField] private TextMeshProUGUI coolDownText;
	[SerializeField] private TextMeshProUGUI damageText;
	[SerializeField] private TextMeshProUGUI manaCostText;
	[SerializeField] private TextMeshProUGUI detailsText;

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
			manaCostText.text = _playerAttack.ManaCost.ToString( );
			detailsText.text = _playerAttack.Description;
		}
	}

	public void UpdateCoolDown ( ) {
		coolDownText.text = PlayerAttack.Cooldown.ToString( );
	}
}
