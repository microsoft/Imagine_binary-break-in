using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// The gameplay manager is responsible for controlling the overall flow of the game. The
/// game is divided into three main states: Tutorial, InGame, and GameOver. The user interface
/// and input controls are different depending on the current game state. The gameplay
/// manager tracks the player progress and switches between the game states based on
/// the results as well as the user input. The gameplay manager is a singleton and can be
/// accessed in any script using the GameplayManager.Instance syntax.
/// </summary>
public class GameplayManager : MonoBehaviour
{
	// The static singleton instance of the gameplay manager.
	public static GameplayManager Instance { get; private set; }

	// Enumeration for the different game states. The default starting
	// state is the tutorial.
	enum GameState
	{
		Tutorial,	// Show player the game instructions.
		InGame,		// Player can start shooting with the left mouse button.
		LevelComplete,
		GameOver,	// Game ended, player input is blocked.
	};
	GameState state = GameState.Tutorial;
	
	int currentLevel = 1;		// The current level the player is playing.

	void Awake()
	{
		// Register this script as the singleton instance.
		Instance = this;
	}

	void Start()
	{
		// Refresh the HUD and show the tutorial screen.
		UIManager.Instance.UpdateHUD(currentLevel);
		UIManager.Instance.ShowHUD(false);
		UIManager.Instance.ShowScreen("Tutorial");
	}

	/// <summary>
	/// Reloads the current scene.
	/// </summary>
	void ReloadScene()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	/// <summary>
	/// Call this function to start the gameplay.
	/// </summary>
	public void OnStartGame()
	{
		state = GameState.InGame;
		LevelManager.Instance.StartLevel(currentLevel);
		UIManager.Instance.UpdateHUD(currentLevel);
		UIManager.Instance.ShowHUD(true);
		UIManager.Instance.ShowScreen("");
	}

	/// <summary>
	/// Called when the level is finished.
	/// </summary>
	public void OnLevelComplete()
	{
		state = GameState.LevelComplete;
		if (currentLevel == LevelManager.Instance.levels.Length)
		{
			UIManager.Instance.ShowScreen("Game Complete");
		}
		else
		{
			UIManager.Instance.ShowScreen("Level Complete");
		}
	}

	/// <summary>
	/// Call this function to reload the current level. The player progress will be reset.
	/// </summary>
	public void OnRetryLevel()
	{
		// Start gameplay and refresh the HUD.
		
		Invoke("OnStartGame", 0.5f);
	}

	/// <summary>
	/// Call this function to advance to the next level.
	/// </summary>
	public void OnNextLevel()
	{
		// Update the current level number but make sure we don't go over the
		// total number of levels.
		currentLevel = Mathf.Clamp(currentLevel + 1, 1, LevelManager.Instance.levels.Length);

		// Call retry level since the logic is essentially the same.
		OnRetryLevel();
	}

	public bool IsInGame()
	{
		return state == GameState.InGame;
	}

	/// <summary>
	/// Call this function to restart the current level.
	/// </summary>
	public void OnRestart()
	{
		// Reload the current scene.
		Invoke("ReloadScene", 0.5f);
	}

	public void OnLanguageChanged()
	{
		UIManager.Instance.OnLanguageChanged();
		UIManager.Instance.UpdateHUD(currentLevel);
	}
}