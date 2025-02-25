using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthUIObject : MonoBehaviour {
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Enemy _enemy;
	[SerializeField] private Image image;
	[SerializeField] TextMeshProUGUI cooldownText;

	public Enemy Enemy {
		get => _enemy;
		set {
			_enemy = value;

			healthSlider.maxValue = _enemy.MaxHealth;
			healthSlider.value = _enemy.CurrentHealth;
			image.sprite = _enemy.Icon;
		}
	}

	public void UpdateHealth ( ) {
		healthSlider.value = Enemy.CurrentHealth;
	}

	public void UpdateCoolDown ( ) {
		if (Enemy.CurrentCooldown == 0) {
			cooldownText.color = Color.red;
		} else {
			cooldownText.color = Color.white;
		}

		cooldownText.text = Enemy.CurrentCooldown.ToString( );
	}

	public void Kill ( ) {
		image.color = Color.gray;
		healthSlider.value = 0;
	}
}
