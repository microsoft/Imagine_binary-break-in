using UnityEngine;
using System.Collections;

/// <summary>
/// BinaryLight - Script that handles toggling a light between two colors.
/// </summary>
public class BinaryLight : MonoBehaviour
{
	[SerializeField]
	Color offColor = Color.black;
	[SerializeField]
	Color onColor = Color.black;

	Light myLight;

	// Use this for initialization
	void Awake()
	{
		myLight = GetComponent<Light>();
	}

	void Start()
	{
		myLight.color = offColor;
	}

	public void SetOn(bool on)
	{
		myLight.color = on ? onColor : offColor;
	}

	public void SetEnabled(bool enabled)
	{
		myLight.enabled = enabled;
	}
}
