using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using Unity.VisualScripting;
using static UnityEngine.EventSystems.EventTrigger;
using System.Numerics;
using UnityEditor.Experimental;

public class PlayerCombatManager : MonoBehaviour {
	[SerializeField] private GardenManager gardenManager;
	[SerializeField] public CombatManager combatManager;
	[SerializeField] private PlayerDataManager playerDataManager;
	[SerializeField] private Transform weaponMenuContainer;
	[SerializeField] private List<PlayerAttackMenuItem> weaponMenuItems = new List<PlayerAttackMenuItem>( );
	[SerializeField] private GameObject weaponMenuItemPrefab;
	[SerializeField] private TextMeshProUGUI energyText;
	[SerializeField] private GameObject damageIndicatorPrefab;
	
	[Space]
	[SerializeField] public int energy = 0;
    [HideInInspector] public int energyModifier;
	 public SpriteRenderer [] tree;
    public EnemySlider enemyAttckSliderAnimation;
	public Inventory inventory; // used for flowers to get a reference
    public GameObject cannonFlashAsset;
	public float tempAnimTimer;
	public InfoPopUp popUp;
	private void Start ( ) {
		foreach (PlayerAttackSO playerAttack in playerDataManager.PlayerAttacks) {
            PlayerAttackMenuItem attackMenuItem = Instantiate(weaponMenuItemPrefab, weaponMenuContainer).GetComponent<PlayerAttackMenuItem>();
            attackMenuItem.PlayerAttack = playerAttack;
            attackMenuItem.UIDisplay = popUp;
            if (attackMenuItem.PlayerAttack.Cooldown == 0)
                attackMenuItem.ReadyToFire();
            else
                attackMenuItem.GetOnCooldown();
            weaponMenuItems.Add(attackMenuItem);
		}
	}
	public void SetUpWeapons()
	{

		foreach (var item in weaponMenuItems)
		{
			Destroy(item.gameObject);
		}
		weaponMenuItems.Clear();
        foreach (PlayerAttackSO playerAttack in playerDataManager.PlayerAttacks)
        {
			PlayerAttackMenuItem attackMenuItem = Instantiate(weaponMenuItemPrefab, weaponMenuContainer).GetComponent<PlayerAttackMenuItem>();
            attackMenuItem.PlayerAttack = playerAttack;
            attackMenuItem.UIDisplay = popUp;
            if (attackMenuItem.PlayerAttack.Cooldown == 0)
                attackMenuItem.ReadyToFire();
            else
                attackMenuItem.GetOnCooldown();
            weaponMenuItems.Add(attackMenuItem);

            if (playerAttack.MaxCooldown>0)
            weaponMenuItems[weaponMenuItems.Count - 1].GetOnCooldown();

        }
    }

    public void PlayerStartTurn()
    {
        StartOfTurnEffects();
        energy += playerDataManager.actionCountModifier;
		energyText.text = energy.ToString();
		
    }
	public void ResetWeapons()
    {
        foreach (PlayerAttackMenuItem weaponMenuItem in weaponMenuItems)
        {
            weaponMenuItem._playerAttack.Cooldown = weaponMenuItem._playerAttack.MaxCooldown;
            weaponMenuItem.UpdateCoolDown();
            if (weaponMenuItem.PlayerAttack.Cooldown == 0)
                weaponMenuItem.ReadyToFire();
            else
                weaponMenuItem.GetOnCooldown();
        }

    }
        public void StartOfTurnEffects()
	{
        foreach (GardenPlaceable item in gardenManager.Plants)
        {
			item.OnTurnStart();

        }
        foreach (GardenPlaceable item in gardenManager.Artifacts)
        {
            item.OnTurnStart();

        }
        combatManager.SaveGameState();

    }

    public void UpdateEnergy(int value)
    {
		energy += value;
        energyText.text = energy.ToString();

    }
	public void ResetEnergy()
	{
		energyText.text = "0";
		energy = 0;
	}

