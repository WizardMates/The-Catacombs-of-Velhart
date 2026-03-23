using System.Collections.Generic;
using TheCatacombsOfVelhart.Scripts.Data.GameEnums;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

public abstract class Creature {
	
	private float _hp;
	public float Hp {
		get => _hp;
		set {
			_hp = value;
			if(_hp <= 0) { Die(); }
		}
	}
	
	public float MaxHp { get; set; }
	
	public List<IAbility> Abilities { get; set; }
	
	/// <summary>
	/// Dictionary of Defences creature has.
	/// Damage Type - Coefficient
	/// Final damage formula: basedamage * ( 1 - Coefficient )
	/// </summary>
	public Dictionary<DamageType, float> Defences { get; set; }

	public CreatureState State { get; set; } = CreatureState.Alive;

	public void TakeDamage(float damage, DamageType damageType, Creature initiator) {
		float damage_after_defence = damage * (1.0f - Defences[damageType]);
		
		Hp -= damage_after_defence;
	}

	protected void Die() {
		State = CreatureState.Dead;
	}
}