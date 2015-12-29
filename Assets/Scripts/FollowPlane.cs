using UnityEngine;
using System.Collections;

public class FollowPlane : MonoBehaviour {

	GameObject plane;
	public float shadowOffsetX;
	public float shadowOffsetY;
	private float initialScale;

	// Use this for initialization
	void Start () {
		plane = GameObject.FindGameObjectWithTag ("Player");
		if (plane == null) {
			Debug.LogError("No plane found!");
			return;
		}
		initialScale = plane.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (plane.GetComponent<PlaneMovement>().isGameOver()) {
			Destroy(this.gameObject);
		}

		Vector3 pos = plane.transform.position;
		Quaternion rot = plane.transform.rotation;
		pos.z += 1;
		pos.x += shadowOffsetX * (plane.transform.localScale.x / initialScale);
		pos.y += shadowOffsetY * (plane.transform.localScale.x / initialScale);
		transform.position = pos;
		transform.rotation = rot;
		transform.localScale = plane.transform.localScale;
	}
}
