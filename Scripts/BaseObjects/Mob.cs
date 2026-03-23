using System;
using System.Collections.Generic;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

public class Mob : Creature {

	/// <summary>
	/// Placeholder for editing in future
	/// List of abilities the mob is wanting to use in the current turn
	/// This is needed for showing the player the mob abilities queue so the player can build a strategy for the current turn
	/// </summary>
	public List<IAbility> AbilitiesQueue;
	
	public void TakeTurn(Fight fight) {
		// placeholder functional for future editing
		// mob chooses random ability from Abilities list and uses it on all Players
		IAbility randomAbility = Abilities[new Random().Next(0, Abilities.Count)];
		if (randomAbility is ISelfCastAbility selfCastAbility) {
			foreach (PlayerBase player in fight.PlayerTeam) {
				selfCastAbility.Use(player);
			}
		}
	}
}