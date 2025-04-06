using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPopUp : MonoBehaviour
{
    public GameObject infoPopUpObject;
    public PlantHover plantHover;
    public EnemyHover enemyHover;
    public Canvas canvas; // Reference to the Canvas the UI element belongs to.
    bool isPlant;
    public void SetUpEnemey(Enemy enemy)
    {
        UpdateUIElementPosition();
        plantHover.gameObject.SetActive(false);
        enemyHover.gameObject.SetActive(true);
        enemyHover.UpdateText(enemy);
        isPlant = false;
    }
    public void SetUpPlant(GardenPlaceable placeble)
    {
        UpdateUIElementPosition();
        plantHover.gameObject.SetActive(true);
        enemyHover.gameObject.SetActive(false);
        plantHover.UpdateText(placeble);
        isPlant = true;

    }
    public Vector2 offsetPlant = new Vector2(20f, 20f); // Optional offset to position the UI element next to the cursor.
    public Vector2 offsetEnemy = new Vector2(-160f, -160f); // Optional offset to position the UI element next to the cursor.

    // Method to be called to update the UI element position.
    public void UpdateUIElementPosition()
    {
      
        Vector3 mousePosition = Input.mousePosition; // Get current mouse position in screen space.

        // Convert screen position to local position in the canvas space
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        if (isPlant)
        {
            // Apply the offset to the local position
            rectTransform.position = mousePosition + new Vector3(offsetPlant.x, offsetPlant.y, 0);
        }
        else
        {
            rectTransform.position = mousePosition + new Vector3(offsetEnemy.x, offsetEnemy.y, 0);


        }

    }
    
}
