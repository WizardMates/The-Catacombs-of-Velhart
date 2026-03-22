using System.Collections.Generic;
using TheCatacombsOfVelhart.Scripts.GameEnums.Data;
using TheCatacombsOfVelhart.Scripts.Data.GameConfig;

namespace TheCatacombsOfVelhart.Scripts.GameEnums.BaseObjects;

public abstract class PlayerBase : ICreature {
	public float Hp { get; set; }
	public float MaxHp { get; set; }
	public List<IAbility> Abilities { get; set; }
	public Dictionary<DamageType, float> Defences { get; set; }

	private int _level;
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
			int _new_experience = value;
			int _experience_needed_to_level_up = GameConfig.ExperienceThresholds[Level];
			if (_new_experience < _experience_needed_to_level_up) {
				_experience = value;
			}
			else {
				do {
					Level++;

					_new_experience -= _experience_needed_to_level_up;
					_experience_needed_to_level_up = GameConfig.ExperienceThresholds[Level];

				} while (_new_experience >= _experience_needed_to_level_up);

				_experience = _new_experience;
			}
		}
	}

	protected abstract void LevelUp();
}
