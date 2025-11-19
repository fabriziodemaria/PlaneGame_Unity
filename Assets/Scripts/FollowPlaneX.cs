using UnityEngine;
using System.Collections;

/// <summary>
/// Makes this object follow the plane's horizontal (X) position only.
/// </summary>
public class FollowPlaneX : MonoBehaviour
{
	private Transform planeTransform;
	private PlaneMovement planeMovement;

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
	}

	void LateUpdate()
	{
		// Destroy when game is over
		if (planeMovement.isGameOver())
		{
			Destroy(gameObject);
			return;
		}

		// Follow plane's X position only
		Vector3 pos = transform.position;
		pos.x = planeTransform.position.x;
		transform.position = pos;
	}
}

