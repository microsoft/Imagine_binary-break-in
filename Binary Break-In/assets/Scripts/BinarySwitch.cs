using UnityEngine;
using System.Collections;

/// <summary>
/// BinarySwitch - Script that handles logic for a single binary switch in-game.
/// </summary>
public class BinarySwitch : MonoBehaviour
{
	[SerializeField]
	BinaryLight activeLight = null;

	[SerializeField]
	TextMesh number = null;

	[SerializeField]
	AudioClip switchAudio = null;
	AudioSource switchSource = null;

	public bool IsOn { get; private set; }

	void Start()
	{
		switchSource = AudioHelper.CreateAudioSource(gameObject, switchAudio);
	}

	/// <summary>
	/// Initialize the switch to a default state.
	/// </summary>
	/// <param name="digit">The </param>
	public void Setup()
	{
		// Defaults - number is 0, light is red
		IsOn = false;
		activeLight.SetEnabled(true);
		activeLight.SetOn(false);
		number.text = 0.ToString();
	}

	/// <summary>
	/// Called when the user clicks on the switch.
	/// </summary>
	public void OnSwitchActivated()
	{
		// Need to be in-game to activate switch
		if (GameplayManager.Instance.IsInGame())
		{
			// Toggle off-on and 0-1
			IsOn = !IsOn;
			activeLight.SetOn(IsOn);
			number.text = IsOn ? 1.ToString() : 0.ToString();
			switchSource.Play();
		}
	}
}
