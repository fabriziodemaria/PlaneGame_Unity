using UnityEngine;
using System.Collections;

/// <summary>
/// Camera controller that smoothly follows the player's vertical (Y) position.
/// </summary>
public class CameraMovement : MonoBehaviour
{
	private Transform planeTransform;
	private float offsetY;

	void Start()
	{
		// Cache player plane reference
		GameObject playerPlane = GameObject.FindGameObjectWithTag("Player");

		if (playerPlane == null)
		{
			Debug.LogError("CameraMovement: No plane found on the screen!");
			enabled = false;
			return;
		}

		planeTransform = playerPlane.transform;
		// Calculate Y offset between camera and plane
		offsetY = transform.position.y - planeTransform.position.y;
	}

	void LateUpdate()
	{
		if (planeTransform == null)
			return;

		// Follow plane's Y position while maintaining offset
		Vector3 pos = transform.position;
		pos.y = planeTransform.position.y + offsetY;
		transform.position = pos;
	}
}

