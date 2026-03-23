using System.Collections.Generic;
using TheCatacombsOfVelhart.Scripts.Data.GameEnums;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

public interface IAbility {
	public float Magnitude {get;set;}
	public int AbilityCircle {get;set;}
	
	public string Name {get;set;}
	public string Description {get;set;}
	
	public AbilityType Type {get;set;}
}

public interface ISelfCastAbility : IAbility {
	public void Use(Creature initiator);
}

public interface ITargetedAbility : IAbility {
	public void Use(Creature initiator, Creature target);
}