using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement controller for clouds with randomized downward speed.
/// </summary>
public class CloudsMovement : MonoBehaviour
{
	[SerializeField] private Vector3 cloudSpeed = new Vector3(0, -2f, 0);

	void Start()
	{
		// Add random variation to downward speed
		cloudSpeed.y -= Random.Range(0.0f, 1.8f);
	}

	void FixedUpdate()
	{
		// Move cloud downward (or in specified direction)
		transform.position += cloudSpeed * Time.deltaTime;
	}
}

