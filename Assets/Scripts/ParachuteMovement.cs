using UnityEngine;
using System.Collections;

public class ParachuteMovement : MonoBehaviour {

	public Vector3 parachuteSpeed;

	// Update is called once per frame
	void Update () {
		transform.position += parachuteSpeed * Time.deltaTime;
	}
}
