using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlayer : MonoBehaviour
{
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
    public void MoveTo(GameObject location)
    {
        //Debug.Log(location);
        transform.position = location.transform.position; 
    }
}
