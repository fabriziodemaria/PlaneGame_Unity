using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FuelBarController : MonoBehaviour {

	public Image fuelBarImage;
	public float fuelConsumptiom;

	private PlaneMovement planeController;

	// Use this for initialization
	void Start () {
		fuelBarImage.fillAmount = 1f;
		planeController = GameObject.FindObjectOfType<PlaneMovement>();
		if (planeController == null) {
			Debug.LogError("No plane found!");
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fuelBarImage.fillAmount <= 0)
			planeController.killPlane();
		if (planeController.velocity.y == 0)
			return;
		fuelBarImage.fillAmount -= Time.deltaTime * fuelConsumptiom;
	}

	public void moreFuel() {
		fuelBarImage.fillAmount += 0.2f;
	}
}
