using UnityEngine;
using System.Collections;

public class CloudsVelocity : MonoBehaviour {

	public Vector3 cloudSpeed;
	
	void Start () {
		cloudSpeed.y -= Random.Range(0.0f,2.0f);
	}
	// Update is called once per frame
	void Update () {
		transform.position += cloudSpeed * Time.deltaTime;
	}
}
