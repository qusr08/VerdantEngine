using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Spawn Rates")]
    [SerializeField] private int enemyChance;
    [SerializeField] private int shopChance;
    [SerializeField] private int eventChance;
    //These aren't used anymore. They were for spawning different events/enemies in the early days
    //[SerializeField] private int[] enemyRates;
    //[SerializeField] private int[] eventRates;

    [Header("Forced Encounters")]
    [SerializeField] private int[] spawnIndex;
    [SerializeField] private Type[] spawnType;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] eventPrefabs;
    [SerializeField] private GameObject[] shopPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private GameObject startPrefab;

    [Header("SOs")]
    [SerializeField] private List<CombatPresetSO> combatSO;
    [SerializeField] private List<Event_SO> eventSO;


    [Header("Debug")]
    [SerializeField] private byte encountersBeforeBoss;
    [SerializeField] private MapPlayer player;
    [SerializeField] private int[] percents;
    public int encounterSinceLastCombat = 0;
    private byte encounterNumber;
    private byte encounterOptions;
    private List<GameObject> encounters;
    private List<GameObject> prevEncounters;
    private EventManager eventManager;

    enum Type { Enemy, Shop, Event, Boss };

    // Start is called before the first frame update
    void Start()
    {
        percents = new int[] { enemyChance, shopChance, eventChance };

        if (player == null)
        {
            player = GameObject.Find("CameraManager").GetComponent<MapPlayer>();
        }

        if(eventManager == null)
        {
            eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        }

        encounterNumber = 0;


        encounters = new List<GameObject>();
        prevEncounters = new List<GameObject>();

        encounters.Add(Instantiate(startPrefab, new Vector3(0, 0, 0), transform.rotation, this.gameObject.transform));
        player.GetComponent<MapPlayer>().MoveTo(encounters[encounters.Count - 1], true);
        EncounterToPrev();

        PopulateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Populates the map with encounters
    /// </summary>
    private void PopulateMap()
    {

        if (encounterNumber % 2 == 0)
        {
            encounterOptions = 2;
        }
        else
        {
            encounterOptions = 3;
        }

        for (int i = 0; i < spawnIndex.Length; i++)
        {
            if (encounterNumber == spawnIndex[i])
            {
                while (encounterOptions > 0)
                {
                    SpawnEncounter(spawnType[i]);
                    encounterOptions--;
                }

                encounterNumber++;
                EncounterToPrev();
                PopulateMap();
                return;
            }

        }

        while(encounterOptions > 0)
        {
            int nextEncounter = Random.Range(0, 100);

            for (byte i = 0; i < percents.Length; i++)
            {
                int tempPercent = percents[i];
                //If the previous type wasn't spawned, add it's chances to this type. This allows something like a [3, 3, 4] to work. The first one is 3, the second turns to 6, and the final is 10.
                if (i > 0)
                {
                    for (byte j = 0; j < i; j++)
                    {
                        tempPercent += percents[j];
                    }
                }
                if (nextEncounter <= tempPercent)
                {
                    //Debug.Log("Enconter: " + encounterAmount + " = " + i);
                    //RepopulatePercents(i);
                    switch (i)
                    {
                        case 0:
                            SpawnEncounter(Type.Enemy);
                            break;
                        case 1:
                            SpawnEncounter(Type.Shop);
                            break;
                        case 2:
                            SpawnEncounter(Type.Event);
                            break;
                    }

                    encounterOptions--;
                    break;
                }
            }
        }

        encounterNumber++;
        EncounterToPrev();

        if (encounterNumber >= encountersBeforeBoss)
        {
            SpawnEncounter(Type.Boss);
        }
        else
        {
            PopulateMap();
        }

    }

    /// <summary>
    /// Spawns the prefab of the encounter
    /// </summary>
    /// <param name="i">prefab to spawn. -1 or lower to spawn boss</param>
    private void SpawnEncounter(Type type)
    {
        Vector3 nextPosition = new Vector3(6f * (encounterOptions - 2), 2f * (encounterNumber) + 2, 0);

        //nextPosition.x += 1.5f;

        if(type == Type.Boss)
        {
            nextPosition.x = 0;
        }
        else if(encounterNumber%2 == 0)
        {
            nextPosition.x += 3f;
        }
        
        if(encounterNumber == 3 && encounterOptions == 2)
        {
            type = Type.Shop;
        }
        else if(encounterNumber <= 3 && type == Type.Shop)
        {
            type = Type.Enemy;
        }

        switch (type)
            {
            case Type.Enemy:
                int enemyDifficulty = 0;

                /*
                //This is for COMPLETELY randomized combat appearences
                enemyDifficulty = GetSubType<CombatPresetSO>(combatSO);
                enemyDifficulty = combatSO[enemyDifficulty].difficulty;
                */

                enemyDifficulty = GetDifficulty();

                switch(enemyDifficulty)
                {
                    case 1:
                    case 2:
                    case 3:
                        encounters.Add(Instantiate(enemyPrefabs[0], nextPosition, transform.rotation, this.gameObject.transform));
                        break;
                    case 4:
                    case 5:
                    case 6:
                        encounters.Add(Instantiate(enemyPrefabs[1], nextPosition, transform.rotation, this.gameObject.transform));
                        break;
                    case 7:
                    case 8:
                    case 9:
                        encounters.Add(Instantiate(enemyPrefabs[2], nextPosition, transform.rotation, this.gameObject.transform));
                        break;

                }
                UpdateCombatEncounter(enemyDifficulty);
                break;
            case Type.Shop:
                encounters.Add(Instantiate(shopPrefabs[0], nextPosition, transform.rotation, this.gameObject.transform));
                break;
            case Type.Event:
                encounters.Add(Instantiate(eventPrefabs[0], nextPosition, transform.rotation, this.gameObject.transform));
                break;
            case Type.Boss:
                encounters.Add(Instantiate(bossPrefabs[0], nextPosition, transform.rotation, this.gameObject.transform));
                for (int i = 0; i < prevEncounters.Count; i++)
                {
                    prevEncounters[i].GetComponent<Encounter>().AddConnection(encounters[encounters.Count - 1]);
                }
                return;
        }

        if (prevEncounters.Count <= 0)
        {
            return;
        }

        //If there are 2 options in this line
        if(encounterNumber % 2 == 0)
        {
            if(encounterOptions > 1 || prevEncounters.Count == 1)
            {
                prevEncounters[0].GetComponent<Encounter>().AddConnection(encounters[encounters.Count - 1]);
                //Debug.DrawLine(prevEncounters[0].transform.position, encounters[encounters.Count - 1].transform.position, Color.blue, 5);
                if(prevEncounters.Count > 1)
                {
                    prevEncounters[1].GetComponent<Encounter>().AddConnection(encounters[encounters.Count - 1]);
                    //Debug.DrawLine(prevEncounters[1].transform.position, encounters[encounters.Count - 1].transform.position, Color.blue, 5);
                }
            }
            else
            {
                prevEncounters[1].GetComponent<Encounter>().AddConnection(encounters[encounters.Count - 1]);
                //Debug.DrawLine(prevEncounters[1].transform.position, encounters[encounters.Count - 1].transform.position, Color.red, 5);
                prevEncounters[2].GetComponent<Encounter>().AddConnection(encounters[encounters.Count - 1]);
                //Debug.DrawLine(prevEncounters[2].transform.position, encounters[encounters.Count - 1].transform.position, Color.red, 5);
            }
        }
        //If there are 3 options in this line
        else
        {
            if (encounterOptions >= 2)
            {
                prevEncounters[0].GetComponent<Encounter>().AddConnection(encounters[encounters.Count - 1]);
                //Debug.DrawLine(prevEncounters[0].transform.position, encounters[encounters.Count - 1].transform.position, Color.green, 5);
            }
            if(encounterOptions <= 2)
            {
                prevEncounters[1].GetComponent<Encounter>().AddConnection(encounters[encounters.Count - 1]);
                //Debug.DrawLine(prevEncounters[1].transform.position, encounters[encounters.Count - 1].transform.position, Color.black, 5);
            }

            if (encounterOptions != 2)
            {
                encounters[encounters.Count - 1].GetComponent<Encounter>().onlyOneConnection = true;
            }

        }

            encounters[encounters.Count - 1].GetComponent<Encounter>().HideEncounter();


    }

    /// <summary>
    /// Takes an array of ints 1-10 (that add to 10) and randomly selects an index based on values (that represent the chance of being picked)
    /// </summary>
    /// <param name="odds">The chances for each sub-type</param>
    /// <returns>The index of the chosen value</returns>
    private int GetSubType<T>(List<T> options)
    {
        int nextEncounter = Random.Range(0, options.Count); //Inclusive, exclusive

        return nextEncounter;
    }

    /// <summary>
    /// Takes an array of ints 1-10 (that add to 10) and randomly selects an index based on values (that represent the chance of being picked)
    /// </summary>
    /// <param name="odds">The chances for each sub-type</param>
    /// <returns>The index of the chosen value</returns>
    private int GetSubTypeSeeded(int[] odds)
    {
        int nextEncounter = Random.Range(0, 10);

        for (byte i = 0; i < odds.Length; i++)
        {
            int tempPercent = odds[i];

            //If the previous type wasn't spawned, add it's chances to this type. This allows something like a [3, 3, 4] to work. The first one is 3, the second turns to 6, and the final is 10.
            if (i > 0)
            {
                for (byte j = 0; j < i; j++)
                {
                    tempPercent += odds[j];
                }
            }
            if (nextEncounter <= tempPercent)
            {
                return i;
            }
        }

        return 0; //This should never call
    }

    private void UpdateCombatEncounter(int i)
    {
        CombatPresetSO newCombat = combatSO[0];

        List<CombatPresetSO> possibleCombats = new List<CombatPresetSO>();

        foreach(CombatPresetSO combat in combatSO)
        {
            if(combat.difficulty == i)
            {
                possibleCombats.Add(combat);
            }
        }

        newCombat = possibleCombats[Random.Range(0, possibleCombats.Count)];
        encounters[encounters.Count - 1].GetComponent<Encounter>().SetCombat(newCombat);
    }

    /// <summary>
    /// Populates prevEncounts with what is currently in encounters
    /// </summary>
    private void EncounterToPrev()
    {
        prevEncounters.Clear();

        for(int i = 0; i < encounters.Count; i++)
        {
            prevEncounters.Add(encounters[i]);
        }

        encounters.Clear();
    }


    public Event_SO GetEvent()
    {

        int eventIndex = Random.Range(0, eventSO.Count);

        Event_SO returnedEvent = eventSO[eventIndex];
        eventSO.RemoveAt(eventIndex);

        //For when adding events leading to combat
        //returnedEvent.ForceCombat


        return returnedEvent;

    }

    private int GetDifficulty()
    {
        int enemyDifficulty = 0;
        switch (encounterNumber)
        {
            case 0:
                enemyDifficulty = 1;
                break;
            case 1:
                enemyDifficulty = Random.Range(1, 3); //inclusive, exclusive
                break;
            case 2:
                enemyDifficulty = Random.Range(1, 4); //inclusive, exclusive
                break;
            case 3:
                enemyDifficulty = Random.Range(2, 5); //inclusive, exclusive
                break;
            case 4:
                enemyDifficulty = Random.Range(3, 6); //inclusive, exclusive
                break;
            case 5:
                enemyDifficulty = Random.Range(4, 7); //inclusive, exclusive
                break;
            case 6:
                enemyDifficulty = Random.Range(5, 8); //inclusive, exclusive
                break;
            case 7:
                enemyDifficulty = Random.Range(6, 9); //inclusive, exclusive
                break;
            case 8:
                enemyDifficulty = Random.Range(7, 10); //inclusive, exclusive
                break;
            case 9:
                enemyDifficulty = Random.Range(8, 10); //inclusive, exclusive
                break;
        }

        return enemyDifficulty;
    }


    //===================UNUSED====================

    /// <summary>
    /// Clears all encounters in the map, will be used when resetting the map
    /// </summary>
    public void ClearMap()
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    
    
    
    /// <summary>
    /// Basically reduces the chance of the selected encounter and increases the chances of the others
    /// </summary>
    /// <param name="i">The encounter to reduce</param>
    private void RepopulatePercents(int i)
    {
        //If the encounter chosen has a chance >= 40%, take 40% off it's chance and give the others +20%
        if (percents[i] >= 4)
        {
            percents[i] -= 4;
            if (i == 0)
            {
                percents[1] += 2;
                percents[2] += 2;
            }
            else if (i == 1)
            {
                percents[0] += 2;
                percents[2] += 2;
            }
            else
            {
                percents[0] += 2;
                percents[1] += 2;
            }
        }
        //If the encounter chosen has a chance >= 20%, take 20% off it's chance and give the others +10%
        else if (percents[i] >= 2)
        {
            percents[i] -= 2;
            if (i == 0)
            {
                percents[1] += 1;
                percents[2] += 1;
            }
            else if (i == 1)
            {
                percents[0] += 1;
                percents[2] += 1;
            }
            else
            {
                percents[0] += 1;
                percents[1] += 1;
            }
        }
        //If the encounter chosen has a chance >= 10%, take 10% off it's chance and give the next one +10%
        else
        {
            percents[i] -= 1;
            if (i == 0)
            {
                percents[1] += 1;
            }
            else if (i == 1)
            {
                percents[2] += 1;
            }
            else
            {
                percents[0] += 1;
            }
        }
    }

}
