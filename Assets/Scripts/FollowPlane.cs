using UnityEngine;
using System.Collections;

/// <summary>
/// Makes this object follow the plane with an offset, typically used for shadow effects.
/// Uses proper sorting order instead of Z-position manipulation.
/// </summary>
public class FollowPlane : MonoBehaviour
{
	[SerializeField] private float shadowOffsetX;
	[SerializeField] private float shadowOffsetY;

	private Transform planeTransform;
	private PlaneMovement planeMovement;
	private SpriteRenderer spriteRenderer;
	private float initialScale;

	void Start()
	{
		// Cache plane references
		GameObject plane = GameObject.FindGameObjectWithTag("Player");
		if (plane == null)
		{
			Debug.LogError("FollowPlane: No plane found!");
			enabled = false;
			return;
		}

		planeTransform = plane.transform;
		planeMovement = plane.GetComponent<PlaneMovement>();
		if (planeMovement == null)
		{
			Debug.LogError("FollowPlane: PlaneMovement component not found on plane!");
			enabled = false;
			return;
		}

		initialScale = planeTransform.localScale.x;

		// Set up proper layering using sorting order instead of Z-position
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null)
		{
			spriteRenderer.sortingLayerName = LayerConstants.SORT_PLAYER;
			spriteRenderer.sortingOrder = LayerConstants.ORDER_PLAYER_SHADOW;
		}

		// Set Z position to match player layer
		Vector3 pos = transform.position;
		pos.z = LayerConstants.Z_PLAYER;
		transform.position = pos;
	}

	void LateUpdate()
	{
		// Destroy shadow when game is over
		if (planeMovement.isGameOver())
		{
			Destroy(gameObject);
			return;
		}

		// Calculate scale factor for offset adjustment
		float scaleFactor = planeTransform.localScale.x / initialScale;

		// Follow plane with offset (no Z manipulation)
		Vector3 pos = planeTransform.position;
		pos.x += shadowOffsetX * scaleFactor;
		pos.y += shadowOffsetY * scaleFactor;
		pos.z = LayerConstants.Z_PLAYER; // Keep consistent Z depth

		transform.position = pos;
		transform.rotation = planeTransform.rotation;
		transform.localScale = planeTransform.localScale;
	}
}
