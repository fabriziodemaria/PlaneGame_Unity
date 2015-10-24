using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	private bool isDead = false;
	public int maxLifes = 2;
	private int currentHits = 0;
	private FuelBarController fuelBarControl;
	private bool clicked;
	private float clickTime;
	private float clickPos;

	/* Physics */
	private Vector3 lateralForce;
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

	void Awake () {
		Application.targetFrameRate = 300;
	}
	
	void Start () {
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
		transform.position += lateralForce * Time.deltaTime;

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

			if (planescale.x <=0)
				Instantiate(SplashFocalPrefab, transform.position, Quaternion.Euler(0,180,180));
			return;
		}

		if (isDead && transform.localScale.x <= 0) {
			velocity = new Vector3 (0, 0, 0);
			GameObject.FindObjectOfType<Canvas> ().GetComponent<LabelsManager> ().showGameOver ();
			Destroy (GameObject.FindObjectOfType<CloudSpawner> ());
			Destroy (GameObject.FindObjectOfType<BirdSpanwer> ());
			Destroy (GameObject.FindObjectOfType<GenerateBox> ());
			for (int i = 0; i < maxLifes; i++) {
				if (currentExplosions[i] != null)
					currentExplosions[i].GetComponent<ParticleSystem>().emissionRate = 0;
			}
			return;
		}

		/* Handle lateral boundaries */
		if (!isDead && ((transform.position.x < -lateralBoundaries && lateralForce.x < 0) || 
			(transform.position.x > lateralBoundaries && lateralForce.x > 0))) {
			lateralVelocityChanged = true;
			lateralForce = new Vector3 ();
		} 

		/* Handle lateral force and rotation */
//		if (lateralForce.x != 0) {
			/* Dynamic velocity change according to rotation TODO */
//			float instantRatio = Mathf.Lerp (0, 1, Mathf.Abs(currentRotationY) / (Mathf.Lerp (0, maxLateralRoll, (Mathf.Abs(lateralForce.x) / maxLateralForce))));
//			Debug.Log("Current ratio " + instantRatio);
//			Debug.Log("Current subratio " + (Mathf.Abs(currentRotationY) / (Mathf.Lerp (0, maxLateralRoll, (Mathf.Abs(lateralForce.x) / maxLateralForce)))));
//			float instantLateralForce = lateralForce.x * instantRatio;
//			Vector3 instantForce = new Vector3(instantLateralForce, lateralForce.y, lateralForce.z);
//			transform.position += instantForce * Time.deltaTime;
//		}

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

		if (moviolaFrames > 0) {
			moviolaFrames--;
			if (moviolaFrames == 0) {
				Time.timeScale = 1;
			}
		}

		if ((Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space)) && GetComponent<PlaneMovement> ().velocity.y == 0) {
			Application.LoadLevel( Application.loadedLevel);
		}

		if (isDead) return;

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
			if (lateralForce.x > -maxLateralForce && transform.position.x > -lateralBoundaries) {
				lateralVelocityChanged = true;
				if (longPress)
					lateralForce.x = -maxLateralForce;
				else
					lateralForce.x--;
			}
		} else {
			if (lateralForce.x < maxLateralForce && transform.position.x < lateralBoundaries) {
				lateralVelocityChanged = true;
				if (longPress)
					lateralForce.x = maxLateralForce;
				else
					lateralForce.x++;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (isDead == false && collider.tag == "Wrench") {
			Destroy (collider.gameObject);
			fuelBarControl.moreFuel();
			if (currentHits > 0) {
				currentHits--;
				currentExplosions[currentHits].GetComponent<ParticleSystem>().emissionRate = 0;
			}
			return;
		}

		if (isDead == true)
			return;

		currentHits++;
		Vector3 pos = transform.position;
		if (collider.transform.position.x < transform.position.x)
			explosionDirectionX[currentHits-1] = -1;
		else 
			explosionDirectionX[currentHits-1] = 1;
		pos.x += explosionDirectionX[currentHits-1] * transform.localScale.x / 2;
		Instantiate(FocalExplosionPrefab, pos, Quaternion.Euler(0,180,180));
		currentExplosions.SetValue((GameObject)Instantiate(ExplosionPrefab, pos, Quaternion.Euler(-90,0,0)), currentHits-1);
		if (currentHits >= maxLifes)
			killPlane();
	}

	public void killPlane() {
		Time.timeScale = 0.1f;
		moviolaFrames = 100;
		isDead = true;
	}

	public bool isGameOver() {
		return isDead;
	}
}
