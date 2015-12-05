using UnityEngine;
using System.Collections;

public class ObjSpawner: MonoBehaviour {
	
	public GameObject inputPrefab;
	public float frequency;
	public float firstDelay;
	
	// Update is called once per frame
	void FixedUpdate () {
		firstDelay -= Time.deltaTime;
		
		if (firstDelay <= 0) {
			firstDelay = frequency;
			Vector3 pos = transform.position;
			pos.x += Random.Range(-Camera.main.orthographicSize / 2, Camera.main.orthographicSize / 2);
			Instantiate(inputPrefab, pos, transform.rotation);
		}
	}
}
