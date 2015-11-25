using UnityEngine;
using System.Collections;

public class FollowPlaneX : MonoBehaviour {
	
	GameObject playerPlane;
	
	// Use this for initialization
	void Start () {
		playerPlane = GameObject.FindGameObjectWithTag ("Player");
		if (playerPlane == null) {
			Debug.LogError("No plane found!");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (playerPlane.transform.localScale.sqrMagnitude <= 1.1f) {
			Destroy(this.gameObject);
		}
		Vector3 pos = transform.position;
		pos.x = playerPlane.transform.position.x;
		transform.position = pos;
	}
}
