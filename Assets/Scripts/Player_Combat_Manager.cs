using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Combat_Manager : MonoBehaviour
{
     List<Part_SO> parts;
    GardenManager garden;
    CombatManager combatManager;
    public Transform weaponMenuPerent;
    [HideInInspector]public List<WeaponMenuItem> MenuObjects = new List<WeaponMenuItem>();
    public GameObject weaponMenuObject;
    int energy = 0;
    public TextMeshProUGUI energyText;
    public PlayerDataManager playerData;
    public GameObject damageIndicatorPrefab;
    public void SetUp(PlayerDataManager data, GardenManager garden, CombatManager combatManager)
    {
        playerData = data;
        this.combatManager = combatManager;
        this.garden = garden;
        parts = playerData.currentParts;
        foreach (Part_SO part in parts )
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(weaponMenuObject, weaponMenuPerent);
            MenuObjects.Add(spawnedItem.GetComponent<WeaponMenuItem>());
            spawnedItem.GetComponent<WeaponMenuItem>().SetUp(part);

        }
    }

    public void PlayerStartTurn()
    {
        energy = garden.CountPlants(new List<PlantType>() { PlantType.POWER_FLOWER }, null);
        energyText.text = energy.ToString();
       

    }

    public IEnumerator PlayerTurn()
    {
        energyText.text = energy.ToString();
        foreach (WeaponMenuItem part in MenuObjects)
        {

            part.part.coolDown--;
            if (part.part.coolDown <= 0 && (energy-part.part.manaCost)>0)
            {
                energy -= part.part.manaCost;
                energyText.text = energy.ToString();
                part.gameObject.GetComponent<Image>().color = Color.red;


                part.part.coolDown = part.part.maxCoolDown;

                yield return combatManager.StartShooting(part.part);

                part.gameObject.GetComponent<Image>().color = Color.white;

                // Fire the part after targeting is complete (or immediately if no targeting needed)
            }
            else
            {
                part.part.coolDown= 1;
            }
            part.coolDownText.text = part.part.coolDown.ToString();
            
        }
        combatManager.EnemyTurn();
    }

    public int GetAddedDamage()
    {
        int powerAdded;
        powerAdded = garden.CountPlants(new List<PlantType>() { PlantType.EMPOWEROOT }, null);
        return (powerAdded);
    }

    public void ApplyDamageToGarden(Enemy enemy, EnemyAttack_SO attack)
    {
        if (!attack.lineAttackIsHorizental && enemy.FinalAim.Count == playerData.GardenSize)
        { 
            playerData.CurrentHealth -= attack.damage;
            Debug.Log(enemy.name + " attacked the player using " + attack.attackName + " dealing " + attack.damage + " to the player");

        }
        else
        {
            GardenTile tileHit = enemy.FinalAim[enemy.FinalAim.Count - 1];
            if (tileHit != null && tileHit.GardenPlaceable != null)
            {
                Debug.Log(enemy.name + " attacked the player using " + attack.attackName + " dealing " + attack.damage + " to the " + tileHit.GardenPlaceable.name);

                // Spawn the damage indicator
                GameObject indicator = Instantiate(damageIndicatorPrefab, tileHit.transform.position, tileHit.GardenPlaceable.transform.rotation);

                // Set the damage text
                indicator.GetComponent<DamageIndicator>().SetDamage(attack.damage);


                tileHit.GardenPlaceable.HealthStat.CurrentValue -= attack.damage;
            }
            else
            {
                Debug.Log("Taeget got killed before turn");
            }

        }
    }







}
