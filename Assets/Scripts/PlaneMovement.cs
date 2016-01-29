using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaneMovement : MonoBehaviour {

	public GameObject ExplosionPrefab; //Needs a better place TODO
	public GameObject FocalExplosionPrefab;
	public GameObject SplashFocalPrefab;
	public Vector3 velocity;
	public float maxLateralRoll;
	public float maxLateralYaw;
	public int maxLateralForce;
	public float fallingRate;
	public float LongPressSensitivity;

	/* Logic */
	private bool isDead = false; //Variable that tells you if the plane has fallen 
	public int maxLifes = 2;
	private int currentHits = 0;
	public bool godMode = false;
	private bool currentHitLeft = false;
	private bool currentHitRight = false;
	private FuelBarController fuelBarControl;
	private bool clicked;
	private float clickTime;
	private float clickPos;

	/* Physics */
	public Vector3 lateralForce;
	private float lateralBoundaries;
	private float currentRotationY = 0.0f;
	private float currentRotationZ = 0.0f;
	private bool lateralVelocityChanged = false;

	/* View */
	public int rotationSpeed;
	private GameObject[] currentExplosions; //Needs a better place TODO
	private GameObject smokeParticles;
	private int[] explosionDirectionX;
	private float rotationTimer = 0;
	private int moviolaFrames;
	private bool isBackButtonPressed = false;

	/* Sound */
	public GameObject hitClip;
	public GameObject parachutesClip;

	void Awake () {
		Application.targetFrameRate = 60;
	}

	public Vector3 LateralForce {
		get {
			return lateralForce;
		}
		set {
			lateralForce = value;
			Debug.Log("Changed force");
			GameObject.FindObjectOfType<ArrowsScript>().updateArrows(lateralForce);
		}
	}
	
	void Start () {
		moviolaFrames = 0;
		lateralBoundaries = Camera.main.orthographicSize / 2;
		currentExplosions = new GameObject[maxLifes];
		explosionDirectionX = new int[maxLifes];
		fuelBarControl = GameObject.FindObjectOfType<FuelBarController>();
		if (fuelBarControl == null) {
			Debug.LogError("Fuel bar not found!");
			return;
		}
	}

	void FixedUpdate () {
		/* Forward speed */
		transform.position += velocity * Time.deltaTime;
		transform.position += LateralForce * Time.deltaTime;

		for (int i = 0; i < currentHits; i++) {
			/* Handle explosion graphics */
			// Probably bettere elsewhere... TODO
			if (currentExplosions[i] != null) {
				Vector3 expPos = currentExplosions[i].transform.position;
				expPos.y = transform.position.y;
				expPos.x = transform.position.x + explosionDirectionX[i] * transform.localScale.x / 2;
				currentExplosions[i].transform.position = expPos;
			}
		}

		/* Handle dead trigger */
		if (isDead && transform.localScale.x >= 0) {
			Vector3 planescale = transform.localScale; 
			planescale.x -= fallingRate * Time.deltaTime;
			planescale.y -= fallingRate * Time.deltaTime;
			transform.localScale = planescale;

			if (planescale.x <=0) {
				if (transform.position.y < 50) {
					Instantiate(SplashFocalPrefab, transform.position, Quaternion.Euler(0,180,180));
				}
				else
					Instantiate(FocalExplosionPrefab, transform.position, Quaternion.Euler(0,180,180));
			}
			return;
		}

		if (isDead && transform.localScale.x <= 0) {
			velocity = new Vector3 (0, 0, 0);
			GameObject.FindObjectOfType<Canvas> ().GetComponent<LabelsManager> ().showGameOver ();
			for (int i = 0; i < maxLifes; i++) {
				if (currentExplosions[i] != null) {
					ParticleSystem.EmissionModule em = currentExplosions[i].GetComponent<ParticleSystem>().emission;
					em.enabled = false;
				}
			}
			return;
		}

		/* Handle lateral boundaries */
		if (!isDead && ((transform.position.x < -lateralBoundaries && LateralForce.x < 0) || 
			(transform.position.x > lateralBoundaries && LateralForce.x > 0))) {
			lateralVelocityChanged = true;
			LateralForce = new Vector3 ();
		} 

		if (lateralVelocityChanged) {
			lateralVelocityChanged = false;
			currentRotationY = transform.rotation.y * 100;
			currentRotationZ = transform.rotation.z * 100;
			rotationTimer = 0;
		}

		if (LateralForce.x > 0) {
			rotationTimer += Time.deltaTime * rotationSpeed;
			transform.rotation = Quaternion.Euler (0, 
				Mathf.SmoothStep (currentRotationY, Mathf.Lerp (0, -maxLateralRoll, LateralForce.x / maxLateralForce), rotationTimer), 
				Mathf.SmoothStep (currentRotationZ, Mathf.Lerp (0, -maxLateralYaw, LateralForce.x / maxLateralForce), rotationTimer));
		} else if (LateralForce.x < 0) {
			rotationTimer += Time.deltaTime * rotationSpeed;
			transform.rotation = Quaternion.Euler (0, 
				Mathf.SmoothStep (currentRotationY, Mathf.Lerp (0, maxLateralRoll, -LateralForce.x / maxLateralForce), rotationTimer), 
				Mathf.SmoothStep (currentRotationZ, Mathf.Lerp (0, maxLateralYaw, -LateralForce.x / maxLateralForce), rotationTimer));
		} else {
			rotationTimer += Time.deltaTime * rotationSpeed;
			transform.rotation = Quaternion.Euler (0, Mathf.SmoothStep (currentRotationY, 0, rotationTimer), Mathf.SmoothStep (currentRotationZ, 0, rotationTimer));
		}
	}

	void Update() {
		/* Moviola effect */
		if (moviolaFrames > 90) {
			moviolaFrames--;
			Time.timeScale -= Time.timeScale * 0.07f;
		} else if (moviolaFrames > 0 && moviolaFrames < 10) {
			moviolaFrames--;
			Time.timeScale += Time.timeScale * 0.07f;
		} else if (moviolaFrames == 0) {
			Time.timeScale = 1;
		} else {
			moviolaFrames--;
		}
		/* End Moviola effect */

		if (isBackButtonPressed || isDead) return;

		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown(0)) {
			clicked = true;
			clickTime = Time.time;

			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				clickPos = 10;
			} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
				clickPos = Screen.width - 10;
			} else {
				clickPos = Input.mousePosition.x;
			}
			handlePlayerInput(clickPos, false);
		}

		if (clicked && (Time.time - clickTime) > LongPressSensitivity) {
			if (Input.GetMouseButton(0))
				handlePlayerInput(Input.mousePosition.x, true);
			else 
				handlePlayerInput(clickPos, true);
			clicked=false;
		}

		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetMouseButtonUp(0)) {
			clicked = false;
		}
	}

	void handlePlayerInput(float posX, bool longPress) {
		if (posX < Screen.width / 2) {
			if (LateralForce.x > -maxLateralForce && transform.position.x > -lateralBoundaries) {
				Debug.Log("Boh");
				lateralVelocityChanged = true;
				if (longPress)
					LateralForce = new Vector3(-maxLateralForce, LateralForce.y, LateralForce.z);
				else
					LateralForce = new Vector3(LateralForce.x - 1, LateralForce.y, LateralForce.z);
			}
		} else {
			if (LateralForce.x < maxLateralForce && transform.position.x < lateralBoundaries) {
				lateralVelocityChanged = true;
				if (longPress)
					LateralForce = new Vector3(maxLateralForce, LateralForce.y, LateralForce.z);
				else
					LateralForce = new Vector3(LateralForce.x + 1, LateralForce.y, LateralForce.z);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (isDead == false && collider.tag == "Wrench") {
			Destroy (collider.gameObject);
			// fuelBarControl.moreFuel();
			if (currentHits > 0) {
				currentHits--;
				currentHitLeft = false;
				currentHitRight = false;
				ParticleSystem ps = currentExplosions[currentHits].GetComponent<ParticleSystem>();
				ParticleSystem.EmissionModule em = ps.emission;
				em.enabled = false;
			}
			parachutesClip.GetComponent<AudioSource>().Play();
			return;
		}

		if (isDead == false && collider.tag == "Tank") {
			Destroy (collider.gameObject);
			fuelBarControl.moreFuel();
			parachutesClip.GetComponent<AudioSource>().Play();
			return;
		}

		if (isDead || godMode) return;

		if(!(currentHits+1 >= maxLifes))
			this.GetComponent<HitPlane>().StartGodMode(); //this enables godMode bool
		bool increaseHit = true;

		if (collider.transform.position.x < transform.position.x) {
			if (currentHitLeft == false) {
				explosionDirectionX[currentHits] = -1;
				currentHitLeft = true;
				increaseHit = true;
				hitClip.GetComponent<AudioSource>().Play();
			}
		} else { 
			if (currentHitRight == false) {
				explosionDirectionX[currentHits] = 1;
				currentHitRight = true;
				increaseHit = true;
				hitClip.GetComponent<AudioSource>().Play();
			}
		}

		//Return if the wing was already hit
		if (!increaseHit) {
			return;
		}

		Vector3 pos = transform.position;
		pos.x += explosionDirectionX[currentHits] * transform.localScale.x / 2;
		Instantiate(FocalExplosionPrefab, pos, Quaternion.Euler(0,180,180));
		currentExplosions.SetValue((GameObject)Instantiate(ExplosionPrefab, pos, Quaternion.Euler(-90,0,0)), currentHits);

		// Move to the next position for the explosions array and logic checks
		currentHits++;

		if (currentHits >= maxLifes)
			killPlane();
	}

	public void killPlane() {
		moviolaFrames = 100;
		isDead = true;
		GameObject.FindObjectOfType<ArrowsScript>().updateArrows(lateralForce);
	}

	public bool isGameOver() {
		return isDead;
	}

	public void bbPressed() {
		isBackButtonPressed = true;
	}

	public void bbUnpressed() {
		isBackButtonPressed = false;
	}
}
