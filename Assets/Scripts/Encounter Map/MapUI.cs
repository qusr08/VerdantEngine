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

    [SerializeField] TMP_Text healthText;
    [SerializeField] TMP_Text balanceText;
    [SerializeField] PlayerDataManager playerDataManager;

    private bool hovering;
    public Vector2 offset = new Vector2(0, 0); // Optional offset to position the UI element next to the cursor.
    private GameObject hoveredEncounter;

    // Start is called before the first frame update
    void Start()
    {
        hovering = false;
        hoveredEncounter = null;

        if (playerDataManager == null)
        {
            playerDataManager = GameObject.Find("Player Data Manager").GetComponent<PlayerDataManager>();
        }
    }

    private void OnEnable()
    {
        balanceText.text = "Balance : " + playerDataManager.Money.ToString();
        healthText.text = "Health : " + playerDataManager.CurrentHealth.ToString() + "/" + playerDataManager.MaxHealth.ToString();

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


    public void StartHover(GameObject encounter, string encounterName, string encounterReward, string encounterDescription)
    {
        hovering = true;

        name.text = encounterName;
        reward.text = encounterReward;
        description.text = encounterDescription;
        hoveredEncounter = encounter;
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
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Vector3 toWorldVec = Camera.main.WorldToScreenPoint(hoveredEncounter.transform.position);

        rectTransform.position = toWorldVec;
        rectTransform.position += new Vector3(offset.x, offset.y, 0);

        //This is for setting it to mouse pos, decided not to use
        //Vector3 mousePosition = Input.mousePosition; // Get current mouse position in screen space.
        //rectTransform.position = mousePosition + new Vector3(offset.x, offset.y, 0);

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
