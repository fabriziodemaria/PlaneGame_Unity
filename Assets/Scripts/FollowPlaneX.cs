using UnityEngine;
using System.Collections;

public class FollowPlaneX : MonoBehaviour {
	
	GameObject plane;
	
	// Use this for initialization
	void Start () {
		plane = GameObject.FindGameObjectWithTag ("Player");
		if (plane == null) {
			Debug.LogError("No plane found!");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (plane.GetComponent<PlaneMovement>().isGameOver()) {
			Destroy(this.gameObject);
		}

		Vector3 pos = transform.position;
		pos.x = plane.transform.position.x;
		transform.position = pos;
	}
}
