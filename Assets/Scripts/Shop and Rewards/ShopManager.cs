using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject[] plantItems;
    [SerializeField] GameObject[] artifactItems;
    [SerializeField] GameObject[] plantPrefabs;
    [SerializeField] GameObject[] artifactPrefabs;
    [SerializeField] TMP_Text balanceText;
    [SerializeField] TMP_Text healText;

    [SerializeField] int costToHeal;

    [SerializeField] PlayerDataManager playerDataManager;
    [SerializeField] Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        Shuffle();
        balanceText.text = "Balance !!: " + playerDataManager.Money.ToString();
        healText.text = "Heal for cost : " + costToHeal.ToString();
    }
    private void OnEnable()
    {
        Shuffle();
        balanceText.text = "Balance !!: " + playerDataManager.Money.ToString();
        healText.text = "Heal for cost : " + costToHeal.ToString();
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

        for(int i = 0; i < artifactPrefabs.Length; i++)
        {
            foreach(Transform artifact in artifactItems[i].transform)
            {
                if(artifact.GetComponent<Artifact>() != null)
                {
                    Destroy(artifact.gameObject);
                }
            }
        }
    }
    void FillShop()
    {
        for(int i = 0; i < plantItems.Length; i++)
        {
            GameObject newPlantItem = Instantiate(plantPrefabs[i], plantItems[i].transform);
            plantItems[i].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = plantPrefabs[i].name.Substring(2); //Display plant name on buttons
            plantItems[i].transform.GetChild(1).GetComponent<Image>().sprite = plantPrefabs[i].GetComponent<Plant>().InventorySprite;
            newPlantItem.GetComponent<MeshRenderer>().enabled = false;
        }

/*        for (int i = 0; i < artifactItems.Length; i++)
        {
            GameObject newArtifactItem = Instantiate(artifactPrefabs[i], artifactItems[i].transform);
            artifactItems[i].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = artifactPrefabs[i].name.Substring(2); //Display plant name on buttons
            artifactItems[i].transform.GetChild(1).GetComponent<Image>().sprite = artifactPrefabs[i].GetComponent<Artifact>().InventorySprite;
            newArtifactItem.GetComponent<MeshRenderer>().enabled = false;
        }*/

    }
    public void BuyPlant(int itemIndex)
    {
        if (playerDataManager.Money >= plantPrefabs[itemIndex - 1].GetComponent<Plant>().Cost) //maybe makes more sense to use trancfom.getchild(itemIndex - 1)? are plantPrefabs always corresponding correctly to iteam slot?? Must check
        {
            //string itemName = transform.GetChild(itemIndex - 1).GetChild(1).name;
           // Debug.Log("Bought Item " + itemIndex + ", Plant Name : " + itemName);
            playerDataManager.Money = playerDataManager.Money - plantPrefabs[itemIndex - 1].GetComponent<Plant>().Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddPlant(plantPrefabs[itemIndex - 1].GetComponent<Plant>().PlantType);
        }
    }
    public void BuyArtifact(int itemIndex)
    {
        if (playerDataManager.Money >= transform.GetChild(itemIndex - 1).GetChild(1).GetComponent<Artifact>().Cost)
        {
            string itemName = transform.GetChild(itemIndex - 1).GetChild(1).name;
            Debug.Log("Bought Item " + itemIndex + ", Artifact Name : " + itemName);
            playerDataManager.Money = playerDataManager.Money - transform.GetChild(itemIndex - 1).GetChild(1).GetComponent<Artifact>().Cost;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();
            inventory.AddArtifact(transform.GetChild(itemIndex - 1).GetComponentInChildren<Artifact>().ArtifactType);
        }
    }
    public void Heal()
    {
        if(playerDataManager.Money >= costToHeal && playerDataManager.CurrentHealth < playerDataManager.MaxHealth)
        {
            playerDataManager.Money = playerDataManager.Money - costToHeal;
            balanceText.text = "Balance : " + playerDataManager.Money.ToString();

            playerDataManager.CurrentHealth = playerDataManager.MaxHealth;
        }
    }
}
