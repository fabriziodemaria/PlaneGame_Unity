using UnityEngine;
using System.Collections;

public class CloudsMovement : MonoBehaviour {

	public Vector3 cloudSpeed;
	
	void Start () {
		cloudSpeed.y -= Random.Range(0.0f,1.8f);
	}
	// Update is called once per frame
	void Update () {
		transform.position += cloudSpeed * Time.deltaTime;
	}
}
