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
    /// <param name="force">Set true if and only if you want to force movement</param>
    public bool MoveTo(GameObject location, bool force = false)
    {
        //Debug.Log(location);
        if (force || CurrentEncounter.GetComponent<Encounter>().ConnectingNode.Contains(location))
        {
            transform.position = location.transform.position;
            CurrentEncounter = location;
            return true;

        }

        return false;
    }
}
