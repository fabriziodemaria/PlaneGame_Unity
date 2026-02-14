using UnityEngine;
using System.Collections;

/// <summary>
/// Handles infinite background scrolling by moving background tiles and changing
/// sprites based on player score/altitude. Ensures proper Z-depth layering.
/// </summary>
public class BGLooping : MonoBehaviour
{
	// Background transition score thresholds
	private const float FOREST_SCORE = 50f;
	private const float DESERT_SCORE = 100f;
	private const float SPACE_SCORE = 150f;
	private const float SPACE_END_SCORE = 200f;
	private const int SCROLL_TIME_OFFSET = 30;

	// Background sprites (set in Unity Inspector)
	[Header("Background Sprites")]
	[SerializeField] private Sprite toForest;
	[SerializeField] private Sprite forest;
	[SerializeField] private Sprite toDesert;
	[SerializeField] private Sprite desert;
	[SerializeField] private Sprite toSpace;
	[SerializeField] private Sprite space;

	private LabelsManager scoreKeeper;
	private int bgIndex = 0;

	void Start()
	{
		// Cache score keeper reference
		scoreKeeper = FindObjectOfType<LabelsManager>();
		if (scoreKeeper == null)
		{
			Debug.LogError("BGLooping: LabelsManager not found!");
			enabled = false;
			return;
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		// Destroy objects that reach the bottom of screen
		if (collider.CompareTag("Cloud") || collider.CompareTag("Wrench") || collider.CompareTag("Tank"))
		{
			Destroy(collider.gameObject);
			return;
		}

		// Update background sprite based on score
		UpdateBackgroundSprite(collider);

		// Move background tile up for infinite scrolling
		MoveBGUp(collider);
	}

	private void UpdateBackgroundSprite(Collider2D collider)
	{
		float currentScore = scoreKeeper.Score;
		SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();

		if (renderer == null)
			return;

		// Transition to Forest (score 20-70)
		if (currentScore > FOREST_SCORE - SCROLL_TIME_OFFSET &&
		    currentScore < DESERT_SCORE - SCROLL_TIME_OFFSET)
		{
			if (bgIndex < 1)
			{
				bgIndex++;
				renderer.sprite = toForest;
			}
			else
			{
				renderer.sprite = forest;
			}
		}
		// Transition to Desert (score 70-120)
		else if (currentScore >= DESERT_SCORE - SCROLL_TIME_OFFSET &&
		         currentScore < SPACE_SCORE - SCROLL_TIME_OFFSET)
		{
			if (bgIndex < 2)
			{
				bgIndex++;
				renderer.sprite = toDesert;
			}
			else
			{
				renderer.sprite = desert;
			}
		}
		// Transition to Space (score 120-170)
		else if (currentScore >= SPACE_SCORE - SCROLL_TIME_OFFSET &&
		         currentScore < SPACE_END_SCORE - SCROLL_TIME_OFFSET)
		{
			if (bgIndex < 3)
			{
				bgIndex++;
				renderer.sprite = toSpace;
			}
			else
			{
				renderer.sprite = space;
			}
		}
	}

	private void MoveBGUp(Collider2D collider)
	{
		BoxCollider2D boxCollider = collider as BoxCollider2D;
		if (boxCollider == null)
			return;

		float heightOfBG = boxCollider.size.y;
		Vector3 pos = collider.transform.position;
		pos.y += heightOfBG * 2f;
		pos.z = 1f; // keep background behind clouds and gameplay objects
		collider.transform.position = pos;
	}
}

