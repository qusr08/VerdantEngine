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
    private List<GameObject> enemies = new List<GameObject>();

    //Current attack being concidered
    private Part_SO current_Attack;
    //Currently selected enemies to attack
    private Queue<Enemy> selectedEnemis = new Queue<Enemy>();
    //number of enemies to select
    int maxTargets;
    bool isTrageting = false;

    [Header("UI")]
    private List <Part_SO> playerAttacks;
    public List<Image> attackIcons;
    public List<TMP_Text> attack_Names;
    public TMP_Text attack_TextBox;
    public TMP_Text attack_CostBox;
    


    // Start is called before the first frame update
    void Start()
    {
        
        SetUpEnemies();
        //Get attacks from player
        playerAttacks = player.currentParts;
        ComabatMenuSetUp();
        player_Combat_Manager.SetUp(player, garden, this);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isTrageting) // Right Click (0 for Left Click)
        {

            MouseClick();
        }
    }
    /// <summary>
	/// Set Up all starting enemies in an encounter
	/// </summary>
    void SetUpEnemies()
    {
        //Sapwn enemy prefabs and place them in the backline or front line.
        //Currently the game is limited to 3 enemies each.
        for (int i = 0; i < currentComabt.enemies.Count; i++)
        {
            enemies.Add(Instantiate(currentComabt.enemies[i], spawnLocations[i]));
        }
    }
    public void ComabatMenuSetUp()
    {
        for (int i = 0; i < playerAttacks.Count; i++)
        {
            attackIcons[i].sprite = playerAttacks[i].icon;
            attack_Names[i].text = playerAttacks[i].attackName;
        }
    }
    public IEnumerator StartTargeting(Part_SO part, System.Action onComplete)
    {
        current_Attack = part;
        attack_CostBox.text = current_Attack.manaCost.ToString();
        attack_TextBox.text = current_Attack.attackText.ToString();
        isTrageting = false;
        

        //decelect
        while (selectedEnemis.Count > 0)
        {
            Enemy enemy = selectedEnemis.Dequeue();
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
        }

        switch (part.targetingType)
        {
        
            case TargetingType.Self:
                break;
            case TargetingType.Graden:
                break;
            case TargetingType.traget:
                isTrageting = true;
                maxTargets = current_Attack.targetNum;
                break;
            case TargetingType.all:
                foreach (GameObject enemy in enemies)
                {
                    SelectEnemy( enemy.GetComponent<Enemy>());
                }
                break;
            
            default:
                break;
        }
        while (isTrageting)
        {
            yield return null;
        }

    }
    public void SelectEnemy(Enemy enemy)
    {
        if(selectedEnemis.Count>=maxTargets)
        {
           selectedEnemis.Dequeue().GetComponent<SpriteRenderer>().color = Color.white;
        }
        selectedEnemis.Enqueue(enemy);
        enemy.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void AttackConfirmed()
    {
        foreach (Enemy enemy in selectedEnemis)
        {
            enemy.attacked(current_Attack);
        }
        EnemyTurn();
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

    public void EnemyTurn()
    {
        foreach (GameObject enemy in enemies)
        {
         EnemyAttack_SO enemyAttack = enemy.GetComponent<Enemy>().PlayTurn();
            player.cuurentHealth -= enemyAttack.damage;
            Debug.Log(enemy.name + " attacked the player using " + enemyAttack.attackName + " dealing " + enemyAttack.damage + " to the player");
        }
    }
    
    public void StartShootingPhase()
    {
        StartCoroutine(player_Combat_Manager.PlayerTurn());
    }
}
