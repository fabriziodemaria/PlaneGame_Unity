using UnityEngine;
using System.Collections;

public class BirdSpanwer : MonoBehaviour {
	
	public GameObject BirdPrefab;
	public float sDealy = 3f;
	float nextS = 1f;
	
	// Update is called once per frame
	void FixedUpdate () {
		nextS -= Time.deltaTime;
		
		if (nextS <= 0) {
			nextS = sDealy;
			Vector3 pos = transform.position;
			pos.x += Random.Range(-Camera.main.orthographicSize / 2, Camera.main.orthographicSize / 2);
			Debug.Log("pos x " + pos.x);
			Instantiate(BirdPrefab, pos, transform.rotation);
			pos.x += 0.3f;
			pos.y += 0.3f;
			Instantiate(BirdPrefab, pos, transform.rotation);
			pos.x += 0.3f;
			pos.y += 0.3f;
			Instantiate(BirdPrefab, pos, transform.rotation);
		}
	}
}
