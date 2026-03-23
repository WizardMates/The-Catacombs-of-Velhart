using System.Collections.Generic;
using TheCatacombsOfVelhart.Scripts.Data.GameEnums;
using TheCatacombsOfVelhart.Scripts.Data.GameConfig;

namespace TheCatacombsOfVelhart.Scripts.BaseObjects;

public abstract class PlayerBase : Creature {
	public float Hp { get; set; }
	public float MaxHp { get; set; }
	public List<IAbility> Abilities { get; set; }
	public Dictionary<DamageType, float> Defences { get; set; }
	
	public bool IsFigting { get; set; }

	private int _level;
	public int Level {
		get => _level;
		set {
			_level = value;
			LevelUp();
		}
	}

	/// <summary>
	/// Magic Circle number : available uses of the Circle in the current turn
	/// </summary>
	public Dictionary<int, int> MagicCircleFreeUses { get; set; }
	
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

	protected PlayerBase() {
		Level = 1;
		IsFigting = false;
	}

	protected abstract void LevelUp();

	protected void EnterFight(Fight fight) {
		IsFigting = true;
	}
	
	protected void ExitFight(Fight fight) {
		IsFigting = false;
	}
}
