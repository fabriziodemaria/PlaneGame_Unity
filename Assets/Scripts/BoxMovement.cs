using UnityEngine;
using System.Collections;

public class BoxMovement : MonoBehaviour {
	
	public Vector3 boxSpeed;
	
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		transform.position += boxSpeed * Time.deltaTime;
	}
}
