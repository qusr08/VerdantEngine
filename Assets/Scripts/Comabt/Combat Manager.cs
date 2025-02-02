using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    public Player_Combat_Manager player_Combat_Manager;
    public ComabtObject currentComabt;
    public PlayerData player;
    public GardenManager garden;
   [SerializeField] private List<Transform> spawnLocations;
    private GameObject[] enemyObjects;
    private  Enemy [] enemyData;

    //number of enemies to select
    int maxTargets;
    bool isTrageting = false;


    


    // Start is called before the first frame update
    void Start()
    {
        
        SetUpEnemies();
        //Get attacks from player
        ComabatMenuSetUp();

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
        enemyData = new Enemy[currentComabt.enemies.Count];
        enemyObjects = new GameObject[currentComabt.enemies.Count];

        for (int i = 0; i < currentComabt.enemies.Count; i++)
        {
            enemyObjects[i] = (Instantiate(currentComabt.enemies[i], spawnLocations[i]));
            enemyData[i]=(enemyObjects[i].GetComponent<Enemy>());
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
            item.attacked(part);
        }

        yield return new WaitForSeconds(3);



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

    
    public void EnemyTurn()
    {
        foreach (GameObject enemy in enemyObjects)
        {
            EnemyAttack_SO enemyAttack = enemy.GetComponent<Enemy>().PlayTurn();
            player.cuurentHealth -= enemyAttack.damage;
            Debug.Log(enemy.name + " attacked the player using " + enemyAttack.attackName + " dealing " + enemyAttack.damage + " to the player");
        }
    }

    public void EndPlayerTurn()
    {
        StartCoroutine(player_Combat_Manager.PlayerTurn());
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
