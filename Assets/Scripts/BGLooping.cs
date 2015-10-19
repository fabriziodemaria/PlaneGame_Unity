using UnityEngine;
using System.Collections;

public class BGLooping : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider) {

		float heigthOfBG = ((BoxCollider2D)collider).size.y;
		Vector3 pos = collider.transform.position;
		pos.y += heigthOfBG * 4;
		collider.transform.position = pos;
	}
}
