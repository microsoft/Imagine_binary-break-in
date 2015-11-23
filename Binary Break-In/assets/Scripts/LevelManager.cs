using UnityEngine;
using System.Collections;

/// <summary>
/// The level manager is responsible for spawning blocks for the different levels. It can
/// also determine the player rank based on the current throw count. The level manager is a
/// singleton and can be accessed in any script using the LevelManager.Instance syntax.
/// </summary>
public class LevelManager : MonoBehaviour
{
	// The static singleton instance of the level manager.
	public static LevelManager Instance;

	[System.Serializable]
	public class Level
	{
		public int numDigits;
	};

	public Level[] levels;	// Array of level data.

	int currentLevel;

	[SerializeField]
	LevelScript levelObject = null;

	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;
	}

	/// <summary>
	/// Spawns the prefab for the blocks of a given level.
	/// </summary>
	/// <returns>The number of good blocks for the given level.</returns>
	/// <param name="level">The level to be spawned.</param>
	public void StartLevel(int level)
	{
		// Spawn the new level. We are subtracting 1 since levels start at 1 but
		// array index starts at 0.
		currentLevel = Mathf.Clamp(level - 1, 0, levels.Length - 1);
		levelObject.DestroyLevel();
		levelObject.SpawnLevel(levels[currentLevel].numDigits);
	}

	void SpawnNewLevel()
	{
		levelObject.SpawnLevel(levels[currentLevel].numDigits);
	}
}