using System.Collections.Generic;

namespace TheCatacombsOfVelhart.Scripts.GameEnums.BaseObjects;

public interface IAbility {
	public float Magnitude {get;set;}
	public int AbilityCircle {get;set;}
	
	public string Name {get;set;}
	public string Description {get;set;}

	/// <summary>
	/// function returns if ability used successfully or not.
	/// If not - something is blocking using the ability
	/// </summary>
	bool Use(ICreature initiator, List<ICreature> targets);
}