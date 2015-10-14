using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaneMovement : MonoBehaviour {
	
	public Vector3 velocity;
	public float maxLateralRoll;
	public int maxLateralForce;
	public float fallingRate;

	/* Logic */
	private bool isDead = false;

	/* Physics */
	private Vector3 lateralForce;
	private float lateralBoundaries;
	
	void Start () {
		lateralBoundaries = Camera.main.orthographicSize / 2;
	}

	void FixedUpdate () {
		transform.position += velocity * Time.deltaTime;

		/* Handle dead trigger */
		if (isDead && transform.localScale.x >= 0) {
			Vector3 planescale = transform.localScale; 
			planescale.x -= fallingRate * Time.deltaTime;
			planescale.y -= fallingRate * Time.deltaTime;
			transform.localScale = planescale;
			if (transform.localScale.x <= 0) {
				velocity = new Vector3 (0, 0, 0);
				GameObject.FindObjectOfType<Canvas> ().GetComponent<LabelsManager> ().showGameOver ();
				Destroy (GameObject.FindObjectOfType<CloudSpawner> ());
			}
		}

		/* Handle lateral boundaries */
		if (!isDead && ((transform.position.x < -lateralBoundaries && lateralForce.x < 0) || 
			(transform.position.x > lateralBoundaries && lateralForce.x > 0))) {
			lateralForce = new Vector3 ();
		} 

		/* Handle lateral force and rotation */
		transform.position += lateralForce * Time.deltaTime;
		if (lateralForce.x > 0) {
			transform.rotation = Quaternion.Euler (0, Mathf.Lerp (0, maxLateralRoll, lateralForce.x / maxLateralForce), 0);
		} else if (lateralForce.x < 0) {
			transform.rotation = Quaternion.Euler (0, -Mathf.Lerp (0, maxLateralRoll, -lateralForce.x / maxLateralForce), 0);
		} else {
			transform.rotation = Quaternion.Euler (0, 0, 0);
		}
	}

	void Update() {
		if (Input.GetMouseButtonUp(0) && GetComponent<PlaneMovement> ().velocity.y == 0) {
			Application.LoadLevel( Application.loadedLevel);
		}

		if (Input.GetMouseButtonDown(0)) {
			if (Input.mousePosition.x < Screen.width / 2) {
				Debug.Log("Left " + Input.mousePosition.x);
				if (lateralForce.x > -maxLateralForce && transform.position.x > -lateralBoundaries)
					lateralForce.x--;
			} else {
				Debug.Log("Right " + Input.mousePosition.x);
				if (lateralForce.x < maxLateralForce && transform.position.x < lateralBoundaries)
					lateralForce.x++;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		isDead = true;
	}
}
