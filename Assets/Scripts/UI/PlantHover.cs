using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantHover : MonoBehaviour {
	[SerializeField] private TMP_Text plantName;
	[SerializeField] private TMP_Text plantDescription;
	[SerializeField] private TMP_Text plantHP;
	[SerializeField] private Image plantImage;

	public void UpdateText (string name, string description, int currentHealth, int maxHealth, Sprite image) {
		plantName.text = name;
		plantDescription.text = description;
		plantHP.text = "" + currentHealth + "/" + maxHealth;
		plantImage.sprite = image;
	}
}
