using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject[] plantItems;
    [SerializeField] GameObject[] artifactItems;
    [SerializeField] GameObject[] partItems;

    [SerializeField] GameObject[] plantPrefabs;
    [SerializeField] GameObject[] artifactPrefabs;
    [SerializeField] PlayerAttackSO[] partPrefabs;

    [SerializeField] int[] plantProbablility;
    [SerializeField] TMP_Text healthText;

    [SerializeField] TMP_Text balanceText;
    [SerializeField] TMP_Text purchaseText;

    [SerializeField] TMP_Text healCost;
    [SerializeField] int costToHeal;

    [SerializeField] PlayerDataManager playerDataManager;
    [SerializeField] PlayerCombatManager playerCombatManager;
    [SerializeField] Inventory inventory;

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
        plantProbablility = new int[plantItems.Length];
        ArrangeByRarity();
        Shuffle();
        balanceText.text = "Balance : " + playerDataManager.Money.ToString();
        healCost.text = costToHeal.ToString();
    }
    private void OnEnable()
    {
        Shuffle();
        balanceText.text = "Balance : " + playerDataManager.Money.ToString();
        healCost.text = costToHeal.ToString();
    }
    private void ArrangeByRarity()
    {
        foreach (GameObject plant in plantPrefabs)
        {
            if (plant.GetComponent<Plant>().Rarity == Rarity.UNCOMMON)
            {
                uncommonPlantsList.Add(plant);
            }
            else if (plant.GetComponent<Plant>().Rarity == Rarity.COMMON)
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
    void Shuffle()
    {
        for (int i = 0; i < plantProbablility.Length; i++)
        {
            // plantProbablility[i] = Random.Range(0, 101); //uncomment when we have rare plants
            plantProbablility[i] = Random.Range(0, 81); //temporary, until we implement rare plants
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


        for (int i = 0; i < artifactPrefabs.Length; i++)
        {
            GameObject tmp = artifactPrefabs[i];
            int r = Random.Range(i, artifactPrefabs.Length);
            artifactPrefabs[i] = artifactPrefabs[r];
            artifactPrefabs[r] = tmp;
        }

        for (int i = 0; i < partPrefabs.Length; i++)
        {
            PlayerAttackSO tmp = partPrefabs[i];
            int r = Random.Range(i, partPrefabs.Length);
            partPrefabs[i] = partPrefabs[r];
            partPrefabs[r] = tmp;
        }

        FillShop();
    }

    void FillShop()
    {
        commonIndex = 0;
        uncommonIndex = 0;
        rareIndex = 0;
        for (int i = 0; i < plantProbablility.Length; i++)
        {
            if (plantProbablility[i] <= 50)
            {
                plantItems[i].GetComponent<Item>().FillShopItemDetails(commonPlants[commonIndex].GetComponent<Plant>().Name, commonPlants[commonIndex].GetComponent<Plant>().Cost, commonPlants[commonIndex].GetComponent<Plant>().InventorySprite, commonPlants[commonIndex].GetComponent<Plant>().Description);
                plantItems[i].GetComponent<Item>().plant = commonPlants[commonIndex].GetComponent<Plant>();
                if (commonIndex < commonPlants.Length - 1)
                {
                    commonIndex++;
                }
                else
                {
                    commonIndex = 0;
                }
            }
            else if (plantProbablility[i] > 50 && plantProbablility[i] <= 80)
            {
                plantItems[i].GetComponent<Item>().FillShopItemDetails(uncommonPlants[uncommonIndex].GetComponent<Plant>().Name, uncommonPlants[uncommonIndex].GetComponent<Plant>().Cost, uncommonPlants[uncommonIndex].GetComponent<Plant>().InventorySprite, uncommonPlants[uncommonIndex].GetComponent<Plant>().Description);
                plantItems[i].GetComponent<Item>().plant = uncommonPlants[uncommonIndex].GetComponent<Plant>();
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
                plantItems[i].GetComponent<Item>().FillShopItemDetails(rarePlants[rareIndex].GetComponent<Plant>().Name, rarePlants[rareIndex].GetComponent<Plant>().Cost, rarePlants[rareIndex].GetComponent<Plant>().InventorySprite, rarePlants[rareIndex].GetComponent<Plant>().Description);
                plantItems[i].GetComponent<Item>().plant = rarePlants[rareIndex].GetComponent<Plant>();
                if (rareIndex < rarePlants.Length - 1)
                {
                    rareIndex++;
                }
                else
                {
                    rareIndex = 0;
                }
            }
        }
        for(int i = 0; i < plantItems.Length; i++)
        {
            plantItems[i].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Purchase";
            plantItems[i].GetComponentInChildren<Button>().interactable = true;
        }
        for (int i = 0; i < artifactItems.Length; i++)
        {
            artifactItems[i].GetComponent<Item>().FillShopItemDetails(artifactPrefabs[i].GetComponent<Artifact>().Name, artifactPrefabs[i].GetComponent<Artifact>().Cost, artifactPrefabs[i].GetComponent<Artifact>().InventorySprite,
                artifactPrefabs[i].GetComponent<Artifact>().Description);
            artifactItems[i].GetComponent<Item>().artifact = artifactPrefabs[i].GetComponent<Artifact>();

            artifactItems[i].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Purchase";
            artifactItems[i].GetComponentInChildren<Button>().interactable = true;
        }
        for (int i = 0; i < partItems.Length; i++)
        {
            partItems[i].GetComponent<Item>().FillShopItemDetails(partPrefabs[i].Name, partPrefabs[i].Cost, partPrefabs[i].Icon,
                partPrefabs[i].Description);
            partItems[i].GetComponent<Item>().part = partPrefabs[i];

            partItems[i].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Purchase";
            partItems[i].GetComponentInChildren<Button>().interactable = true;
        }

        healthText.text = "Health : " + playerDataManager.CurrentHealth.ToString() + "/" + playerDataManager.MaxHealth.ToString();
    }
    public void BuyPlant(int itemIndex)
    {
        if (playerDataManager.Money >= plantItems[itemIndex - 1].GetComponent<Item>().plant.Cost) 
        {
            playerDataManager.Money = playerDataManager.Money - plantItems[itemIndex - 1].GetComponentInChildren<Item>().plant.Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddPlant(plantItems[itemIndex - 1].GetComponent<Item>().plant.PlantType);

            purchaseText.text = "Purchased " + plantItems[itemIndex - 1].GetComponent<Item>().itemName.text;

            plantItems[itemIndex - 1].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Sold Out!";
            plantItems[itemIndex - 1].GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
            purchaseText.text = "Not enough money to buy " + plantItems[itemIndex - 1].GetComponent<Item>().itemName.text;
        }
    }
    public void BuyArtifact(int itemIndex)
    {
        if (playerDataManager.Money >= artifactItems[itemIndex - 1].GetComponent<Item>().artifact.Cost)
        {
            playerDataManager.Money = playerDataManager.Money - artifactItems[itemIndex - 1].GetComponent<Item>().artifact.Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddArtifact(artifactItems[itemIndex - 1].GetComponent<Item>().artifact.ArtifactType);

            purchaseText.text = "Purchased " + artifactItems[itemIndex - 1].GetComponent<Item>().itemName.text;

            artifactItems[itemIndex - 1].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Sold Out!";
            artifactItems[itemIndex - 1].GetComponentInChildren<Button>().interactable = false;
        }
        else
        {
            purchaseText.text = "Not enough money to buy " + artifactItems[itemIndex - 1].GetComponent<Item>().itemName.text;
        }
    }

    public void BuyPart(int itemIndex)
    {
        if (playerDataManager.Money >= partItems[itemIndex - 1].GetComponent<Item>().part.Cost)
        {
            if(playerDataManager.PlayerAttacks.Count < 4)
            {
                playerDataManager.Money = playerDataManager.Money - partItems[itemIndex - 1].GetComponent<Item>().part.Cost;
                balanceText.text = "Balance : " + playerDataManager.Money.ToString();
                playerDataManager.PlayerAttacks.Add(partItems[itemIndex - 1].GetComponent<Item>().part);
                playerCombatManager.SetUpWeapons();
                purchaseText.text = "Purchased " + partItems[itemIndex - 1].GetComponent<Item>().itemName.text;

                partItems[itemIndex - 1].GetComponentInChildren<Button>().GetComponentInChildren<TMP_Text>().text = "Sold Out!";
                partItems[itemIndex - 1].GetComponentInChildren<Button>().interactable = false;
            }
            else
            {
                purchaseText.text = "Cannot add any more parts";
            }
        }
        else
        {
            purchaseText.text = "Not enough money to buy " + partItems[itemIndex - 1].GetComponent<Item>().itemName.text;
        }
    }
    public void Heal()
    {
        if(playerDataManager.Money >= costToHeal)
        {
            if(playerDataManager.CurrentHealth < playerDataManager.MaxHealth)
            {
                playerDataManager.Money = playerDataManager.Money - costToHeal;
                balanceText.text = "Balance : " + playerDataManager.Money.ToString();

                playerDataManager.CurrentHealth = playerDataManager.MaxHealth;

                healthText.text = "Health : " + playerDataManager.CurrentHealth.ToString() + "/" + playerDataManager.MaxHealth.ToString();

                purchaseText.text = "Health Restored!";
            }
            else
            {
                purchaseText.text = "Health is already full";
            }
        }
        else
        {
            purchaseText.text = "Not enough money to Heal Tree";
        }
    }

    public void CloseShopScreen()
    {
        gameObject.SetActive(false);
    }
}
