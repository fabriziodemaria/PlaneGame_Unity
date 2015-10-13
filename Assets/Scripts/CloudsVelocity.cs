using UnityEngine;
using System.Collections;

public class CloudsVelocity : MonoBehaviour {

	public Vector3 cloudSpeed;
	
	// Update is called once per frame
	void Update () {
		transform.position += cloudSpeed * Time.deltaTime;
	}
}
