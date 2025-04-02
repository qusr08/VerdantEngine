using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI reward;
    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private MapPlayer cameraManager;

    private bool hovering;
    public Vector2 offset = new Vector2(-80f, -80f); // Optional offset to position the UI element next to the cursor.

    // Start is called before the first frame update
    void Start()
    {
        hovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hovering)
        {
            UpdateUIElementPosition();
        }
    }

    //Currently not called
    /*public void AtEvent(string type, string rewards = "")
    {

        if(rewards != "")
        {
            text.text = "" + type + " - " + rewards;
            return;
        }

        text.text = "" + type;
    }*/

    /*public void HoverEvent(string type, string rewards = "")
    {

        if (rewards != "")
        {
            text.text = "" + type + ": " + rewards;
            return;
        }

        text.text = "" + type;
    }*/


    public void StartHover(string encounterName, string encounterReward, string encounterDescription)
    {
        hovering = true;

        name.text = encounterName;
        reward.text = encounterReward;
        description.text = encounterDescription;
    }

    public void EndHover()
    {
        hovering = false;
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        rectTransform.position = new Vector3(100000, 100000, 0);
    }

    // Method to be called to update the UI element position.
    public void UpdateUIElementPosition()
    {

        Vector3 mousePosition = Input.mousePosition; // Get current mouse position in screen space.

        // Convert screen position to local position in the canvas space
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        rectTransform.position = mousePosition + new Vector3(offset.x, offset.y, 0);

    }

    public void ToGarden()
    {
        cameraManager.scene = ActiveScene.Garden;
        cameraManager.UpdateCameraPosition();
    }

    public void ToShop()
    {
        cameraManager.scene = ActiveScene.Shop;
        cameraManager.UpdateCameraPosition();
    }
}
