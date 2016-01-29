using UnityEngine;
using System.Collections;

public class SpawnersController : MonoBehaviour {

	private GameObject meteoriteSpawner;
	private GameObject cloudSpawner;
	private GameObject playerPlane;

	// Use this for initialization
	void Start () {
		playerPlane = GameObject.FindGameObjectWithTag ("Player");
		if (playerPlane == null) {
			Debug.LogError("No plane found!");
			return;
		}
		meteoriteSpawner = GameObject.Find("MeteoriteSpawner");
		cloudSpawner = GameObject.Find("CloudSpawner");
		meteoriteSpawner.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (playerPlane.GetComponent<PlaneMovement>().isGameOver() && playerPlane.transform.localScale.x <= 0) {
			Destroy (GameObject.Find("CloudSpawner"));
			Destroy (GameObject.Find("MeteoriteSpawner"));
			Destroy (GameObject.Find("BirdxSpawner"));
			Destroy (GameObject.Find("WrenchSpawner"));
			Destroy (GameObject.Find("TankSpawner"));
			return;
		}
		if (playerPlane.transform.position.y > 150 && meteoriteSpawner.activeSelf == false) {
			cloudSpawner.SetActive(false);
			meteoriteSpawner.SetActive(true);
		}
	}
}
