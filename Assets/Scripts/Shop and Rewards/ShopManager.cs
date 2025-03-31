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
    [SerializeField] GameObject[] partPrefabs;

    [SerializeField] int[] plantProbablility;
    [SerializeField] TMP_Text healthText;

    [SerializeField] TMP_Text balanceText;
    [SerializeField] TMP_Text purchaseText;

    [SerializeField] TMP_Text healCost;
    [SerializeField] int costToHeal;

    [SerializeField] PlayerDataManager playerDataManager;
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
        healCost.text = "Heal for cost : " + costToHeal.ToString();
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
        ClearShop();

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
            GameObject tmp = partPrefabs[i];
            int r = Random.Range(i, partPrefabs.Length);
            partPrefabs[i] = partPrefabs[r];
            partPrefabs[r] = tmp;
        }

        FillShop();
    }
    void ClearShop()
    {
        purchaseText.text = "";

        for (int i = 0; i < plantItems.Length; i++)
        {
            foreach(Transform plant in plantItems[i].transform)
            {
                if(plant.GetComponent<Plant>() != null)
                {
                    Destroy(plant.gameObject);
                }
            }
        }

/*        for(int i = 0; i < artifactItems.Length; i++)
        {
            foreach(Transform artifact in artifactItems[i].transform)
            {
                if(artifact.GetComponent<Artifact>() != null)
                {
                    Destroy(artifact.gameObject);
                }
            }
        }*/

/*        for (int i = 0; i < partItems.Length; i++)
        {
            foreach (Transform part in partItems[i].transform)
            {
                if (part.GetComponent<Part>() != null)
                {
                    Destroy(part.gameObject);
                }
            }
        }*/
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
                plantItems[i].GetComponentInChildren<Item>().FillShopItemDetails(commonPlants[commonIndex].name.Substring(2), commonPlants[commonIndex].GetComponent<Plant>().Cost, commonPlants[commonIndex].GetComponent<Plant>().InventorySprite, commonPlants[commonIndex].GetComponent<Plant>().Description);
                GameObject newPlantItem = Instantiate(commonPlants[commonIndex], plantItems[i].transform);
                newPlantItem.GetComponent<MeshRenderer>().enabled = false;
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
                plantItems[i].GetComponentInChildren<Item>().FillShopItemDetails(uncommonPlants[uncommonIndex].name.Substring(2), uncommonPlants[uncommonIndex].GetComponent<Plant>().Cost, uncommonPlants[uncommonIndex].GetComponent<Plant>().InventorySprite, uncommonPlants[uncommonIndex].GetComponent<Plant>().Description);
                GameObject newPlantItem = Instantiate(uncommonPlants[uncommonIndex], plantItems[i].transform);
                newPlantItem.GetComponent<MeshRenderer>().enabled = false;
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
                plantItems[i].GetComponentInChildren<Item>().FillShopItemDetails(rarePlants[rareIndex].name.Substring(2), rarePlants[rareIndex].GetComponent<Plant>().Cost, rarePlants[rareIndex].GetComponent<Plant>().InventorySprite, rarePlants[rareIndex].GetComponent<Plant>().Description);
                GameObject newPlantItem = Instantiate(rarePlants[rareIndex], plantItems[i].transform);
                newPlantItem.GetComponent<MeshRenderer>().enabled = false;
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
/*        for (int i = 0; i < artifactItems.Length; i++)
        {
            artifactItems[i].GetComponentInChildren<Item>().FillShopItemDetails(artifactPrefabs[i].name.Substring(2), artifactPrefabs[i].GetComponent<Artifact>().Cost, artifactPrefabs[i].GetComponent<Artifact>().InventorySprite,
                artifactPrefabs[i].GetComponent<Artifact>().Description);
        }*/
/*        for (int i = 0; i < partItems.Length; i++)
        {
            partItems[i].GetComponentInChildren<Item>().FillShopItemDetails(partPrefabs[i].name.Substring(2), partPrefabs[i].GetComponent<Part>().Cost, partPrefabs[i].GetComponent<Part>().InventorySprite,
                partPrefabs[i].GetComponent<Part>().Description);
        }*/

        healthText.text = "Health : " + playerDataManager.CurrentHealth.ToString() + "/" + playerDataManager.MaxHealth.ToString();
    }
    public void BuyPlant(int itemIndex)
    {
        if (playerDataManager.Money >= plantItems[itemIndex - 1].GetComponentInChildren<Plant>().Cost) 
        {
            playerDataManager.Money = playerDataManager.Money - plantItems[itemIndex - 1].GetComponentInChildren<Plant>().Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddPlant(plantItems[itemIndex - 1].GetComponentInChildren<Plant>().PlantType);

            purchaseText.text = "Purchased " + plantItems[itemIndex - 1].GetComponent<Item>().itemName.text;
        }
        else
        {
            purchaseText.text = "Not enough money to buy " + plantItems[itemIndex - 1].GetComponent<Item>().itemName.text;
        }
    }
    public void BuyArtifact(int itemIndex)
    {
        if (playerDataManager.Money >= artifactItems[itemIndex - 1].GetComponentInChildren<Artifact>().Cost)
        {
            playerDataManager.Money = playerDataManager.Money - artifactItems[itemIndex - 1].GetComponentInChildren<Artifact>().Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddArtifact(artifactItems[itemIndex - 1].GetComponentInChildren<Artifact>().ArtifactType);

            purchaseText.text = "Purchased " + artifactItems[itemIndex - 1].GetComponent<Item>().itemName.text;
        }
        else
        {
            purchaseText.text = "Not enough money to buy " + artifactItems[itemIndex - 1].GetComponent<Item>().itemName.text;
        }
    }

/*    public void BuyPart(int itemIndex)
    {
        if (playerDataManager.Money >= partItems[itemIndex - 1].GetComponentInChildren<Part>().Cost)
        {
            playerDataManager.Money = playerDataManager.Money - partItems[itemIndex - 1].GetComponentInChildren<Part>().Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddArtifact(partItems[itemIndex - 1].GetComponentInChildren<Part>().PartType);
        }
    }*/
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
