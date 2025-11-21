using UnityEngine;
using System.Collections;

/// <summary>
/// Simple movement controller for parachutes with constant downward speed.
/// </summary>
public class ParachuteMovement : MonoBehaviour
{
	[SerializeField] private Vector3 parachuteSpeed = new Vector3(0, -2f, 0);

	void FixedUpdate()
	{
		// Move parachute with constant velocity
		transform.position += parachuteSpeed * Time.deltaTime;
	}
}

