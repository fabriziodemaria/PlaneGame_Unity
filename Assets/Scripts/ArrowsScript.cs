using UnityEngine;
using System.Collections;

public class ArrowsScript : MonoBehaviour {

	private GameObject[] rightArrows;
	private GameObject[] leftArrows;
	private GameObject plane;

	void Start () {
		plane = GameObject.FindGameObjectWithTag("Player");
		rightArrows = new GameObject[3];
		leftArrows = new GameObject[3];
		rightArrows[0] = GameObject.Find("ArrowR1");
		rightArrows[1] = GameObject.Find("ArrowR2");
		rightArrows[2] = GameObject.Find("ArrowR3");
		leftArrows[0] = GameObject.Find("ArrowL1");
		leftArrows[1] = GameObject.Find("ArrowL2");
		leftArrows[2] = GameObject.Find("ArrowL3");
	}
	
	void Update () {
		Vector3 pos = transform.position;
		pos.x = plane.transform.position.x;
		pos.y = plane.transform.position.y;
		pos.z = plane.transform.position.z;
		transform.position = pos;
	}

	public void updateArrows(Vector3 lateralForce) {
		int i;

		/* Remove arrows upon Game Over */
		if (plane.GetComponent<PlaneMovement>().isGameOver()) {
			for (i = 0; i < 3; i++) {
				rightArrows[i].GetComponent<SpriteRenderer>().enabled = false;
				leftArrows[i].GetComponent<SpriteRenderer>().enabled = false;
			}
			return;
		}

		lateralForce = plane.GetComponent<PlaneMovement>().lateralForce;

		Color dim = Color.white;
		Color filled = Color.white;
		dim.a = 0.2f;
		filled.a = 0.8f;

		if (lateralForce.x == 0) {
			for (i = 0; i < 3; i++) {
				rightArrows[i].GetComponent<SpriteRenderer>().color = dim;
				leftArrows[i].GetComponent<SpriteRenderer>().color = dim;
			}
		} else if (lateralForce.x > 0) {
			for (i = 0; i < lateralForce.x; i++) {
				rightArrows[i].GetComponent<SpriteRenderer>().color = filled;
			}
			for (; i < 3; i++) {
				rightArrows[i].GetComponent<SpriteRenderer>().color = dim;
			}
			for (i = 0; i < 3; i++) {
				leftArrows[i].GetComponent<SpriteRenderer>().color = dim;
			}
		} else if (lateralForce.x < 0) {
			for (i = 0; i < -lateralForce.x; i++) {
				leftArrows[i].GetComponent<SpriteRenderer>().color = filled;
			}
			for (; i < 3; i++) {
				leftArrows[i].GetComponent<SpriteRenderer>().color = dim;
			}
			for (i = 0; i < 3; i++) {
				rightArrows[i].GetComponent<SpriteRenderer>().color = dim;
			}
		}
	}
}
