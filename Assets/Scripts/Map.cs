using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] prefabs = new GameObject[3];
    [SerializeField] private GameObject boss;

    [Header("Debug")]
    [SerializeField] private byte encountersBeforeBoss;
    [SerializeField] private int[] percents = new int[] { 10, 0, 0, 0 }; //{Enemy, shop, ?, MapSplit(idk might remove this one)}
    private byte encounterAmount;

    // Start is called before the first frame update
    void Start()
    {
        encounterAmount = 0;
        PopulateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                Debug.Log("Enconter: " + encounterAmount + " = " + i);
                encounterAmount++;
                RepopulatePercents(i);
                SpawnEncounter(i);
                break;
            }
        }

        if(encounterAmount >= encountersBeforeBoss)
        {
            SpawnEncounter(-1);
        }
        else
        {
            PopulateMap();
        }

    }

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

    private void SpawnEncounter(int i)
    {
        Vector3 nextPosition = new Vector3(2f * encounterAmount, 0, 0);

        if(i < 0)
        {
            Instantiate(boss, nextPosition, transform.rotation);
            return;
        }

        Instantiate(prefabs[i], nextPosition, transform.rotation);
    }
}
