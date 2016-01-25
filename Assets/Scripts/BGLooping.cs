using UnityEngine;
using System.Collections;

public class BGLooping : MonoBehaviour {

	/* Set background images here in Unity */
	public Sprite toForest;
	public Sprite forest;
	public Sprite toDesert;
	public Sprite Desert;

	private LabelsManager scoreKeeper;
	private int scrollTimeOffset = 30;
	private int BGIndex = 0;

	void Start () {
		scoreKeeper = GameObject.FindObjectOfType<LabelsManager>();
	}

	void OnTriggerEnter2D(Collider2D collider) {

		if (collider.tag == "Cloud" || collider.tag == "Wrench" || collider.tag == "Tank") {
			Destroy(collider.gameObject);
			return;
		}

		float currentScore = scoreKeeper.Score;

		/* Handle background changes */
		if (currentScore > 50 - scrollTimeOffset && currentScore < 100 - scrollTimeOffset) {
			if (BGIndex < 1) {
				BGIndex++;
				collider.GetComponent<SpriteRenderer>().sprite = toForest;
			} else {
				collider.GetComponent<SpriteRenderer>().sprite = forest;
			}
		}
		/* End background changes */
		if (currentScore >= 100 - scrollTimeOffset && currentScore < 150 - scrollTimeOffset) {
			if (BGIndex < 2) {
				BGIndex++;
				collider.GetComponent<SpriteRenderer>().sprite = toDesert;
			} else {
				collider.GetComponent<SpriteRenderer>().sprite = Desert;
			}
		}
		moveBGUp(collider);
	}

	void moveBGUp (Collider2D collider) {
		float heigthOfBG = ((BoxCollider2D)collider).size.y;
		Vector3 pos = collider.transform.position;
		pos.y += heigthOfBG * 2;
		collider.transform.position = pos;
	}
}
