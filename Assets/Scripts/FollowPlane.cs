using UnityEngine;
using System.Collections;

public class FollowPlane : MonoBehaviour {

	GameObject playerPlane;
	public float shadowOffsetX;
	public float shadowOffsetY;
	private float initialScale;

	// Use this for initialization
	void Start () {
		playerPlane = GameObject.FindGameObjectWithTag ("Player");
		if (playerPlane == null) {
			Debug.LogError("No plane found!");
			return;
		}
		initialScale = playerPlane.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerPlane.transform.localScale.sqrMagnitude <= 1.1f) {
			Destroy(this.gameObject);
		}

		Vector3 pos = playerPlane.transform.position;
		Quaternion rot = playerPlane.transform.rotation;
		pos.z += 1;
		pos.x += shadowOffsetX * (playerPlane.transform.localScale.x / initialScale);
		pos.y += shadowOffsetY * (playerPlane.transform.localScale.x / initialScale);
		transform.position = pos;
		transform.rotation = rot;
		transform.localScale = playerPlane.transform.localScale;
	}
}
