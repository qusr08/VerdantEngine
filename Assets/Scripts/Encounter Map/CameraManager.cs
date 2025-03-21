using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayer : MonoBehaviour
{

    public bool onMap; //True for on map, false for on garden
    public GameObject CurrentEncounter;

    [SerializeField] private Transform gardenCameraLocation;
    [SerializeField] private GameObject gardenStuff;
    [SerializeField] private GameObject mapStuff;
    [SerializeField] private GameObject camera;
    [SerializeField] private CombatUIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        UpdateCameraPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Used when flipping between garden and map
    public void FlipScenes()
    {
        uiManager.GameState = GameState.IDLE;
        onMap = !onMap;
        UpdateCameraPosition();
    }
    public void UpdateCameraPosition()
    {
        gardenStuff.SetActive(!onMap);
        mapStuff.SetActive(onMap);

        if (onMap)
        {
            camera.transform.position = this.transform.position;
            camera.transform.rotation = this.transform.rotation;

        }
        else
        {
            camera.transform.position = gardenCameraLocation.transform.position;
            camera.transform.rotation = gardenCameraLocation.transform.rotation;
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
        if (force || CurrentEncounter.GetComponent<Encounter>().ConnectingNode.Contains(location))
        {
            if(CurrentEncounter != null)
            {
                CurrentEncounter.GetComponent<Encounter>().PlayerLeave();
            }

            transform.position = new Vector3(location.transform.position.x, location.transform.position.y, -10);
            UpdateCameraPosition();
            CurrentEncounter = location;
            if (location.GetComponent<Encounter>().EncounterType == EncounterTypes.Enemy)
            {
                gardenStuff.SetActive(true);
            }
            location.GetComponent<Encounter>().PlayerReached();
            return true;
        }

        return false;
    }
}
