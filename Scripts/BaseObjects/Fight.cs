using System.Collections.Generic;
using System.Linq;
using TheCatacombsOfVelhart.Scripts.Data.GameEnums;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

/// <summary>
/// Manages a single turn-based fight between a player team and an enemy team.
/// Handles turn order, win/loss conditions, and enemy queue processing.
/// Create a new instance at the start of each fight.
/// </summary>
public class Fight
{
	/// <summary>
	/// Current state of the fight.
	/// </summary>
	public FightState State { get; private set; }
	
	/// <summary>
	/// The player's team participating in the fight.
	/// </summary>
	public List<PlayerBase> PlayerTeam { get; }
	
	/// <summary>
	/// The enemy team participating in the fight.
	/// </summary>
	public List<Mob> EnemyTeam { get; }
    
	private int _currentEnemyIndex;

	/// <summary>
	/// Initializes a new fight between the given teams.
	/// Fight starts with the player's turn.
	/// </summary>
	/// <param name="playerTeam">List of players participating in the fight.</param>
	/// <param name="enemyTeam">List of enemies participating in the fight.</param>
	public Fight(List<PlayerBase> playerTeam, List<Mob> enemyTeam)
	{
		PlayerTeam = playerTeam;
		EnemyTeam = enemyTeam;
		State = FightState.PlayerTurn;
		_currentEnemyIndex = 0;
	}
	
	/// <summary>
	/// Checks for victory and starts the player's turn if the fight is still ongoing.
	/// </summary>
	private void StartPlayerTurn()
	{
		if (CheckVictory()) return;
		State = FightState.PlayerTurn;
	}
	
	/// <summary>
	/// Ends the player's turn and starts processing the enemy queue.
	/// Has no effect if it is not currently the player's turn.
	/// </summary>
	public void EndPlayerTurn()
	{
		if (State != FightState.PlayerTurn) return; // foolproof

		State = FightState.EnemyTurn;
		_currentEnemyIndex = 0;
		ProcessNextEnemy();
	}
	
	/// <summary>
	/// Processes the next living enemy in the queue.
	/// Skips dead enemies and returns the turn to the player once all enemies have acted.
	/// </summary>
	private void ProcessNextEnemy()
	{
		while (_currentEnemyIndex < EnemyTeam.Count && EnemyTeam[_currentEnemyIndex].Hp <= 0) // skipping dead enemies
		{
			_currentEnemyIndex++;
		}

		if (_currentEnemyIndex >= EnemyTeam.Count)
		{
			StartPlayerTurn();
			return;
		}

		Mob currentEnemy = EnemyTeam[_currentEnemyIndex];
		currentEnemy.TakeTurn(this);
		_currentEnemyIndex++;

		CheckDefeat();

		if (State != FightState.Defeat)
		{
			ProcessNextEnemy();
		}
	}

	/// <summary>
	/// Checks if all enemies are defeated and sets state to Victory if so.
	/// </summary>
	/// <returns>True if the player team has won.</returns>
	private bool CheckVictory()
	{
		if (EnemyTeam.All(e => e.Hp <= 0))
		{
			State = FightState.Victory;
			return true;
		}
		return false;
	}

	/// <summary>
	/// Checks if all players are defeated and sets state to Defeat if so.
	/// </summary>
	private void CheckDefeat()
	{
		if (PlayerTeam.All(e => e.Hp <= 0))
		{
			State = FightState.Defeat;
		}
	}
}