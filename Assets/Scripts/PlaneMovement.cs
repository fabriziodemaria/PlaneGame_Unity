using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaneMovement : MonoBehaviour {

	public GameObject ExplosionPrefab; //Needs a better place
	public Vector3 velocity;
	public float maxLateralRoll;
	public float maxLateralYaw;
	public int maxLateralForce;
	public float fallingRate;

	/* Logic */
	private bool isDead = false;

	/* Physics */
	private Vector3 lateralForce;
	private float lateralBoundaries;
	private float currentRotationY = 0.0f;
	private float currentRotationZ = 0.0f;
	private bool lateralVelocityChanged = false;

	/* View */
	public int rotationSpeed;
	private GameObject currentExplosion; //Needs a better place
	private int explosionDirectionX;
	private float rotationTimer = 0;
	
	void Start () {
		lateralBoundaries = Camera.main.orthographicSize / 2;
	}

	void FixedUpdate () {
		/* Forward speed */
		transform.position += velocity * Time.deltaTime;

		/* Handle dead trigger */
		if (isDead && transform.localScale.x >= 0) {

			/* Handle explosion graphics */
			// Probably bettere elsewhere...
			Vector3 expPos = currentExplosion.transform.position;
			expPos.y = transform.position.y;
			expPos.x = transform.position.x + explosionDirectionX * transform.localScale.x / 2;
			currentExplosion.transform.position = expPos;

			Vector3 planescale = transform.localScale; 
			planescale.x -= fallingRate * Time.deltaTime;
			planescale.y -= fallingRate * Time.deltaTime;
			transform.localScale = planescale;

			if (transform.localScale.x <= 0) {
				velocity = new Vector3 (0, 0, 0);
				GameObject.FindObjectOfType<Canvas> ().GetComponent<LabelsManager> ().showGameOver ();
				Destroy (GameObject.FindObjectOfType<CloudSpawner> ());
				Destroy (GameObject.FindObjectOfType<BirdSpanwer> ());
				currentExplosion.GetComponent<ParticleSystem>().emissionRate = 0;
			}
		}

		/* Handle lateral boundaries */
		if (!isDead && ((transform.position.x < -lateralBoundaries && lateralForce.x < 0) || 
			(transform.position.x > lateralBoundaries && lateralForce.x > 0))) {
			lateralVelocityChanged = true;
			lateralForce = new Vector3 ();
		} 

		/* Handle lateral force and rotation */
		if (lateralForce.x != 0) {
			transform.position += lateralForce * Time.deltaTime;
//			float instantRatio = Mathf.Lerp (0, 1, Mathf.Abs(currentRotationY) / (Mathf.Lerp (0, maxLateralRoll, (Mathf.Abs(lateralForce.x) / maxLateralForce))));
//			Debug.Log("Current ratio " + instantRatio);
//			Debug.Log("Current subratio " + (Mathf.Abs(currentRotationY) / (Mathf.Lerp (0, maxLateralRoll, (Mathf.Abs(lateralForce.x) / maxLateralForce)))));
//			float instantLateralForce = lateralForce.x * instantRatio;
//			Vector3 instantForce = new Vector3(instantLateralForce, lateralForce.y, lateralForce.z);
//			transform.position += instantForce * Time.deltaTime;
		}

		if (lateralVelocityChanged) {
			lateralVelocityChanged = false;
			currentRotationY = transform.rotation.y * 100;
			currentRotationZ = transform.rotation.z * 100;
			rotationTimer = 0;
		}
		if (lateralForce.x > 0) {
			rotationTimer += Time.deltaTime * rotationSpeed;
			transform.rotation = Quaternion.Euler (0, 
			                                       Mathf.SmoothStep (currentRotationY, Mathf.Lerp (0, -maxLateralRoll, lateralForce.x / maxLateralForce), rotationTimer), 
			                                       Mathf.SmoothStep (currentRotationZ, Mathf.Lerp (0, -maxLateralYaw, lateralForce.x / maxLateralForce), rotationTimer));
		} else if (lateralForce.x < 0) {
			rotationTimer += Time.deltaTime * rotationSpeed;
			transform.rotation = Quaternion.Euler (0, 
			                                       Mathf.SmoothStep (currentRotationY, Mathf.Lerp (0, maxLateralRoll, -lateralForce.x / maxLateralForce), rotationTimer), 
			                                       Mathf.SmoothStep (currentRotationZ, Mathf.Lerp (0, maxLateralYaw, -lateralForce.x / maxLateralForce), rotationTimer));
		} else {
			rotationTimer += Time.deltaTime * rotationSpeed;
			transform.rotation = Quaternion.Euler (0, Mathf.SmoothStep (currentRotationY, 0, rotationTimer), Mathf.SmoothStep (currentRotationZ, 0, rotationTimer));
		}
	}

	void Update() {
		if (Input.GetMouseButtonUp(0) && GetComponent<PlaneMovement> ().velocity.y == 0) {
			Application.LoadLevel( Application.loadedLevel);
		}

		if (Input.GetMouseButtonDown(0)) {
			if (Input.mousePosition.x < Screen.width / 2) {
				Debug.Log("Left " + Input.mousePosition.x);
				if (lateralForce.x > -maxLateralForce && transform.position.x > -lateralBoundaries) {
					lateralVelocityChanged = true;
					lateralForce.x--;
				}
			} else {
				Debug.Log("Right " + Input.mousePosition.x);
				if (lateralForce.x < maxLateralForce && transform.position.x < lateralBoundaries) {
					lateralVelocityChanged = true;
					lateralForce.x++;
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (isDead == true)
			return;

		isDead = true;
		Vector3 pos = transform.position;
		if (collider.transform.position.x < transform.position.x)
			explosionDirectionX = -1;
		else 
			explosionDirectionX = 1;
		pos.x += explosionDirectionX * transform.localScale.x / 2;
		currentExplosion = (GameObject)Instantiate(ExplosionPrefab, pos, Quaternion.Euler(0,0,0));
	}
}
