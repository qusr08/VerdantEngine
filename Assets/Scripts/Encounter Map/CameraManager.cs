using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public enum ActiveScene { Map, Garden, Shop};

public class MapPlayer : MonoBehaviour
{

    public ActiveScene scene; //True for on map, false for on garden
    public GameObject CurrentEncounter;

    [SerializeField] private Transform gardenCameraLocation;
    [SerializeField] private GameObject gardenStuff;
    [SerializeField] private GameObject mapStuff;
    [SerializeField] private GameObject shopStuff;
    [SerializeField] private GameObject camera;
    [SerializeField] private CombatUIManager uiManager;
    [SerializeField] private GameObject playerSprite;

    private bool moving = false;
    private GameObject nextLocation = null;
    private float movingPercent = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            movingPercent += Time.deltaTime;

            transform.position = Vector3.Lerp(CurrentEncounter.transform.position, nextLocation.transform.position, movingPercent);

            //transform.position = new Vector3(0, nextLocation.transform.position.y + 3, -10);
            //playerSprite.transform.position = new Vector3(nextLocation.transform.position.x, playerSprite.transform.position.y, playerSprite.transform.position.z);

            //This is needed to load the garden before the combat loads in. If this isn't here combat will break
            if (nextLocation.GetComponent<Encounter>().EncounterType == EncounterTypes.Enemy)
            {
                gardenStuff.SetActive(true);
            }
            nextLocation.GetComponent<Encounter>().PlayerReached();

            if(movingPercent >= 1)
            {
                movingPercent = 0.0f;
                moving = false;
                UpdateCameraPosition();

            }
        }
    }

    //Used when flipping between garden, map, and shop
    public void ChangeScenes(ActiveScene nextScene)
    {
        uiManager.GameState = GameState.IDLE;
        scene = nextScene;
        UpdateCameraPosition();
    }

    /// <summary>
    /// For some reason buttons can't call the ChangeScenes() function so these functions are used to change scenes
    /// </summary>
    public void GoToGarden()
    {
        ChangeScenes(ActiveScene.Garden);
    }
    public void GoToMap()
    {
        ChangeScenes(ActiveScene.Map);
    }
    public void GoToShop()
    {
        ChangeScenes(ActiveScene.Shop);
    }

    public void UpdateCameraPosition()
    {
        switch (scene)
        {
            case ActiveScene.Map:
                gardenStuff.SetActive(false);
                shopStuff.SetActive(false);
                mapStuff.SetActive(true);
                playerSprite.SetActive(true);


                camera.transform.position = this.transform.position;
                camera.transform.rotation = this.transform.rotation;
                break;
            case ActiveScene.Garden:
                gardenStuff.SetActive(true);
                shopStuff.SetActive(false);
                mapStuff.SetActive(false);
                playerSprite.SetActive(false);

                camera.transform.position = gardenCameraLocation.transform.position;
                camera.transform.rotation = gardenCameraLocation.transform.rotation;
                break;
            case ActiveScene.Shop:
                gardenStuff.SetActive(true);
                shopStuff.SetActive(true);
                mapStuff.SetActive(false);
                playerSprite.SetActive(false);


                camera.transform.position = gardenCameraLocation.transform.position;
                camera.transform.rotation = gardenCameraLocation.transform.rotation;
                break;
        }

    }


    /// <summary>
    /// Moves the player to selected encounter
    /// </summary>
    /// <param name="location">The encounter the player moves too</param>
    /// <param name="force">Set true if and only if you want to force movement</param>
    public bool MoveTo(GameObject location, bool force = false)
    {
        //Debug.Log(location);
        if(force)
        {
            CurrentEncounter = location;
            transform.position = new Vector3(0, nextLocation.transform.position.y + 3, -10);
            playerSprite.transform.position = new Vector3(nextLocation.transform.position.x, playerSprite.transform.position.y, playerSprite.transform.position.z);
        }
        if (CurrentEncounter.GetComponent<Encounter>().ConnectingNode.Contains(location))
        {
            /* - Calls a function telling it that the player left, currently that function does nothing so I commented this out
             
            if(CurrentEncounter != null)
            {
                CurrentEncounter.GetComponent<Encounter>().PlayerLeave();
            }*/

            nextLocation = location;
            moving = true;

        }
        else
        {
            moving = false;
        }

        return moving;
    }
}
