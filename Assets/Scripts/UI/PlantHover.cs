using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantHover : MonoBehaviour {
	[SerializeField] private TMP_Text plantNameText;
	[SerializeField] private TMP_Text plantDescriptionText;
	[SerializeField] private TMP_Text plantHealthText;
	[SerializeField] private TMP_Text plantShieldText;
	[SerializeField] private Image plantImage;

	public void UpdateText (GardenPlaceable gardenPlaceable) {
		plantNameText.text = gardenPlaceable.Name;
		plantDescriptionText.text = gardenPlaceable.Description;
		plantImage.sprite = gardenPlaceable.InventorySprite;

		// This makes the health modifier visible when looking at the health of one of the plants
		// This also subtracts from the health modifier when the base value is less than 0 to help with making it easier to understand
		string healthModifierString = "";
		if (gardenPlaceable.HealthStat.TotalModifier > 0) {
			healthModifierString = $"<color=\"green\"> + {gardenPlaceable.HealthStat.TotalModifier}</color>";
		} else if (gardenPlaceable.HealthStat.TotalModifier < 0) {
			healthModifierString = $"<color=\"red\"> - {gardenPlaceable.HealthStat.TotalModifier}</color>";
		}
		plantHealthText.text = Mathf.Max(0, gardenPlaceable.HealthStat.BaseValue) + healthModifierString;

		plantShieldText.text = gardenPlaceable.ShieldStat.CurrentValue.ToString( );
	}
}
