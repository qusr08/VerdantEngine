using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterTypes { Enemy, Shop, Event, Start};

public class Encounter : MonoBehaviour
{

    public bool First = false;

    public List<GameObject> ConnectingNode;
    public EncounterTypes EncounterType;
    [SerializeField] private MapPlayer player;
    [SerializeField] private MapUI mapUI;
    [SerializeField] private CombatPresetSO combatEncounter;
    [SerializeField] private CombatManager combatManager;

    [Header("Text")]
    [SerializeField] private string Name;
    [SerializeField] private string Rewards;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("CameraManager").GetComponent<MapPlayer>();
        }

        if (combatManager == null)
        {
            combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        }

        if (mapUI == null)
        {
            mapUI = GameObject.Find("Reward Text").GetComponent<MapUI>();
        }


        if (First)
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
    public void PlayerReached()
    {
        MapUI text = GameObject.Find("Reward Text").GetComponent<MapUI>();
        text.AtEvent(Name, Rewards);

        if (EncounterType == EncounterTypes.Enemy)
        {
            combatManager.NewCombat(combatEncounter);
            mapUI.ToGarden();
        }
        //Debug.Log("Player is at " + gameObject.name);
    }

    /// <summary>
    /// Runs when the player is at this encounter, and then leaves it
    /// </summary>
    public void PlayerLeave()
    {
        //Currently just hide this encounter. Eventually replace it's graphic
        gameObject.SetActive(false);
    }

    public void AddConnection(GameObject connection)
    {
        ConnectingNode.Add(connection);

        //Creates a child, gives it a line rendered, and then sets the line to be between this and it's connection
        GameObject child = new GameObject();
        child.transform.SetParent(transform);
        LineRenderer lineRenderer = child.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, connection.transform.position);
    }

    /// <summary>
    /// This encounter was clicked
    /// </summary>
    void OnMouseDown()
    {
        if(player.MoveTo(gameObject))
        {
            //PlayerReached();
            return;
        }
        Debug.Log("Not connecting encounter");
    }

    void OnMouseOver()
    {
        MapUI text = GameObject.Find("Reward Text").GetComponent<MapUI>();
        text.HoverEvent(Name, Rewards);
    }
}
