using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum GameState
{
	IDLE, COMBAT
}

public class CombatUIManager : MonoBehaviour
{
	[SerializeField] private CombatManager combatManager;
	[SerializeField] private PlayerCombatManager playerCombatManager;
	[SerializeField] private GameObject enemyHealthUIPrefab;
	[SerializeField] private Transform healthUIContainer;
	[SerializeField] private List<EnemyHealthUIObject> enemyHealthUIObjects = new List<EnemyHealthUIObject>();
	[SerializeField] private RectTransform lowerPanel;
	[SerializeField] private GameObject moveCounter;
	[SerializeField] private GameObject resetTurnButton;
	[SerializeField] private GameObject endTurnButton;
	[SerializeField] private GameObject viewMapButton;
    [SerializeField] private GameObject EnemyViewPort;

    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject plantUIPanal;
	[SerializeField] private GameObject GoBackToMap;
    [SerializeField] private GameObject enemyUIContainer;
    [SerializeField] private GameObject enemyUIBackGround;

    [Space]
	[SerializeField] private GameState _gameState;

	/// <summary>
	/// The current combat state of the game
	/// </summary>
	public GameState GameState
	{
		get => _gameState;
		set
		{
			_gameState = value;

			switch (_gameState)
			{
				case GameState.IDLE:
					enemyUIContainer.SetActive(false);
                    enemyUIBackGround.SetActive(false);

                    lowerPanel.gameObject.SetActive(false);

                    moveCounter.SetActive(false);
			//resetTurnButton.SetActive(false);
					endTurnButton.SetActive(false);
					viewMapButton.SetActive(true);


					break;
				case GameState.COMBAT:
                    enemyUIContainer.SetActive(true);
                    enemyUIBackGround.SetActive(true);

                    lowerPanel.gameObject.SetActive(true); 

                    moveCounter.SetActive(true);
//esetTurnButton.SetActive(true);
					endTurnButton.SetActive(true);
					viewMapButton.SetActive(false);

				

					break;
			}
		}
	}

	private void Awake()
	{
		// Update all of the UI elements based on whatever the gamestate is set to
		//GameState = GameState.IDLE;
	}

	public void AddEnemyHealth (Enemy enemy) {
		EnemyHealthUIObject enemyHealthUIObject = Instantiate(enemyHealthUIPrefab, healthUIContainer).GetComponent<EnemyHealthUIObject>( );
		enemyHealthUIObject.Enemy = enemy;
		enemyHealthUIObject.Hover = popUp.GetComponent<InfoPopUp>();
        enemyHealthUIObjects.Add(enemyHealthUIObject);
	}

	public void UpdateHealth (Enemy enemy) {
		foreach (EnemyHealthUIObject enemyHealthUIObject in enemyHealthUIObjects) {
			if (enemyHealthUIObject.Enemy.EnemyID == enemy.EnemyID) {
				enemyHealthUIObject.UpdateHealth( );
				break;
			}
		}
	}

	public void UpdateCooldown (Enemy enemy) {
		foreach (EnemyHealthUIObject enemyHealthUIObject in enemyHealthUIObjects) {
			if (enemyHealthUIObject.Enemy.EnemyID == enemy.EnemyID) {
				enemyHealthUIObject.UpdateCoolDown( );
				break;
			}
		}
	}

	public void KillEnemy (Enemy enemy) {
		foreach (EnemyHealthUIObject enemyHealthUIObject in enemyHealthUIObjects) {
			if (enemyHealthUIObject.Enemy == enemy) {
				enemyHealthUIObject.Kill( );
				enemyHealthUIObject.X.SetActive(true);

            }
		}
	}

	public void ResetTurn_RestUI(List<Enemy> enemies)
	{
		PurgeList();

        foreach (var enemy in enemies)
		{
			AddEnemyHealth(enemy);
			UpdateHealth(enemy);
			UpdateCooldown(enemy);
			
		}
	}

	public void PurgeList()
	{

		Debug.Log("ListPurged");
        while(enemyHealthUIObjects.Count > 0)
        {
			Destroy(enemyHealthUIObjects[0].gameObject);
			enemyHealthUIObjects.RemoveAt(0);
        }
    }




    /// <summary>
    /// Set the game state of the game from a function. This is used for UI elements as it cannot set variables directly
    /// </summary>
    /// <param name="gameState">The integer representation of the game state to set</param>
    public void SetGameState(int gameState)
	{
		GameState = (GameState)gameState;
	}
}
