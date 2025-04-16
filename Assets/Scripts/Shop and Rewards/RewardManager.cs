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
    [SerializeField] TMP_Text healthText;

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
        for (int i = 0; i < probablility.Length; i++)
        {
           probablility[i] = Random.Range(0, 101); //uncomment when we have rare plants
           //probablility[i] = Random.Range(0, 81); //temporary, until we implement rare plants
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

    void FillShop()
    {
        commonIndex = 0;
        uncommonIndex = 0;
        rareIndex = 0;
        for(int i = 0; i < probablility.Length; i++)
        {
            if (probablility[i] <= 45)
            {
                items[i].GetComponentInChildren<Item>().FillRewardItemDetails(commonPlants[commonIndex].GetComponent<Plant>().Name, commonPlants[commonIndex].GetComponent<Plant>().InventorySprite, commonPlants[commonIndex].GetComponent<Plant>().Description, 1);
                items[i].GetComponent<Item>().plant = commonPlants[commonIndex].GetComponent<Plant>();
                if (commonIndex < commonPlants.Length - 1)
                {
                    commonIndex++;
                }
                else
                {
                    commonIndex = 0;
                }
            }
            else if (probablility[i] <= 77)
            {
                items[i].GetComponentInChildren<Item>().FillRewardItemDetails(uncommonPlants[uncommonIndex].GetComponent<Plant>().Name, uncommonPlants[uncommonIndex].GetComponent<Plant>().InventorySprite, uncommonPlants[uncommonIndex].GetComponent<Plant>().Description, 2);
                items[i].GetComponent<Item>().plant = uncommonPlants[uncommonIndex].GetComponent<Plant>();
                if (uncommonIndex < uncommonPlants.Length - 1)
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
                items[i].GetComponentInChildren<Item>().FillRewardItemDetails(rarePlants[rareIndex].GetComponent<Plant>().Name, rarePlants[rareIndex].GetComponent<Plant>().InventorySprite, rarePlants[rareIndex].GetComponent<Plant>().Description, 3);
                items[i].GetComponent<Item>().plant = rarePlants[rareIndex].GetComponent<Plant>();
                if (rareIndex <  rarePlants.Length - 1)
                {
                    rareIndex++;
                }
                else
                {
                    rareIndex = 0;
                }
            }
        }

        moneyRewardText.text = "Money earned from encounter : " + moneyReward.ToString();
        healthText.text = "Health : " + playerDataManager.CurrentHealth.ToString() + "/" + playerDataManager.MaxHealth.ToString();
    }
    public void AddToInventoryAfterFight(int itemIndex)
    {
        inventory.AddPlant(items[itemIndex - 1].GetComponent<Item>().plant.PlantType);
        combatManager.Win();
        gameObject.SetActive(false);
        
    }
}
