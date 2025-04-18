using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterTypes { Enemy, Shop, Event, Start};

public class Encounter : MonoBehaviour
{

    public bool First = false;
    public bool onlyOneConnection = false;

    public List<GameObject> ConnectingNode;
    public List<GameObject> ConnectingLines;
    public EncounterTypes EncounterType;
    [SerializeField] private MapPlayer player;
    [SerializeField] private MapUI mapUI;
    [SerializeField] private CombatPresetSO combatEncounter;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private Sprite clearedSprite;

    [Header("Text")]
    [SerializeField] private string Name;
    [SerializeField] private string Rewards;
    [SerializeField] private string Description;
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
            mapUI = GameObject.Find("Hover Text").GetComponent<MapUI>();
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

    public void SetCombat(CombatPresetSO newCombat)
    {
        combatEncounter = newCombat;
        Rewards = Rewards + " + $" + newCombat.rewardMoeny;
    }

    /// <summary>
    /// Runs when the player reaches this encounter
    /// </summary>
    public void PlayerReached()
    {

        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = clearedSprite;
        foreach (GameObject connection in ConnectingNode)
        {
            connection.GetComponent<Encounter>().UnHideEncounter();
        }
        foreach (GameObject line in ConnectingLines)
        {
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.startColor = new Color(.88f, .89f, .73f);
            lineRenderer.endColor = new Color(.88f, .89f, .73f);
        }

        if (EncounterType == EncounterTypes.Enemy)
        {
            combatManager.NewCombat(combatEncounter);
            mapUI.ToGarden();
        }
        else if(EncounterType == EncounterTypes.Shop)
        {
            mapUI.ToShop();
        }
        //Debug.Log("Player is at " + gameObject.name);
    }

    /// <summary>
    /// Runs when the player is at this encounter, and then leaves it
    /// </summary>
    public void PlayerLeave(GameObject connection)
    {
        if(ConnectingNode.Contains(connection))
        {
            foreach(GameObject line in ConnectingLines)
            {
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();

                if (line.GetComponent<MapLines>().to == connection)
                {
                    lineRenderer.startColor = Color.green;
                    lineRenderer.endColor = Color.green;
                }
                else
                {
                    lineRenderer.startColor = Color.gray;
                    lineRenderer.endColor = Color.gray;
                }
            }

            //Loops through options. If option is unpicked hide it
            foreach(GameObject otherTile in ConnectingNode)
            {
                if(otherTile != connection)
                {
                    otherTile.GetComponent<Encounter>().HideEncounter();
                }
            }
        }
    }

    /// <summary>
    /// Hides an unpicked/unpickable encounter
    /// </summary>
    public void HideEncounter()
    {
        foreach(GameObject line in ConnectingLines)
        {
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.startColor = Color.gray;
            lineRenderer.endColor = Color.grey;
        }
        this.GetComponent<SpriteRenderer>().color = Color.grey;
    }

    /// <summary>
    /// Hides an unpicked/unpickable encounter
    /// </summary>
    public void UnHideEncounter()
    {
        this.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void AddConnection(GameObject connection)
    {
        ConnectingNode.Add(connection);

        //Creates a child, gives it a line rendered, and then sets the line to be between this and it's connection
        GameObject child = new GameObject();
        child.transform.SetParent(transform);

        ConnectingLines.Add(child);

        MapLines line = child.AddComponent<MapLines>();
        line.from = this.gameObject;
        line.to = connection;

        LineRenderer lineRenderer = child.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.grey;
        //lineRenderer.startColor = new Color (.88f, .89f, .73f);
        //lineRenderer.endColor = new Color(.88f, .89f, .73f);

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

    private void OnMouseEnter()
    {

        mapUI.StartHover(this.gameObject, Name, Rewards, Description);
        //_PopUpDisplay.gameObject.SetActive(true);

    }

    private void OnMouseExit()
    {
        mapUI.EndHover();
        //_PopUpDisplay.gameObject.SetActive(false);

    }

}
