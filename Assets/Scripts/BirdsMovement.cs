using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement controller for birds with constant velocity.
/// </summary>
public class BirdsMovement : MonoBehaviour
{
	[SerializeField] private Vector3 velocity = new Vector3(0, -2f, 0);

	void FixedUpdate()
	{
		// Move bird with constant velocity
		transform.position += velocity * Time.deltaTime;
	}
}