    public IEnumerator PlayerTurn ( ) {
		
		foreach (PlayerAttackMenuItem weaponMenuItem in weaponMenuItems) {

			if (weaponMenuItem.PlayerAttack.Cooldown <= 0 && (energy - weaponMenuItem.PlayerAttack.ManaCost) >= 0) {
				energy -= weaponMenuItem.PlayerAttack.ManaCost;
				energyText.text = energy.ToString( );
				weaponMenuItem.GetComponent<Image>( ).color = Color.yellow;
				weaponMenuItem.PlayerAttack.Cooldown = weaponMenuItem.PlayerAttack.MaxCooldown;

				//If attack is targetting enemies, handle it
				if (weaponMenuItem.PlayerAttack.PlayerTargetingType == PlayerTargetingType.TARGET)
				{
					cannonFlashAsset.SetActive(true);
				//	yield return new WaitForSeconds(tempAnimTimer);
                 yield return combatManager.IUpdateTargetedEnemies(weaponMenuItem.PlayerAttack);
                    cannonFlashAsset.SetActive(false);

                }
                //Else, if it tagets the garden, hendle that
                else if (weaponMenuItem.PlayerAttack.PlayerTargetingType == PlayerTargetingType.GARDEN)
				{
					//If the weapon is healing, heal.
					if (weaponMenuItem.PlayerAttack.AttackType == AttackType.HEAL)
					{
						foreach (Plant plant in gardenManager.Plants)
						{
							plant.Heal(weaponMenuItem.PlayerAttack.Damage);
						}
					}
				}

				weaponMenuItem.GetComponent<Image>( ).color = Color.white;
                weaponMenuItem.PlayerAttack.Cooldown = weaponMenuItem.PlayerAttack.MaxCooldown;
                weaponMenuItem.GetOnCooldown();
                // Fire the part after targeting is complete (or immediately if no targeting needed)
            }
            else if(weaponMenuItem.PlayerAttack.Cooldown <= 0 && (energy - weaponMenuItem.PlayerAttack.ManaCost) < 0)
            {
				weaponMenuItem.PlayerAttack.Cooldown = 1;
			}
            weaponMenuItem.PlayerAttack.Cooldown--;



            if (weaponMenuItem.PlayerAttack.Cooldown <= 0)
            { 
                weaponMenuItem.ReadyToFire();
                weaponMenuItem.PlayerAttack.Cooldown = 0;
            }
            weaponMenuItem.UpdateCoolDown();

        }
        EndOfTurnEffects(); 
		StartCoroutine( combatManager.EnemyTurn( ));
	}

	public int GetAddedDamage ( ) {
		int powerAdded = 0;
		powerAdded += gardenManager.CountPlants(new List<PlantType>( ) { PlantType.BLAST_BLOOM }, null);
		return powerAdded;
	}
	public void EndOfTurnEffects()
    {
		//heathichoke & FLYTRAP vempire effects
		foreach (Plant plant in gardenManager.Plants )
        {
			plant.OnTurnEnd();
        }
        foreach (Artifact artifact in gardenManager.Artifacts)
        {
            artifact.OnTurnEnd();
        }

    }

    public IEnumerator ApplyDamageToGarden(Enemy enemy, EnemyAttackSO enemyAttack)
    {
		
		
      //  enemyAttckSliderAnimation.enemyImage.texture = enemy.Icon.texture;
      //  enemyAttckSliderAnimation.gameObject.GetComponent<PlayableDirector>().Play();
      //  yield return new WaitForSeconds((float)enemyAttckSliderAnimation.gameObject.GetComponent<PlayableDirector>().duration);
        if (enemyAttack.EnemyTargetingType == EnemyTargetingType.SHAPE)
        {
            foreach (GardenTile tile in enemy.FinalAim)
            {
				if (tile.GardenPlaceable != null)
				{
					int damageDealt = enemyAttack.Damage;

					// Deal damage to the garden placeable that was hit by the attack
					tile.GardenPlaceable.TakeDamage(enemy, damageDealt);
				}
            }
        }
        else if (!enemyAttack.IsLineAttackHorizontal && enemy.FinalAim.Count == playerDataManager.GardenSize && enemy.FinalAim[enemy.FinalAim.Count - 1].GardenPlaceable == null)
        {
            playerDataManager.CurrentHealth -= enemyAttack.Damage;
			foreach (SpriteRenderer mesh in tree)
			{
                mesh.color = Color.red;
            }
			yield return new WaitForSeconds(0.2f);
            foreach (SpriteRenderer mesh in tree)
            {
                mesh.color = Color.white;
            }
            Debug.Log(enemy.name + " attacked the player using " + enemyAttack.Name + " dealing " + enemyAttack.Damage + " to the player");
        }
        else
        {
            GardenTile tileHit = enemy.FinalAim[enemy.FinalAim.Count - 1];
            if (tileHit != null && tileHit.GardenPlaceable != null)
            {
                int damageDealt =  enemyAttack.Damage;

                // Deal damage to the garden placeable that was hit by the attack
                tileHit.GardenPlaceable.TakeDamage(enemy, damageDealt);
            }
            else
            {
                Debug.Log("Taeget got killed before turn");
            }
        }
    }

}
