using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayer : MonoBehaviour
{

    public GameObject CurrentEncounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Moves the player to selected encounter
    /// </summary>
    /// <param name="location">The encounter the player moves too</param>
    public bool MoveTo(GameObject location)
    {
        //Debug.Log(location);
        if (CurrentEncounter.GetComponent<Encounter>().ConnectingNode.Contains(location))
        {
            transform.position = location.transform.position;
            CurrentEncounter = location;
            return true;

        }

        return false;
    }
}
