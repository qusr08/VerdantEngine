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

    [SerializeField] TMP_Text healthText;

    [SerializeField] TMP_Text balanceText;

    [SerializeField] TMP_Text healCost;
    [SerializeField] int costToHeal;

    [SerializeField] PlayerDataManager playerDataManager;
    [SerializeField] Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
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
    void Shuffle()
    {
        ClearShop();

        for (int i = 0; i < plantPrefabs.Length; i++)
        {
            GameObject tmp = plantPrefabs[i];
            int r = Random.Range(i, plantPrefabs.Length);
            plantPrefabs[i] = plantPrefabs[r];
            plantPrefabs[r] = tmp;
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
        for(int i = 0; i < plantItems.Length; i++)
        {
            plantItems[i].GetComponentInChildren<Item>().FillShopItemDetails(plantPrefabs[i].name.Substring(2), plantPrefabs[i].GetComponent<Plant>().Cost, plantPrefabs[i].GetComponent<Plant>().InventorySprite,
                plantPrefabs[i].GetComponent<Plant>().Description);
            GameObject newPlantItem = Instantiate(plantPrefabs[i], plantItems[i].transform);
            newPlantItem.GetComponent<MeshRenderer>().enabled = false;
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
        if (playerDataManager.Money >= plantItems[itemIndex - 1].GetComponentInChildren<Plant>().Cost) //maybe makes more sense to use trancfom.getchild(itemIndex - 1)? are plantPrefabs always corresponding correctly to iteam slot?? Must check
        {
            playerDataManager.Money = playerDataManager.Money - plantItems[itemIndex - 1].GetComponentInChildren<Plant>().Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddPlant(plantItems[itemIndex - 1].GetComponentInChildren<Plant>().PlantType);
        }
    }
    public void BuyArtifact(int itemIndex)
    {
        if (playerDataManager.Money >= artifactItems[itemIndex - 1].GetComponentInChildren<Artifact>().Cost)
        {
            playerDataManager.Money = playerDataManager.Money - artifactItems[itemIndex - 1].GetComponentInChildren<Artifact>().Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddArtifact(artifactItems[itemIndex - 1].GetComponentInChildren<Artifact>().ArtifactType);
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
        if(playerDataManager.Money >= costToHeal && playerDataManager.CurrentHealth < playerDataManager.MaxHealth)
        {
            playerDataManager.Money = playerDataManager.Money - costToHeal;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();

            playerDataManager.CurrentHealth = playerDataManager.MaxHealth;
        }
    }

    public void CloseShopScreen()
    {
        gameObject.SetActive(false);
    }
}
