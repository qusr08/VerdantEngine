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


    [SerializeField] Sprite treeImage;

	public void UpdateText (GardenPlaceable gardenPlaceable) {
		plantNameText.text = gardenPlaceable.Name;
		plantDescriptionText.text = gardenPlaceable.Description;
		plantImage.sprite = gardenPlaceable.InventorySprite;

		// If the health stat is null, then this garden placeable is a prefab in the inventory
		// Just get the starting health value instead of the health stat current value
		if (gardenPlaceable.HealthStat == null) {
			plantHealthText.text = gardenPlaceable.StartingHealth.ToString( );
		} else {
			// This makes the health modifier visible when looking at the health of one of the plants
			// This also subtracts from the health modifier when the base value is less than 0 to help with making it easier to understand
			string healthModifierString = "";
			if (gardenPlaceable.HealthStat.TotalModifier > 0) {
				healthModifierString = $"<color=\"green\"> + {gardenPlaceable.HealthStat.TotalModifier}</color>";
			} else if (gardenPlaceable.HealthStat.TotalModifier < 0) {
				healthModifierString = $"<color=\"red\"> - {gardenPlaceable.HealthStat.TotalModifier}</color>";
			}
			if (gardenPlaceable.HealthStat.CurrentValue > 0)
			{
				plantHealthText.text = Mathf.Max(0, gardenPlaceable.HealthStat.CurrentValue) + healthModifierString + "/" + gardenPlaceable.StartingHealth.ToString();
			}
			else
			{
                plantHealthText.text = gardenPlaceable.StartingHealth.ToString() + healthModifierString + "/" + gardenPlaceable.StartingHealth.ToString();

            }
        }

		// If the shield stat is null, then this garden placeable is a prefab in the inventory
		// Just set the default value to be 0 (this may be changed in the future)
		if (gardenPlaceable.ShieldStat == null) {
			plantShieldText.text = "0";
		} else {
			plantShieldText.text = gardenPlaceable.ShieldStat.CurrentValue.ToString( );
		}
	}
    public void UpdateTextTree(PlayerDataManager data)
    {
        plantNameText.text = "Tree Of Life";
        plantDescriptionText.text = "The Tree of life is the power source of your mech, if he dies, hope dies with it";
        plantImage.sprite = treeImage;

        // If the health stat is null, then this garden placeable is a prefab in the inventory
        // Just get the starting health value instead of the health stat current value
        
            plantHealthText.text = data.CurrentHealth.ToString();
        
   

        // If the shield stat is null, then this garden placeable is a prefab in the inventory
        // Just set the default value to be 0 (this may be changed in the future)
        
            plantShieldText.text = "0";
        
        
    }
}
