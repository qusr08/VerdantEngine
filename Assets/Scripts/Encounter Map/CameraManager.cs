using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.FilePathAttribute; - idk what this is but it's causing a build error so uhhh??

public enum ActiveScene { Map, Garden, Shop, Event, Win};

public class MapPlayer : MonoBehaviour
{

    public ActiveScene scene; //True for on map, false for on garden
    public GameObject CurrentEncounter;

    [SerializeField] private Transform gardenCameraLocation;
    [SerializeField] private GameObject gardenStuff;
    [SerializeField] private GameObject mapStuff;
    [SerializeField] private GameObject shopStuff;
    [SerializeField] private GameObject eventStuff;
    [SerializeField] private GameObject winStuff;
    [SerializeField] private GameObject tutorialStuff;
    [SerializeField] private GameObject camera;
    [SerializeField] private CombatUIManager uiManager;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private GameObject walker;
    [SerializeField] private Sprite[] walkerSprites;
    private int currentSpriteIndex = 0;

    [SerializeField] private GameObject flowerTrailPrefab;
    [SerializeField] private Sprite[] flowerTrailSprites;

    private bool moving = false;
    private GameObject nextLocation = null;
    private float movingPercent = 0.0f;
    private float timeSinceFlowerSpawn = 0.0f;

    public BG_Music_Manager soundManager;

    public CombatManager combatManager;

    public GameObject[] enemyHP;
    
    // Start is called before the first frame update
    void Start()
    {
        if(eventManager == null)
        {
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        }
        if (combatManager == null)
        {
            combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        }
        UpdateCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            movingPercent += Time.deltaTime;
            timeSinceFlowerSpawn += Time.deltaTime;

            Vector3 currentLocation = CurrentEncounter.transform.position;
            currentLocation.y += .5f;

            Vector3 nextLocationEdited = nextLocation.transform.position;
            nextLocationEdited.y += .5f;

            if (nextLocationEdited.x < currentLocation.x)
            {
                walker.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                walker.GetComponent<SpriteRenderer>().flipX = false;
            }

            walker.transform.position = Vector3.Lerp(currentLocation, nextLocationEdited, movingPercent);

            if (timeSinceFlowerSpawn >= .08f)
            {
                GameObject flowerTrail = Instantiate(flowerTrailPrefab, mapStuff.transform);
                flowerTrail.transform.position = walker.transform.position;
                flowerTrailPrefab.GetComponent<SpriteRenderer>().sprite = flowerTrailSprites[Random.Range(0, flowerTrailSprites.Length)];
                flowerTrail.transform.position = new Vector3(flowerTrail.transform.position.x, flowerTrail.transform.position.y - Random.Range(.4f, .6f), flowerTrail.transform.position.z);
                timeSinceFlowerSpawn = 0;

                currentSpriteIndex++;

                if (currentSpriteIndex >= walkerSprites.Length)
                {
                    currentSpriteIndex = 0;
                }

                walker.GetComponent<SpriteRenderer>().sprite = walkerSprites[currentSpriteIndex];
            }
            

            if(movingPercent >= 1)
            {
                movingPercent = 0.0f;
                moving = false;

                //Combat will break if this doesn't pre-load garden
                if (nextLocation.GetComponent<Encounter>().EncounterType == EncounterTypes.Enemy)
                {
                    gardenStuff.SetActive(true);
                }
                nextLocation.GetComponent<Encounter>().PlayerReached();

                CurrentEncounter = nextLocation;

                transform.position = new Vector3(0, nextLocation.transform.position.y + 3, -10);
                walker.transform.position = new Vector3(nextLocation.transform.position.x, transform.position.y - 2.5f, walker.transform.position.z);

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
    public void GoToEvent(Event_SO incomingEvent)
    {
        eventManager.InitilazeEvent(incomingEvent);
        ChangeScenes(ActiveScene.Event);
    }
    public void GoToWin()
    {
        ChangeScenes(ActiveScene.Win);
    }

    public void HideTutorial()
    {
        tutorialStuff.SetActive(false);
    }

    public void UpdateCameraPosition()
    {
        gardenStuff.SetActive(false);
        shopStuff.SetActive(false);
        mapStuff.SetActive(false);
        walker.SetActive(false);
        eventStuff.SetActive(false);
        winStuff.SetActive(false);
        tutorialStuff.SetActive(false);
        switch (scene)
        {
            case ActiveScene.Map:
                mapStuff.SetActive(true);
                walker.SetActive(true);
                tutorialStuff.SetActive(true);

                camera.transform.position = this.transform.position;
                camera.transform.rotation = this.transform.rotation;
                break;
            case ActiveScene.Garden:
                gardenStuff.SetActive(true);
                tutorialStuff.SetActive(true);

                camera.transform.position = gardenCameraLocation.transform.position;
                camera.transform.rotation = gardenCameraLocation.transform.rotation;

                //check combat
                if(combatManager.Enemies.Count>0)
                {
                    ShowEnemeyPartsUI();

                    combatManager.UpdateEnemyAttackVisuals();
                }
                else
                {
                    HideEnemeyPartsUI();
                }
                break;
            case ActiveScene.Shop:
                gardenStuff.SetActive(true);
                shopStuff.SetActive(true);

                camera.transform.position = gardenCameraLocation.transform.position;
                camera.transform.rotation = gardenCameraLocation.transform.rotation;
                break;
            case ActiveScene.Event:
                gardenStuff.SetActive(true);
                eventStuff.SetActive(true);

                camera.transform.position = gardenCameraLocation.transform.position;
                camera.transform.rotation = gardenCameraLocation.transform.rotation;
                break;
            case ActiveScene.Win:
                winStuff.SetActive(true);
                break;
        }

    }

    public void HideEnemeyPartsUI()
    {
        foreach (var item in enemyHP)
        {
            item.SetActive(false);
        }
    }

    public void ShowEnemeyPartsUI()
    {
        foreach (var item in enemyHP)
        {
            item.SetActive(true);
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
            transform.position = new Vector3(0, location.transform.position.y + 3, -10);
            walker.transform.position = new Vector3(location.transform.position.x, walker.transform.position.y, walker.transform.position.z);

            return true;
        }

        if(CurrentEncounter == location)
        {
            GoToGarden();
            return true;
        }

        if (CurrentEncounter.GetComponent<Encounter>().ConnectingNode.Contains(location))
        {

            //Tells current encounter that it left
            if(CurrentEncounter != null)
            {
                CurrentEncounter.GetComponent<Encounter>().PlayerLeave(location);
            }

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
