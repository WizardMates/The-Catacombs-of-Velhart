using System.Collections.Generic;
using TheCatacombsOfVelhart.Scripts.Data.GameEnums;
using TheCatacombsOfVelhart.Scripts.Data.GameConfig;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

public abstract class PlayerBase : ICreature {
	public float Hp { get; set; }
	public float MaxHp { get; set; }
	public List<IAbility> Abilities { get; set; }
	public Dictionary<DamageType, float> Defences { get; set; }

	private int _level = 1; // Player always starts with level 1
	public int Level {
		get => _level;
		set {
			_level = value;
			LevelUp();
		}
	}

	private int _experience;
	public int Experience {
		get => _experience;
		set {
			// if income experience amount is higher than current level experience cap, then leveling up and spending experience to level up
			_experience = value;
			while (_experience >= GameConfig.GetExperienceThreshold(Level)) {
				_experience -= GameConfig.GetExperienceThreshold(Level);
				Level++;
			}
		}
	}

	protected abstract void LevelUp();
}
