using System.Collections.Generic;
using TheCatacombsOfVelhart.Scripts.GameEnums.Data;

namespace TheCatacombsOfVelhart.Scripts.GameEnums.BaseObjects;

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