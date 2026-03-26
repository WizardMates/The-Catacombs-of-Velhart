using TheCatacombsOfVelhart.Scripts.Core.Data;
using TheCatacombsOfVelhart.Scripts.Game.Creatures;

namespace TheCatacombsOfVelhart.Scripts.Game.Abilities;

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