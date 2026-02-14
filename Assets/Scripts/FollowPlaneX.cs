using UnityEngine;
using System.Collections;

/// <summary>
/// Makes this object follow the plane's position while maintaining its initial Y offset.
/// Used by spawners to stay above the camera as the plane flies upward.
/// </summary>
public class FollowPlaneX : MonoBehaviour
{
	private Transform planeTransform;
	private PlaneMovement planeMovement;
	private float offsetY;

	void Start()
	{
		// Cache plane references
		GameObject plane = GameObject.FindGameObjectWithTag("Player");
		if (plane == null)
		{
			Debug.LogError("FollowPlaneX: No plane found!");
			enabled = false;
			return;
		}

		planeTransform = plane.transform;
		planeMovement = plane.GetComponent<PlaneMovement>();
		if (planeMovement == null)
		{
			Debug.LogError("FollowPlaneX: PlaneMovement component not found!");
			enabled = false;
			return;
		}

		// Store the initial Y offset so the spawner stays above the viewport
		offsetY = transform.position.y - planeTransform.position.y;
	}

	void LateUpdate()
	{
		// Destroy when game is over
		if (planeMovement.isGameOver())
		{
			Destroy(gameObject);
			return;
		}

		// Follow plane X directly, and track Y with the initial offset
		Vector3 pos = transform.position;
		pos.x = planeTransform.position.x;
		pos.y = planeTransform.position.y + offsetY;
		transform.position = pos;
	}
}

