using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject[] items;
    [SerializeField] GameObject[] plantPrefabs;
    [SerializeField] PlayerWallet_Placeholder wallet;
    [SerializeField] TMP_Text balanceText;

    // Start is called before the first frame update
    void Start()
    {
        ShufflePlants();
        FillShop();

        balanceText.text = "Balance : " + wallet.balance.ToString();
    }
    void ShufflePlants()
    {
        for (int i = 0; i < plantPrefabs.Length; i++)
        {
            GameObject tmp = plantPrefabs[i];
            int r = Random.Range(i, plantPrefabs.Length);
            plantPrefabs[i] = plantPrefabs[r];
            plantPrefabs[r] = tmp;
        }
    }
    void FillShop()
    {
        for(int i = 0; i < items.Length; i++)
        {
            Instantiate(plantPrefabs[i], items[i].transform);
            items[i].transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = plantPrefabs[i].name;
        }
    }
    public void BuyPlant(int itemIndex)
    {
        string itemName = transform.GetChild(itemIndex - 1).GetChild(1).name;
        Debug.Log("Bought Item " + itemIndex + ", Plant Name : " + itemName);
        wallet.balance = wallet.balance - transform.GetChild(itemIndex - 1).GetChild(1).GetComponent<Plant_Placeholder>().cost;
        balanceText.text = "Balance : " + wallet.balance.ToString();
    }
}
