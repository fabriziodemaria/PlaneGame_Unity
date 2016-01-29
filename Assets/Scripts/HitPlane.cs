using UnityEngine;
using System.Collections;

public class HitPlane : MonoBehaviour {

	public float blinkRate;
	public float blinkTime;
	private bool active = false;

	// Use this for initialization
	void Start () {
	
	}

	public void StartGodMode () {
		if (active) return;
		StartCoroutine(BlinkingPlane(blinkTime));
		this.gameObject.GetComponent<Renderer>().enabled = false;
		this.GetComponent<PlaneMovement>().godMode = true;
		active = true;
	}


	IEnumerator BlinkingPlane (float blinkingTime) {
		float endtime = Time.time + blinkingTime;
		while (Time.time < endtime) {
			this.gameObject.GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(blinkRate);
			this.gameObject.GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(blinkRate);
		}
		this.GetComponent<PlaneMovement>().godMode = false;
		active = false;
	}
}
