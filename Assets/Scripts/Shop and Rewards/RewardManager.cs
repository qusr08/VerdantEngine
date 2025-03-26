using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class RewardManager : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    [SerializeField] GameObject[] plantPrefabs;
  
    [SerializeField] TMP_Text moneyRewardText;

    [SerializeField] int[] probablility;

    [SerializeField] PlayerDataManager playerDataManager;
    public CombatManager combatManager;
    [SerializeField] Inventory inventory;   

    public int moneyReward;

    public List<GameObject> commonPlantsList;
    public List<GameObject> uncommonPlantsList;
    public List<GameObject> rarePlantsList;

    public GameObject[] commonPlants;
    public GameObject[] uncommonPlants;
    public GameObject[] rarePlants;

    public int commonIndex;
    public int uncommonIndex;
    public int rareIndex;
    // Start is called before the first frame update
    void Start()
    {
        probablility = new int[items.Length];
        ArrangeByRarity();
        ShufflePlants();
    }
    private void OnEnable()
    {
        ShufflePlants();
    }

    private void ArrangeByRarity()
    {
        foreach(GameObject plant in plantPrefabs)
        {
            if(plant.GetComponent<Plant>().Rarity == Rarity.UNCOMMON)
            {
                uncommonPlantsList.Add(plant);
            }
            else if(plant.GetComponent<Plant>().Rarity == Rarity.COMMON)
            {
                commonPlantsList.Add(plant);
            }
            else
            {
                rarePlantsList.Add(plant);
            }
        }

        commonPlants = commonPlantsList.ToArray();
        uncommonPlants = uncommonPlantsList.ToArray();
        rarePlants = rarePlantsList.ToArray();
    }

    void ShufflePlants()
    {
        ClearShop();

        for (int i = 0; i < probablility.Length; i++)
        {
            probablility[i] = Random.Range(0, 101);
        }

        for (int i = 0; i < commonPlants.Length; i++)
        {
            GameObject tmp = commonPlants[i];
            int r = Random.Range(i, commonPlants.Length);
            commonPlants[i] = commonPlants[r];
            commonPlants[r] = tmp;
        }

        for (int i = 0; i < uncommonPlants.Length; i++)
        {
            GameObject tmp = uncommonPlants[i];
            int r = Random.Range(i, uncommonPlants.Length);
            uncommonPlants[i] = uncommonPlants[r];
            uncommonPlants[r] = tmp;
        }

        for (int i = 0; i < rarePlants.Length; i++)
        {
            GameObject tmp = rarePlants[i];
            int r = Random.Range(i, rarePlants.Length);
            rarePlants[i] = rarePlants[r];
            rarePlants[r] = tmp;
        }

        FillShop();
    }
    void ClearShop()
    {
        for (int i = 0; i < items.Length; i++)
        {
            foreach(Transform child in items[i].transform)
            {
                if(child.GetComponent<Plant>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
    void FillShop()
    {
        commonIndex = 0;
        uncommonIndex = 0;
        rareIndex = 0;
        for(int i = 0; i < items.Length; i++)
        {
            if (probablility[i] <= 50)
            {
                items[i].GetComponentInChildren<Item>().FillRewardItemDetails(commonPlants[commonIndex].name.Substring(2), commonPlants[commonIndex].GetComponent<Plant>().InventorySprite);
                GameObject newPlantItem = Instantiate(commonPlants[commonIndex], items[i].transform);
                newPlantItem.GetComponent<MeshRenderer>().enabled = false;
                if(commonIndex < commonPlants.Length - 1)
                {
                    commonIndex++;
                }
                else
                {
                    commonIndex = 0;
                }
            }
            else if (probablility[i] > 50 && probablility[i] <= 80)
            {
                items[i].GetComponentInChildren<Item>().FillRewardItemDetails(uncommonPlants[uncommonIndex].name.Substring(2), uncommonPlants[uncommonIndex].GetComponent<Plant>().InventorySprite);
                GameObject newPlantItem = Instantiate(uncommonPlants[uncommonIndex], items[i].transform);
                newPlantItem.GetComponent<MeshRenderer>().enabled = false;
                if(uncommonIndex < uncommonPlants.Length - 1)
                {
                    uncommonIndex++;
                }
                else
                {
                    uncommonIndex = 0;
                }
            }
            else
            {
                items[i].GetComponentInChildren<Item>().FillRewardItemDetails(rarePlants[rareIndex].name.Substring(2), rarePlants[rareIndex].GetComponent<Plant>().InventorySprite);
                GameObject newPlantItem = Instantiate(rarePlants[rareIndex], items[i].transform);
                newPlantItem.GetComponent<MeshRenderer>().enabled = false;
                if(rareIndex <  rarePlants.Length - 1)
                {
                    rareIndex++;
                }
                else
                {
                    rareIndex = 0;
                }
            }
        }


        //also add the uncokmmon/rare plant in random spot on the prefabs and items array
/*        if (includeUncommon)
        {
            randomSpotUncommon = Random.Range(0, plantItems.Length);

            foreach (Transform child in plantItems[randomSpotUncommon].transform)
            {
                if (child.GetComponent<Plant>() != null)
                {
                    Destroy(child.gameObject);
                }
            }

            Debug.Log("Including uncommon");
            GameObject newPlantItem = Instantiate(test1, plantItems[randomSpotUncommon].transform);
            plantItems[randomSpotUncommon].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = test1.name.Substring(2); //Display plant name on buttons
            plantItems[randomSpotUncommon].transform.GetChild(1).GetComponent<Image>().sprite = test1.GetComponent<Plant>().InventorySprite;
        }
        if (includeRare)
        {
            do
            {
                randomSpotRare = Random.Range(0, plantItems.Length);
            } while (randomSpotRare == randomSpotUncommon); //Making sure rare and uncommon and rare are not spawned in same item spot
            

            foreach (Transform child in plantItems[randomSpotRare].transform)
            {
                if (child.GetComponent<Plant>() != null)
                {
                    Destroy(child.gameObject);
                }
            }

            Debug.Log("Including rare");
            GameObject newPlantItem = Instantiate(test2, plantItems[randomSpotRare].transform);
            plantItems[randomSpotRare].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = test2.name.Substring(2); //Display plant name on buttons
            plantItems[randomSpotRare].transform.GetChild(1).GetComponent<Image>().sprite = test2.GetComponent<Plant>().InventorySprite;
        }*/

        moneyRewardText.text = "Money earned from encounter : " + moneyReward.ToString();
    }
    public void AddToInventoryAfterFight(int itemIndex)
    {
        inventory.AddPlant(items[itemIndex - 1].GetComponentInChildren<Plant>().PlantType);
        combatManager.Win();
        gameObject.SetActive(false);
        
    }
}
