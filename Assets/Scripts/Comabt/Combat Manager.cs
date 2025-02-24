using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using static UnityEditor.Progress;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    public Player_Combat_Manager player_Combat_Manager;
    public ComabtUI_Manager combatUIManager;
    public ComabtObject currentComabt;
    public PlayerDataManager player;
    public GardenManager garden;
   [SerializeField] private List<Transform> spawnLocations;
    private List<GameObject> enemyObjects;
    private  List<Enemy> enemyData;
    bool playerFreeze = false;
    //number of enemies to select
    int maxTargets;
    bool isTrageting = false;


    public GameObject CombatWonScreen;


    // Start is called before the first frame update
    void Start()
    {
        
        SetUpEnemies();
        //Get attacks from player
        ComabatMenuSetUp();
        player_Combat_Manager.PlayerStartTurn();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    /// <summary>
	/// Set Up all starting enemies in an encounter
	/// </summary>
    void SetUpEnemies()
    {
        //Sapwn enemy prefabs and place them in the backline or front line.
        //Currently the game is limited to 3 enemies each.
        enemyData = new List<Enemy>();
        enemyObjects = new List<GameObject>();
        combatUIManager.combat = (this);

        for (int i = 0; i < currentComabt.enemies.Count; i++)
        {
            enemyObjects.Add (Instantiate(currentComabt.enemies[i], spawnLocations[i]));
            enemyData.Add((enemyObjects[i].GetComponent<Enemy>()));
            enemyData[i].manager = this;
            enemyData[i].enemyID = i;
            enemyData[i].gameObject.name = "Enemy " + i;
            combatUIManager.AddEnemyHealth(enemyData[i]);
            combatUIManager.SetHealth(enemyData[i]);

        }

        AllEnemeiesSrartRound();
    }

    public void AllEnemeiesSrartRound()
    {
        foreach (Enemy enemy in enemyData)
        {
            enemy.StartRound();
        }
    }
    public void ComabatMenuSetUp()
    {
        player_Combat_Manager.SetUp(player,garden,this);
    }
    public IEnumerator StartShooting(Part_SO part)
    {
        List<Enemy> targetEnemies = GetTargets(part);
        //decelect
        foreach (GameObject item in enemyObjects)
        {
            item.GetComponent<SpriteRenderer>().color = Color.white;
        }
        foreach (Enemy item in targetEnemies)
        {
            item.GetComponent<SpriteRenderer>().color = Color.red;
            item.attacked(part,player_Combat_Manager);
        }

        yield return new WaitForSeconds(1);



       // current_Attack = part;
        //attack_CostBox.text = current_Attack.manaCost.ToString();
        //attack_TextBox.text = current_Attack.attackText.ToString();
        //isTrageting = false;

    }


    public List<Enemy> GetTargets(Part_SO part)
    {
        List<Enemy> targetEnemies = new List<Enemy>();
        switch (part.targetingType)
        {
            case TargetingType.Self:
                break;
            case TargetingType.Graden:
                break;
            case TargetingType.traget:
                for (int i = 0; i < part.targetNum; i++)
                {
                    if(enemyData.Count>i)
                    targetEnemies.Add(enemyData[i]);
                }
                break;
            case TargetingType.all:
                foreach (Enemy item in enemyData)
                {
                    targetEnemies.Add(item);
                }
                break;
            default:
                break;
        }
        return targetEnemies;
    }

    public void SetEnemyAttackVisuals()
    {
        foreach (GardenTile item in player.Garden)
        {
            item.IsAttacked = false;
        }
        foreach (Enemy enemy in enemyData)
            {
            enemy.MarkMapBeforeAttack();
            }

    }

    public void EnemyTurn()
    {
        foreach (GardenTile item in player.Garden)
        {
            item.IsAttacked = false;
        }
      
        foreach (Enemy enemy in enemyData)
        {
            if (enemy.currentCoolDown == 0)
            {
                EnemyAttack_SO enemyAttack = enemy.PlayTurn();

                player_Combat_Manager.ApplyDamageToGarden(enemy, enemyAttack);
            }

        }
        playerFreeze = false;

       
        player_Combat_Manager.PlayerStartTurn();
        AllEnemeiesSrartRound();
    }
    

 
 
    public void EndPlayerTurn()
    {
        if (!playerFreeze)
        {
            playerFreeze = true;

            // Reset the current actions for the player
            player.CurrentActions = player.MaxActions;

            StartCoroutine(player_Combat_Manager.PlayerTurn());
        }
    }
    
    public void EnemyDied(Enemy enemy)
    {
        for (int i = 0; i < enemyData.Count; i++)
        {
            if (enemyData[i]== enemy)
            {
                combatUIManager.KillEnemy(enemy);
                enemyObjects.Remove(enemy.gameObject);
                enemyData.Remove(enemy.GetComponent<Enemy>());

                Destroy(enemy.gameObject);
                

            }
        } 
        if(enemyData.Count==0)
        {
            CombatWonScreen.SetActive(true);
        }
    }
    
    ///               ///
    ///Code Grave Yard///
    ///               ///
    /*   public void SelectEnemy(Enemy enemy)
       {
           if(selectedEnemis.Count>=maxTargets)
           {
              selectedEnemis.Dequeue().GetComponent<SpriteRenderer>().color = Color.white;
           }
           selectedEnemis.Enqueue(enemy);
           enemy.GetComponent<SpriteRenderer>().color = Color.red;
       }

       //Used for selecting enemies, should probably find a better way of doing this
       void MouseClick()
       {
           Debug.Log("click");
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;

           if (Physics.Raycast(ray, out hit))
           {
               if (hit.collider != null && hit.collider.GetComponent<Enemy>() != null)
               {
                   Debug.Log("Right-clicked on: " + hit.collider.gameObject.name);
                   Enemy enemy = hit.collider.GetComponent<Enemy>();
                   SelectEnemy(enemy);
               }
           }
       }

       */

}
