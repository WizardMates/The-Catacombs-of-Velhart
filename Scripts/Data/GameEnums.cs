namespace TheCatacombsOfVelhart.Scripts.Data.GameEnums;

public enum AbilityType {
	Damage,
	Heal,
	Defence,
	Utility
}

public enum DamageType {
	Magical,
	Physical,
	Fire,
	Cold
}

public enum PlayerClass {
	Wizard,
	Knight,
	Cleric
}

public enum FightState
{
	PlayerTurn,
	EnemyTurn,
	Victory,
	Defeat
}

public enum CreatureState
{
	Alive,
	Dead
}