using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{

    public List<GameObject> ConnectingNode;
    [SerializeField] private MapPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<MapPlayer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Runs when the player reaches this encounter
    /// </summary>
    protected void PlayerReached()
    {
        Debug.Log("Player is at " + gameObject.name);
    }

    /// <summary>
    /// This encounter was clicked
    /// </summary>
    void OnMouseDown()
    {
        if(player.MoveTo(gameObject))
        {
            PlayerReached();

        }
        Debug.Log("Not connecting encounter");
    }
}
