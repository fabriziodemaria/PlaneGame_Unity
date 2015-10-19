using UnityEngine;
using System.Collections;

public class GenerateBox : MonoBehaviour {

	public GameObject WrenchPrefab;
	public float sDealy = 3f;
	float nextS = 1f;
	
	// Update is called once per frame
	void FixedUpdate () {
		nextS -= Time.deltaTime;
		
		if (nextS <= 0) {
			nextS = sDealy;
			Vector3 pos = transform.position;
			pos.x += Random.Range(-Camera.main.orthographicSize / 2, Camera.main.orthographicSize / 2);
			Instantiate(WrenchPrefab, pos, transform.rotation);
		}
	}
}
