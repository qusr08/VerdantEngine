using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Combat_Manager : MonoBehaviour
{
     List<Part_SO> parts;
    GardenManager garden;
    CombatManager combatManager;
    public Transform weaponMenuPerent;
    [HideInInspector]public List<WeaponMenuItem> MenuObjects = new List<WeaponMenuItem>();
    public GameObject weaponMenuObject;

    public void SetUp( PlayerData data, GardenManager garden, CombatManager combatManager)
    {
        this.combatManager = combatManager;
        this.garden = garden;
        parts = data.currentParts;
        foreach (Part_SO part in parts )
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(weaponMenuObject, weaponMenuPerent);
            MenuObjects.Add(spawnedItem.GetComponent<WeaponMenuItem>());
            spawnedItem.GetComponent<WeaponMenuItem>().SetUp(part);

        }
    }
    public IEnumerator PlayerTurn()
    {
        foreach (WeaponMenuItem part in MenuObjects)
        {

            part.part.coolDown--;
            if (part.part.coolDown <= 0)
            {
                part.gameObject.GetComponent<Image>().color = Color.red;


                part.part.coolDown = part.part.maxCoolDown;

                yield return combatManager.StartShooting(part.part);

                part.gameObject.GetComponent<Image>().color = Color.blue;

                // Fire the part after targeting is complete (or immediately if no targeting needed)
            }
            part.coolDownText.text = part.part.coolDown.ToString();

        }
        combatManager.EnemyTurn();
    }
  

  

  



}
