using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using Unity.VisualScripting;

public class PlayerCombatManager : MonoBehaviour {
	[SerializeField] private GardenManager gardenManager;
	[SerializeField] private CombatManager combatManager;
	[SerializeField] private PlayerDataManager playerDataManager;
	[SerializeField] private Transform weaponMenuContainer;
	[SerializeField] private List<PlayerAttackMenuItem> weaponMenuItems = new List<PlayerAttackMenuItem>( );
	[SerializeField] private GameObject weaponMenuItemPrefab;
	[SerializeField] private TextMeshProUGUI energyText;
	[SerializeField] private GameObject damageIndicatorPrefab;
	[Space]
	[SerializeField] private int energy = 0;
	public EnemySlider enemyAttckSliderAnimation;

	private void Start ( ) {
		foreach (PlayerAttackSO playerAttack in playerDataManager.PlayerAttacks) {
			weaponMenuItems.Add(Instantiate(weaponMenuItemPrefab, weaponMenuContainer).GetComponent<PlayerAttackMenuItem>( ));
			weaponMenuItems[weaponMenuItems.Count - 1].PlayerAttack = playerAttack;
		}
	}

	public void PlayerStartTurn ( ) {
		energyText.text = energy.ToString( );
	}

	public IEnumerator PlayerTurn ( ) {
		energy += gardenManager.CountPlants(new List<PlantType>() { PlantType.POWER_FLOWER }, null);
		energyText.text = energy.ToString( );
		foreach (PlayerAttackMenuItem weaponMenuItem in weaponMenuItems) {
			weaponMenuItem.PlayerAttack.Cooldown--;

			if (weaponMenuItem.PlayerAttack.Cooldown <= 0 && (energy - weaponMenuItem.PlayerAttack.ManaCost) >= 0) {
				energy -= weaponMenuItem.PlayerAttack.ManaCost;
				energyText.text = energy.ToString( );
				weaponMenuItem.GetComponent<Image>( ).color = Color.red;
				weaponMenuItem.PlayerAttack.Cooldown = weaponMenuItem.PlayerAttack.MaxCooldown;

				//If attack is targetting enemies, handle it
				if(weaponMenuItem.PlayerAttack.PlayerTargetingType == PlayerTargetingType.TARGET)
				yield return combatManager.IUpdateTargetedEnemies(weaponMenuItem.PlayerAttack);
				//Else, if it tagets the garden, hendle that
				else if(weaponMenuItem.PlayerAttack.PlayerTargetingType == PlayerTargetingType.GARDEN)
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

				// Fire the part after targeting is complete (or immediately if no targeting needed)
			} else {
				weaponMenuItem.PlayerAttack.Cooldown = 1;
			}

			weaponMenuItem.UpdateCoolDown( );
		}
		EndOfTurnEffects(); 
		StartCoroutine( combatManager.EnemyTurn( ));
	}

	public int GetAddedDamage ( ) {
		int powerAdded = 0;
		powerAdded += gardenManager.CountPlants(new List<PlantType>( ) { PlantType.EMPOWEROOT }, null);
		return powerAdded;
	}
	public void EndOfTurnEffects()
    {
		//heathichoke & FLYTRAP vempire effects
		foreach (Plant plant in gardenManager.GetFilteredPlants((new List<PlantType>() { PlantType.HEARTICHOKE,PlantType.FLYTRAP })))
        {
			plant.OnTurnEnd();
        }
     

	}

	public IEnumerator ApplyDamageToGarden (Enemy enemy, EnemyAttackSO enemyAttack) {


		enemyAttckSliderAnimation.enemyImage.texture = enemy.Icon.texture;
        enemyAttckSliderAnimation.gameObject.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds((float)enemyAttckSliderAnimation.gameObject.GetComponent<PlayableDirector>().duration);
		if (!enemyAttack.IsLineAttackHorizontal && enemy.FinalAim.Count == playerDataManager.GardenSize && enemy.FinalAim[enemy.FinalAim.Count-1].GardenPlaceable==null) {
			playerDataManager.CurrentHealth -= enemyAttack.Damage;
			Debug.Log(enemy.name + " attacked the player using " + enemyAttack.Name + " dealing " + enemyAttack.Damage + " to the player");
		} else {
			GardenTile tileHit = enemy.FinalAim[enemy.FinalAim.Count - 1];
			if (tileHit != null && tileHit.GardenPlaceable != null) {
				Debug.Log(enemy.name + " attacked the player using " + enemyAttack.Name + " dealing " + enemyAttack.Damage + " to the " + tileHit.GardenPlaceable.name);

				// Calculate the amount of damage that the enemy will deal to the plant
				// Make sure to factor in the shield that the plant has as well
				// Also make sure that the damage done never goes below 0, as that would add health back to the plant
				int damageDealt = Mathf.Max(0, enemyAttack.Damage - tileHit.GardenPlaceable.ShieldStat.CurrentValue);

				// Deal damage to the garden placeable that was hit by the attack
				tileHit.GardenPlaceable.LastEnemyWhichDamagedPlaceble = enemy;
				tileHit.GardenPlaceable.TakeDamage(damageDealt);


			}
			else {
				Debug.Log("Taeget got killed before turn");
			}
		}
	}
}
