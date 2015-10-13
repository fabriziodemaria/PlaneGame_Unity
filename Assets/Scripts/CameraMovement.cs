using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	Transform planetransform;
	private float offsetX;

	// Use this for initialization
	void Start () {
		GameObject playerplane = GameObject.FindGameObjectWithTag ("Player");

		if (playerplane == null) {
			Debug.LogError("Ups, no plane found on the screen");
			return;
		}
		planetransform = playerplane.transform;
		offsetX = transform.position.y - planetransform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.y = planetransform.position.y + offsetX;
		transform.position = pos;
	}
}
