using System.Collections.Generic;
using TheCatacombsOfVelhart.Scripts.Data.GameEnums;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

public interface ICreature {
	public float Hp { get; set; }
	public float MaxHp { get; set; }
	
	public List<IAbility> Abilities { get; set; }
	
	/// <summary>
	/// Dictionary of Defences creature has.
	/// Damage Type - Coefficient
	/// Final damage formula: basedamage * ( 1 - Coefficient)
	/// </summary>
	public Dictionary<DamageType, float> Defences { get; set; }
}