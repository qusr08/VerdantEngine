using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Map : MonoBehaviour
{
    [Header("Spawn Rates")]
    [SerializeField] private int enemyChance;
    [SerializeField] private int shopChance;
    [SerializeField] private int eventChance;
    [SerializeField] private int[] enemyRates;
    [SerializeField] private int[] eventRates;


    [Header("Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] eventPrefabs;
    [SerializeField] private GameObject[] shopPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private GameObject startPrefab;

    [Header("Debug")]
    [SerializeField] private byte encountersBeforeBoss;
    [SerializeField] private MapPlayer player;
    [SerializeField] private int[] percents;
    private byte encounterNumber;
    private byte encounterOptions;
    private List<GameObject> encounters;
    private List<GameObject> prevEncounters;

    enum Type { Enemy, Shop, Event, Boss };

    // Start is called before the first frame update
    void Start()
    {
        percents = new int[] { enemyChance, shopChance, eventChance };

        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<MapPlayer>();
        }

        encounterNumber = 0;


        encounters = new List<GameObject>();
        prevEncounters = new List<GameObject>();

        encounters.Add(Instantiate(startPrefab, new Vector3(0, 1.5f, 0), transform.rotation));
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
        int nextEncounter = Random.Range(0, 10);

        if (encounterNumber%2 == 0)
        {
            encounterOptions = 2;
        }else
        {
            encounterOptions = 3;
        }

        while(encounterOptions > 0)
        {
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
                    RepopulatePercents(i);
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

    /// <summary>
    /// Spawns the prefab of the encounter
    /// </summary>
    /// <param name="i">prefab to spawn. -1 or lower to spawn boss</param>
    private void SpawnEncounter(Type type)
    {
        Vector3 nextPosition = new Vector3(2f * (encounterNumber + 1), 2f * (encounterOptions - 1), 0);

        if(encounterNumber%2 == 0)
        {
            nextPosition.y += .75f;
        }

        if(nextPosition.y < 0)
        {
            nextPosition.y = 1.5f;
        }

        switch (type)
            {
            case Type.Enemy:
                encounters.Add(Instantiate(enemyPrefabs[GetSubType(enemyRates)], nextPosition, transform.rotation));
                for(int i = 0; i<prevEncounters.Count; i++)
                {
                    prevEncounters[i].GetComponent<Encounter>().ConnectingNode.Add(encounters[encounters.Count - 1]);
                }
                break;
            case Type.Shop:
                encounters.Add(Instantiate(shopPrefabs[0], nextPosition, transform.rotation));
                for (int i = 0; i < prevEncounters.Count; i++)
                {
                    prevEncounters[i].GetComponent<Encounter>().ConnectingNode.Add(encounters[encounters.Count - 1]);
                }
                break;
            case Type.Event:
                encounters.Add(Instantiate(eventPrefabs[GetSubType(eventRates)], nextPosition, transform.rotation));
                for (int i = 0; i < prevEncounters.Count; i++)
                {
                    prevEncounters[i].GetComponent<Encounter>().ConnectingNode.Add(encounters[encounters.Count - 1]);
                }
                break;
            case Type.Boss:
                encounters.Add(Instantiate(bossPrefabs[0], nextPosition, transform.rotation));
                for (int i = 0; i < prevEncounters.Count; i++)
                {
                    prevEncounters[i].GetComponent<Encounter>().ConnectingNode.Add(encounters[encounters.Count - 1]);
                }
                break;
        }
            
    }

    private int GetSubType(int[] odds)
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

    private void EncounterToPrev()
    {
        prevEncounters.Clear();

        for(int i = 0; i < encounters.Count; i++)
        {
            prevEncounters.Add(encounters[i]);
        }

        encounters.Clear();
    }
}
