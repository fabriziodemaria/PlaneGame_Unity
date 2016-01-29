using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FuelBarController : MonoBehaviour {

	public Image fuelBarImage;
	public float fuelConsumptiom;
	private PlaneMovement planeController;
	private Text NLabel;
	private bool isLow;

	// Use this for initialization
	void Start () {
		fuelBarImage.fillAmount = 1f;
		planeController = GameObject.FindObjectOfType<PlaneMovement>();
		if (planeController == null) {
			Debug.LogError("No plane found!");
			return;
		}

		NLabel = GameObject.FindGameObjectWithTag("NotificationLabel").GetComponent<Text>();
		if (NLabel == null) {
			Debug.LogError("No Notification label has been found!");
			return;
		}
		NLabel.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if (planeController.isGameOver()) {
			NLabel.text = "";			
			GameObject.Destroy(this);
			return;
		}
		
		if (fuelBarImage.fillAmount <= 0) {
			planeController.killPlane();
			NLabel.text = "";	
			return;
		}

		Color barColor = Color.white;
		if (fuelBarImage.fillAmount <= 0.3f) {
			if (!isLow) {
				isLow = true;
				NLabel.text = "LOW FUEL";
			}
			barColor = Color.red;
		} else {
			if (isLow) {
				isLow = false;
				NLabel.text = "";
			}
		}
		barColor.a = 0.6f;
		fuelBarImage.color = barColor;
		fuelBarImage.fillAmount -= Time.deltaTime * fuelConsumptiom;
	}

	public void moreFuel() {
		fuelBarImage.fillAmount += 0.2f;
	}
}
