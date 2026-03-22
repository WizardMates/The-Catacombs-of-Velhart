using System;
using System.Collections.Generic;

namespace TheCatacombsOfVelhart.Scripts.Data.GameConfig;

public static class GameConfig {
	
	private const int BaseExperience = 500;
	private const float ExperienceMultiplier = 1.5f;
	/// <summary>
	/// Function for getting current level experience threshold
	///
	/// Level: experience threshold | using multiplier 1.5
	/// 1: 500
	/// 2: 750
	/// 3: 1125
	/// 4: 1687
	/// 5: 2531
	/// </summary>
	/// <param name="level">Level of a Creature</param>
	/// <returns>Experience needed to level up from current level</returns>
	public static int GetExperienceThreshold(int level)
	{
		return (int)(BaseExperience * Math.Pow(ExperienceMultiplier, level-1));
	}
	
	
	/*
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
	};*/
}