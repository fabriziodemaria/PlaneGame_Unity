using UnityEngine;
using System.Collections;

public class BirdsMovement : MonoBehaviour {

	public Vector3 velocity;
	
	// Update is called once per frame
	void Update () {
		transform.position += velocity * Time.deltaTime;
	}
}
