using UnityEngine;
using System.Collections;

public class BGLooping : MonoBehaviour {

	public Sprite toForest;
	public Sprite forest;
	private LabelsManager scoreKeeper;
	private int currentBG = 0;

	void Start () {
		scoreKeeper = GameObject.FindObjectOfType<LabelsManager>();
	}

	void OnTriggerEnter2D(Collider2D collider) {

		if (collider.tag == "Cloud" || collider.tag == "Wrench" || collider.tag == "Tank") {
			Destroy(collider.gameObject);
			return;
		}

		float currentScore = scoreKeeper.Score;

		if (currentScore > 20 && currentScore < 70) {
			if (currentBG < 1) {
				Debug.Log("Setting to forest");
				currentBG++;
				collider.GetComponent<SpriteRenderer>().sprite = toForest;
			} else {
				Debug.Log("Setting forest");
				collider.GetComponent<SpriteRenderer>().sprite = forest;
			}
		}

		float heigthOfBG = ((BoxCollider2D)collider).size.y;
		Vector3 pos = collider.transform.position;
		pos.y += heigthOfBG * 2;
		collider.transform.position = pos;
	}
}
