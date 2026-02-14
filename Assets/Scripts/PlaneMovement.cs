using UnityEngine;
using System.Collections;

/// <summary>
/// Main player controller for the plane. Handles movement, collisions, damage, and death.
/// </summary>
public class PlaneMovement : MonoBehaviour
{
	[Header("Prefabs")]
	[SerializeField] private GameObject explosionPrefab;
	[SerializeField] private GameObject focalExplosionPrefab;
	[SerializeField] private GameObject splashFocalPrefab;

	[Header("Movement Settings")]
	[SerializeField] private Vector3 velocity = new Vector3(0, 5, 0);
	[SerializeField] private float maxLateralRoll = 30f;
	[SerializeField] private float maxLateralYaw = 15f;
	[SerializeField] private int maxLateralForce = 3;
	[SerializeField] private float fallingRate = 2f;
	[SerializeField] private int rotationSpeed = 5;

	[Header("Input Settings")]
	[SerializeField] private float longPressSensitivity = 0.3f;

	[Header("Gameplay Settings")]
	[SerializeField] private int maxLifes = 2;
	[SerializeField] public bool godMode = false;

	[Header("Audio")]
	[SerializeField] private GameObject hitClip;
	[SerializeField] private GameObject parachutesClip;

	// Logic state
	private bool isDead = false;
	private int currentHits = 0;
	private bool currentHitLeft = false;
	private bool currentHitRight = false;
	private bool clicked;
	private float clickTime;
	private float clickPos;

	// Cached references
	private FuelBarController fuelBarControl;
	private ArrowsScript arrowsScript;
	private LabelsManager labelsManager;
	private SpriteRenderer spriteRenderer;

	// Physics
	public Vector3 lateralForce;
	private float lateralBoundaries;
	private float currentRotationY = 0.0f;
	private float currentRotationZ = 0.0f;
	private bool lateralVelocityChanged = false;

	// Visual effects
	private GameObject[] currentExplosions;
	private int[] explosionDirectionX;
	private float rotationTimer = 0;
	private int moviolaFrames;

	void Awake () {
		Application.targetFrameRate = 60;
	}

	public Vector3 LateralForce
	{
		get { return lateralForce; }
		set
		{
			lateralForce = value;
			if (arrowsScript != null)
				arrowsScript.updateArrows(lateralForce);
		}
	}

	void Start()
	{
		// Initialize state
		moviolaFrames = 0;
		currentExplosions = new GameObject[maxLifes];
		explosionDirectionX = new int[maxLifes];

		// Cache component references
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer != null)
		{
			spriteRenderer.sortingLayerName = LayerConstants.SORT_PLAYER;
			spriteRenderer.sortingOrder = LayerConstants.ORDER_PLAYER;
		}

		// Set proper Z-depth for player
		Vector3 pos = transform.position;
		pos.z = LayerConstants.Z_PLAYER;
		transform.position = pos;

		// Cache external references
		fuelBarControl = FindObjectOfType<FuelBarController>();
		if (fuelBarControl == null)
		{
			Debug.LogError("PlaneMovement: FuelBarController not found!");
		}

		arrowsScript = FindObjectOfType<ArrowsScript>();
		if (arrowsScript == null)
		{
			Debug.LogWarning("PlaneMovement: ArrowsScript not found!");
		}

		labelsManager = FindObjectOfType<LabelsManager>();
		if (labelsManager == null)
		{
			Debug.LogWarning("PlaneMovement: LabelsManager not found!");
		}

		// Create PauseManager if one does not exist yet
		if (PauseManager.Instance == null)
		{
			GameObject pauseObj = new GameObject("PauseManager");
			pauseObj.AddComponent<PauseManager>();
		}

		// Calculate lateral boundaries
		if (Camera.main != null)
		{
			lateralBoundaries = Camera.main.orthographicSize / 2f;
		}
		else
		{
			Debug.LogWarning("PlaneMovement: Main camera not found!");
			lateralBoundaries = 2.5f;
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

			if (planescale.x <= 0) {
				if (transform.position.y < 50) {
					Instantiate(splashFocalPrefab, transform.position, Quaternion.Euler(0, 180, 180));
				}
				else {
					Instantiate(focalExplosionPrefab, transform.position, Quaternion.Euler(0, 180, 180));
				}
			}
			return;
		}

		if (isDead && transform.localScale.x <= 0) {
			velocity = Vector3.zero;
			if (labelsManager != null)
			{
				labelsManager.showGameOver();
			}
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
		// Skip all gameplay logic while paused
		if (PauseManager.Instance != null && PauseManager.Instance.IsPaused) return;

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

		if (clicked && (Time.time - clickTime) > longPressSensitivity) {
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
		Instantiate(focalExplosionPrefab, pos, Quaternion.Euler(0, 180, 180));
		currentExplosions.SetValue((GameObject)Instantiate(explosionPrefab, pos, Quaternion.Euler(-90, 0, 0)), currentHits);

		// Move to the next position for the explosions array and logic checks
		currentHits++;

		if (currentHits >= maxLifes)
			killPlane();
	}

	public void killPlane()
	{
		moviolaFrames = 100;
		isDead = true;
		if (arrowsScript != null)
		{
			arrowsScript.updateArrows(lateralForce);
		}
	}

	public bool isGameOver() {
		return isDead;
	}

	public void bbPressed() {
		if (PauseManager.Instance != null)
			PauseManager.Instance.TogglePause();
	}

	public void bbUnpressed() {
		// No longer needed — pause is toggled by bbPressed
	}
}
