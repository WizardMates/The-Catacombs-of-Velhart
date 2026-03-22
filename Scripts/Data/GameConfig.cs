using System.Collections.Generic;

namespace TheCatacombsOfVelhart.Scripts.Data.GameConfig;

public static class GameConfig {
	/// <summary>
	/// Dictionary of experience amount thresholds
	/// Level : Experience amount required to level up
	/// </summary>
	public static readonly Dictionary<int, int> ExperienceThresholds = new() {
		{1, 500},
		{2, 1000},
		{3, 2000},
		{4, 3000},
		{5, 4000},
	};
}