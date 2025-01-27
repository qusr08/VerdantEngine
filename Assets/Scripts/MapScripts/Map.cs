using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Debug")]
    [SerializeField] private byte encountersBeforeBoss;
    [SerializeField] private MapPlayer player;
    [SerializeField] private int[] percents;
    private byte encounterAmount;
    private GameObject encounter;
    private GameObject prevEncounter;

    enum Type { Enemy, Shop, Event, Boss };

    // Start is called before the first frame update
    void Start()
    {
        percents = new int[] { enemyChance, shopChance, eventChance };

        if (player == null)
        {
            player = GameObject.Find("Player").GetComponent<MapPlayer>();
        }

        encounterAmount = 0;
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

        for(byte i = 0; i < percents.Length; i++)
        {
            int tempPercent = percents[i];

            //If the previous type wasn't spawned, add it's chances to this type. This allows something like a [3, 3, 4] to work. The first one is 3, the second turns to 6, and the final is 10.
            if(i > 0)
            {
                for(byte j = 0; j < i; j++)
                {
                    tempPercent += percents[j];
                }
            }
            if(nextEncounter <= tempPercent)
            {
                //Debug.Log("Enconter: " + encounterAmount + " = " + i);
                encounterAmount++;
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
                break;
            }
        }

        if(encounterAmount >= encountersBeforeBoss)
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
        Vector3 nextPosition = new Vector3(2f * encounterAmount, 0, 0);

        switch (type)
            {
            case Type.Enemy:
                if (encounter != null)
                {
                    prevEncounter = encounter;
                    encounter = Instantiate(enemyPrefabs[GetSubType(enemyRates)], nextPosition, transform.rotation);
                    prevEncounter.GetComponent<Encounter>().ConnectingNode.Add(encounter);
                }
                else
                {
                    encounter = Instantiate(enemyPrefabs[GetSubType(enemyRates)], nextPosition, transform.rotation);
                    player.GetComponent<MapPlayer>().MoveTo(encounter, true);
                    encounter.GetComponent<Encounter>().First = true;
                }
                break;
            case Type.Shop:
                if (encounter != null)
                {
                    prevEncounter = encounter;
                    encounter = Instantiate(shopPrefabs[0], nextPosition, transform.rotation);
                    prevEncounter.GetComponent<Encounter>().ConnectingNode.Add(encounter);
                }
                else
                {
                    encounter = Instantiate(shopPrefabs[0], nextPosition, transform.rotation);
                    player.GetComponent<MapPlayer>().MoveTo(encounter, true);
                    encounter.GetComponent<Encounter>().First = true;
                }
                break;
            case Type.Event:
                if (encounter != null)
                {
                    prevEncounter = encounter;
                    encounter = Instantiate(eventPrefabs[GetSubType(eventRates)], nextPosition, transform.rotation);
                    prevEncounter.GetComponent<Encounter>().ConnectingNode.Add(encounter);
                }
                else
                {
                    encounter = Instantiate(eventPrefabs[GetSubType(eventRates)], nextPosition, transform.rotation);
                    player.GetComponent<MapPlayer>().MoveTo(encounter, true);
                    encounter.GetComponent<Encounter>().First = true;
                }
                break;
            case Type.Boss:
                nextPosition.x += 2f;
                prevEncounter = encounter;
                encounter = Instantiate(bossPrefabs[0], nextPosition, transform.rotation);
                prevEncounter.GetComponent<Encounter>().ConnectingNode.Add(encounter);
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
}
