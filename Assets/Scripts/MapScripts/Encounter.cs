using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{

    public bool First = false;

    public List<GameObject> ConnectingNode;
    [SerializeField] private MapPlayer player;

    [Header("Text")]
    [SerializeField] private string Name;
    [SerializeField] private string Rewards;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<MapPlayer>();
        }

        if(First)
        {
            PlayerReached();
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
        MapText text = GameObject.Find("Text (TMP)").GetComponent<MapText>();
        text.AtEvent(Name, Rewards);
        //Debug.Log("Player is at " + gameObject.name);
    }

    /// <summary>
    /// This encounter was clicked
    /// </summary>
    void OnMouseDown()
    {
        if(player.MoveTo(gameObject))
        {
            PlayerReached();
            return;
        }
        Debug.Log("Not connecting encounter");
    }
}
