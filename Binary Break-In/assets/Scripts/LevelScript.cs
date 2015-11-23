using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// LevelScript -- Script that manages level generation, destruction, completion, and animations.
/// </summary>
public class LevelScript : MonoBehaviour
{
	int numDigits;
	int currentNumber;

	bool finished;

	[SerializeField]
	float horizontalSwitchCenter = 0f;

	[SerializeField]
	float horizontalSwitchOffset = 5f;

	List<GameObject> binarySwitches;

	[SerializeField]
	GameObject binarySwitchPrefab = null;

	[SerializeField]
	BinaryLight successLight = null;

	[SerializeField]
	Animator doorAnimator = null;

	[SerializeField]
	Animator wheelAnimator = null;

	[SerializeField]
	TextMesh goalNumber = null;

	[SerializeField]
	AudioClip wheelCorrectAudio = null;
	AudioSource wheelCorrectSource = null;

	[SerializeField]
	AudioClip wheelFailedAudio = null;
	AudioSource wheelFailedSource = null;


	void Awake()
	{
		binarySwitches = new List<GameObject>();
	}

	void Start()
	{
		wheelCorrectSource = AudioHelper.CreateAudioSource(gameObject, wheelCorrectAudio);
		wheelFailedSource = AudioHelper.CreateAudioSource(gameObject, wheelFailedAudio);
	}

	/// <summary>
	/// Spawns a new set of switches. Also sets the target number.
	/// </summary>
	/// <param name="digits">Number of binary switches to spawn, as well as the number of digits
	/// for the goal number.</param>
	public void SpawnLevel(int digits)
	{
		finished = false;

		successLight.SetEnabled(false);

		numDigits = digits;

		// Loop from right to left to generate the least significant digit at index 0
		for (int i = 0; i < numDigits; ++i)
		{
			var switchObj = ObjectPooler.Instance.GetPooledObject(binarySwitchPrefab.name);

			// Let the switch set itself up
			switchObj.GetComponent<BinarySwitch>().Setup();
			switchObj.SetActive(true);

			// Parent switch to level
			switchObj.transform.SetParent(transform);

			// Position switch
			var distanceFromCenter = (0.5f * (numDigits - 1) - i) * horizontalSwitchOffset;
			switchObj.transform.localPosition = new Vector3(distanceFromCenter + horizontalSwitchCenter, 0, 0);
			switchObj.transform.localRotation = Quaternion.identity;

			// Add to list. Switches are added from least significant digit to most significant,
			// meaning digit 0 is in index 0.
			binarySwitches.Add(switchObj);
		}

		// Generate number with the correct number of digits
		// Most significant digit will always be 1 because of this
		currentNumber = Random.Range((int)(Mathf.Pow(2, digits - 1)), (int)(Mathf.Pow(2, digits)));

		goalNumber.text = currentNumber.ToString();
	}

	/// <summary>
	/// Returns the binary switches in the level to the object pool.
	/// </summary>
	public void DestroyLevel()
	{
		foreach (GameObject o in binarySwitches)
		{
			ObjectPooler.Instance.ReturnPooledObject(o);
		}

		binarySwitches.Clear();

		doorAnimator.SetBool("DoorOpen", false);
	}

	public void OnFinishLevelClicked()
	{
		// Level's not done unless we're in-game and the correct code is input
		if (GameplayManager.Instance.IsInGame() && !finished && CheckLevelCompletion())
		{
			finished = true;

			// Wait 2 seconds to finish the level to give animations time to complete
			Invoke("FinishLevel", 2f);
		}
	}

	/// <summary>
	/// Triggers level completion animations and gameplay state.
	/// </summary>
	void FinishLevel()
	{
		// Animations
		doorAnimator.SetBool("DoorOpen", true);
		wheelAnimator.ResetTrigger("FailedCode");
		wheelAnimator.ResetTrigger("AcceptedCode");

		// Tell GameplayManager the level is finished
		GameplayManager.Instance.OnLevelComplete();
	}

	/// <summary>
	/// Checks the binary switches to see if the input matches the target number.
	/// Plays wheel animations based on correctness.
	/// </summary>
	/// <returns>True if the correct number was input, false otherwise.</returns>
	bool CheckLevelCompletion()
	{
		int inputNum = 0;

		// *** Add your source code here ***

		bool complete = inputNum == currentNumber;

		successLight.SetEnabled(true);
		successLight.SetOn(complete);

		CancelInvoke("TurnOffSuccessLight");
		Invoke("TurnOffSuccessLight", 1f);

		// Animations
		wheelAnimator.ResetTrigger("AcceptedCode");
		wheelAnimator.ResetTrigger("FailedCode");
		if (complete)
		{
			wheelAnimator.SetTrigger("AcceptedCode");
			wheelCorrectSource.Play();
		}
		else
		{
			wheelAnimator.SetTrigger("FailedCode");
			if (!wheelFailedSource.isPlaying)
			{
				wheelFailedSource.Play();
			}
		}

		return complete;
	}

	void TurnOffSuccessLight()
	{
		successLight.SetEnabled(false);
	}
}
