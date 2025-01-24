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
    private List<GameObject> enemies;
    private int current_Attack_Index;

    [Header("UI")]
    private List <Attack_SO> playerAttacks;
    public List<Image> attackIcons;
    public List<TMP_Text> attack_Names;
    public TMP_Text attack_TextBox;
    public TMP_Text attack_CostBox;
    


    // Start is called before the first frame update
    void Start()
    {
        //Sapwn enemy prefabs and place them in the backline or front line.
        //Currently the game is limited to 3 enemies each.
        for (int i = 0; i < currentComabt.frontLineEnemies.Count; i++)
        {
            enemies.Add(Instantiate(currentComabt.frontLineEnemies[i], spawnLocations[i]));
        }
        for (int i = 0; i < currentComabt.backLineLineEnemies.Count; i++)
        {
            enemies.Add(Instantiate(currentComabt.backLineLineEnemies[i], spawnLocations[i+3]));
        }

        //Get attacks from player
        playerAttacks = player.CurrentAttacks;
        ComabatMenuSetUp();

    }

    // Update is called once per frame
    void Update()
    {
        
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

    }
}
