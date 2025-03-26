using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    [SerializeField] GameObject[] plantPrefabs;
  
    [SerializeField] TMP_Text moneyRewardText;

    [SerializeField] bool includeUncommon = false;
    [SerializeField] bool includeRare = false;
    [SerializeField] int probablility;
    [SerializeField] int randomSpotUncommon;
    [SerializeField] int randomSpotRare;

    [SerializeField] PlayerDataManager playerDataManager;
    public CombatManager combatManager;
    [SerializeField] Inventory inventory;

    public bool forcingProbability;
    public bool mustIncludeUncommon;
    public bool mustInlcudeRare;

    public int moneyReward;
    // Start is called before the first frame update
    void Start()
    {
        ShufflePlants();
        //balanceText.text = "Balance : " + playerDataManager.Money.ToString();
    }
    private void OnEnable()
    {
        ShufflePlants();
    }
    void ShufflePlants()
    {
        ClearShop();

        includeUncommon = false;
        includeRare = false;

        for (int i = 0; i < plantPrefabs.Length; i++)
        {
            GameObject tmp = plantPrefabs[i];
            int r = Random.Range(i, plantPrefabs.Length);
            plantPrefabs[i] = plantPrefabs[r];
            plantPrefabs[r] = tmp;
        }
        probablility = Random.Range(0, 101);
        if(probablility > 50  && probablility <= 90)
        {
            includeUncommon = true;
        }
        else if(probablility > 90)
        {
            includeRare = true;
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
        if (forcingProbability)
        {
            includeUncommon = mustIncludeUncommon;
            includeRare = mustInlcudeRare;
        }

        for(int i = 0; i < items.Length; i++)
        {
            items[i].GetComponentInChildren<Item>().FillRewardItemDetails(plantPrefabs[i].name.Substring(2), plantPrefabs[i].GetComponent<Plant>().InventorySprite);
            GameObject newPlantItem = Instantiate(plantPrefabs[i], items[i].transform);
            newPlantItem.GetComponent<MeshRenderer>().enabled = false;
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
