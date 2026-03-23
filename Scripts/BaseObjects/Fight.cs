using System;
using System.Collections.Generic;
using System.Linq;
using TheCatacombsOfVelhart.Scripts.Data.GameEnums;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

/// <summary>
/// <para>
/// Manages a single turn-based fight between a player and an enemy team.
/// Handles turn order, win/loss conditions, and enemy queue processing.
/// Create a new instance at the start of each fight.
/// </para>
/// <para>
/// Example of using:
/// <code>
/// Fight fight = new Fight(PlayerBase WizardPlayer, new List&lt;Mob&gt; { Skeleton, Troll, Zombie }, 500, PlayerWin, PlayerLose);
/// //Godot code: initializing fight interface and related stuff
/// //Player uses abilities and presses the End turn button => button calls fight.EndPlayerTurn()
/// </code>
///</para>
/// <para>
/// Object functions cycle logic:<br/>
/// Constructor(Player, EnemyTeam, ExperienceReward, OnPlayerWin, OnPlayerLose) =><br/>
/// => StartPlayerTurn() => Godot.ButtonPressed calls EndPlayerTurn() => ProcessNextEnemy() =><br/>
/// if played dies -> FightState = Defeat and calling OnPlayerLose delegate<br/>
/// if not -> ProcessNextEnemy() until all enemies took their turns<br/>
/// => StartPlayerTurn()<br/>
/// </para>
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
	public PlayerBase Player { get; }
	
	/// <summary>
	/// The enemy team participating in the fight.
	/// </summary>
	public List<Mob> EnemyTeam { get; }
	
	public int ExperienceReward { get; }
    
	private int _currentEnemyIndex;

	private Action OnPlayerWin;
	private Action OnPlayerLose;

	/// <summary>
	/// Initializes a new fight between the given teams.
	/// Fight starts with the player's turn.
	/// </summary>
	/// <param name="player">List of players participating in the fight.</param>
	/// <param name="enemyTeam">List of enemies participating in the fight.</param>
	/// <param name="experienceReward">Amount of experience the player will receive if win the fight</param>
	/// <param name="onPlayerWin">Function with void type which will be called when Player wins</param>
	/// <param name="onPlayerLose">Function with void type which will be called when Player loses</param>
	public Fight(PlayerBase player, List<Mob> enemyTeam, int experienceReward, Action onPlayerWin, Action onPlayerLose)
	{
		Player = player;
		EnemyTeam = enemyTeam;
		ExperienceReward = experienceReward;
		OnPlayerWin = onPlayerWin;
		OnPlayerLose = onPlayerLose;
		
		StartPlayerTurn();
	}
	
	/// <summary>
	/// Checks for victory and starts the player's turn if the fight is still ongoing.
	/// </summary>
	private void StartPlayerTurn()
	{
		State = FightState.PlayerTurn;
	}
	
	/// <summary>
	/// Ends the player's turn and starts processing the enemy queue.
	/// Has no effect if it is not currently the player's turn.
	/// </summary>
	public void EndPlayerTurn()
	{
		CheckVictory();
		if (State == FightState.Victory) {
			Player.Experience += ExperienceReward;
			OnPlayerWin();
			return;
		}
		
		State = FightState.EnemiesTurn;
		_currentEnemyIndex = 0;
		ProcessNextEnemy();
	}
	
	/// <summary>
	/// Processes the next living enemy in the queue.
	/// Skips dead enemies and returns the turn to the player once all enemies have acted.
	/// </summary>
	private void ProcessNextEnemy()
	{
		while (_currentEnemyIndex < EnemyTeam.Count && EnemyTeam[_currentEnemyIndex].State == CreatureState.Dead) // skipping dead enemies
		{
			_currentEnemyIndex++;
		}

		if (_currentEnemyIndex >= EnemyTeam.Count) // if all enemies took their turns -> return the turn to the player
		{
			StartPlayerTurn();
			return;
		}

		// Enemy takes turn
		Mob currentEnemy = EnemyTeam[_currentEnemyIndex];
		currentEnemy.TakeTurn(this);
		_currentEnemyIndex++;
		
		// checking if the player died after the enemy's turn
		CheckDefeat();
		if (State != FightState.Defeat)
		{
			ProcessNextEnemy();
		}
		else {
			OnPlayerLose();
		}
	}

	/// <summary>
	/// Checks if all enemies are defeated and sets state to Victory if so.
	/// </summary>
	/// <returns>True if the player team has won.</returns>
	private void CheckVictory()
	{
		if (EnemyTeam.All(e => e.State == CreatureState.Dead))
		{
			State = FightState.Victory;
		}
	}

	/// <summary>
	/// Checks if all players are defeated and sets state to Defeat if so.
	/// </summary>
	private void CheckDefeat()
	{
		if (Player.State == CreatureState.Dead)
		{
			State = FightState.Defeat;
		}
	}
}