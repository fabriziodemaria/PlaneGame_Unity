using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the UI arrows that indicate lateral force direction and strength.
/// Follows the plane's X/Y position but maintains proper UI Z-depth layering.
/// </summary>
public class ArrowsScript : MonoBehaviour
{
	private const int ARROW_COUNT = 3;

	private SpriteRenderer[] rightArrowRenderers;
	private SpriteRenderer[] leftArrowRenderers;
	private Transform planeTransform;
	private PlaneMovement planeMovement;

	private readonly Color dimColor = new Color(1f, 1f, 1f, 0.2f);
	private readonly Color filledColor = new Color(1f, 1f, 1f, 0.8f);

	void Start()
	{
		// Cache plane references
		GameObject plane = GameObject.FindGameObjectWithTag("Player");
		if (plane == null)
		{
			Debug.LogError("ArrowsScript: No plane found!");
			enabled = false;
			return;
		}

		planeTransform = plane.transform;
		planeMovement = plane.GetComponent<PlaneMovement>();
		if (planeMovement == null)
		{
			Debug.LogError("ArrowsScript: PlaneMovement component not found!");
			enabled = false;
			return;
		}

		// Initialize and cache arrow sprite renderers
		rightArrowRenderers = new SpriteRenderer[ARROW_COUNT];
		leftArrowRenderers = new SpriteRenderer[ARROW_COUNT];

		// Cache right arrow renderers
		for (int i = 0; i < ARROW_COUNT; i++)
		{
			GameObject arrow = GameObject.Find($"ArrowR{i + 1}");
			if (arrow != null)
			{
				rightArrowRenderers[i] = arrow.GetComponent<SpriteRenderer>();
			}
			else
			{
				Debug.LogWarning($"ArrowsScript: ArrowR{i + 1} not found!");
			}
		}

		// Cache left arrow renderers
		for (int i = 0; i < ARROW_COUNT; i++)
		{
			GameObject arrow = GameObject.Find($"ArrowL{i + 1}");
			if (arrow != null)
			{
				leftArrowRenderers[i] = arrow.GetComponent<SpriteRenderer>();
			}
			else
			{
				Debug.LogWarning($"ArrowsScript: ArrowL{i + 1} not found!");
			}
		}

	}



	void Update()
	{
		// Follow plane's position
		Vector3 pos = transform.position;
		pos.x = planeTransform.position.x;
		pos.y = planeTransform.position.y;
		pos.z = planeTransform.position.z;
		transform.position = pos;
	}

	public void updateArrows(Vector3 lateralForce)
	{
		// Hide arrows when game is over
		if (planeMovement.isGameOver())
		{
			SetArrowsEnabled(false);
			return;
		}

		// Get current lateral force from plane
		lateralForce = planeMovement.lateralForce;

		if (lateralForce.x == 0)
		{
			// No lateral force - dim all arrows
			SetArrowColors(rightArrowRenderers, 0, dimColor);
			SetArrowColors(leftArrowRenderers, 0, dimColor);
		}
		else if (lateralForce.x > 0)
		{
			// Moving right - fill right arrows proportionally
			int fillCount = Mathf.Min((int)lateralForce.x, ARROW_COUNT);
			SetArrowColors(rightArrowRenderers, fillCount, filledColor, dimColor);
			SetArrowColors(leftArrowRenderers, 0, dimColor);
		}
		else if (lateralForce.x < 0)
		{
			// Moving left - fill left arrows proportionally
			int fillCount = Mathf.Min((int)(-lateralForce.x), ARROW_COUNT);
			SetArrowColors(leftArrowRenderers, fillCount, filledColor, dimColor);
			SetArrowColors(rightArrowRenderers, 0, dimColor);
		}
	}

	private void SetArrowsEnabled(bool enabled)
	{
		for (int i = 0; i < ARROW_COUNT; i++)
		{
			if (rightArrowRenderers[i] != null)
				rightArrowRenderers[i].enabled = enabled;
			if (leftArrowRenderers[i] != null)
				leftArrowRenderers[i].enabled = enabled;
		}
	}

	private void SetArrowColors(SpriteRenderer[] arrows, int fillCount, Color color)
	{
		for (int i = 0; i < ARROW_COUNT; i++)
		{
			if (arrows[i] != null)
				arrows[i].color = color;
		}
	}

	private void SetArrowColors(SpriteRenderer[] arrows, int fillCount, Color filledColor, Color dimColor)
	{
		for (int i = 0; i < ARROW_COUNT; i++)
		{
			if (arrows[i] != null)
				arrows[i].color = (i < fillCount) ? filledColor : dimColor;
		}
	}
}

