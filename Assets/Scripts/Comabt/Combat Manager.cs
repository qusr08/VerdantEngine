using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    public ComabtObject currentComabt;
    public PlayerData player;
    public GardenManager garden;
   [SerializeField] private List<Transform> spawnLocations;
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> frontEnemies = new List<GameObject>();
    private List<GameObject> backEnemies = new List<GameObject>(); 

    //Current attack being concidered
    private int current_Attack_Index;
    //Currently selected enemies to attack
    private Queue<Enemy> selectedEnemis = new Queue<Enemy>();
    //number of enemies to select
    int maxTargets;
    bool isSelecting = false;

    [Header("UI")]
    private List <Attack_SO> playerAttacks;
    public List<Image> attackIcons;
    public List<TMP_Text> attack_Names;
    public TMP_Text attack_TextBox;
    public TMP_Text attack_CostBox;
    


    // Start is called before the first frame update
    void Start()
    {
        
        SetUpEnemies();
        //Get attacks from player
        playerAttacks = player.CurrentAttacks;
        ComabatMenuSetUp();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isSelecting) // Right Click (0 for Left Click)
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
        for (int i = 0; i < currentComabt.frontLineEnemies.Count; i++)
        {
            frontEnemies.Add(Instantiate(currentComabt.frontLineEnemies[i], spawnLocations[i]));
            enemies.Add(frontEnemies[i]);
        }
        for (int i = 0; i < currentComabt.backLineLineEnemies.Count; i++)
        {
            backEnemies.Add(Instantiate(currentComabt.backLineLineEnemies[i], spawnLocations[i + 3]));
            enemies.Add(backEnemies[i]);
            
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
    public void AttackChoosen(int index)
    {
        current_Attack_Index = index;
        attack_CostBox.text = playerAttacks[current_Attack_Index].manaCost.ToString();
        attack_TextBox.text = playerAttacks[current_Attack_Index].attackText.ToString();
        isSelecting = false;

        //decelect
        while (selectedEnemis.Count > 0)
        {
            Enemy enemy = selectedEnemis.Dequeue();
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
        }

        switch (playerAttacks[current_Attack_Index].targetingType)
        {
            case TargetingType.FrontLine:
                foreach (GameObject item in frontEnemies)
                {
                    maxTargets = 3;
                    SelectEnemy(item.GetComponent<Enemy>());
                }
                break;
            case TargetingType.BackLine:
                foreach (GameObject item in backEnemies)
                {
                    maxTargets = 3;
                    SelectEnemy(item.GetComponent<Enemy>());
                }
                break;
            case TargetingType.Self:
                break;
            case TargetingType.Graden:
                break;
            case TargetingType.traget:
                isSelecting = true;
                maxTargets = playerAttacks[current_Attack_Index].targetNum;
                break;
            default:
                break;
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
            enemy.attacked(playerAttacks[current_Attack_Index]);
        }
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
}
