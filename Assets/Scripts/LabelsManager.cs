﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LabelsManager : MonoBehaviour {

	public GameObject playerPlane;

	private float score = 0;
	private float scoreOffset;

	private Text GOLabel;
	private Text SCLabel;

	// Use this for initialization
	void Start () {
		GOLabel = GameObject.FindGameObjectWithTag ("GameOverLabel").GetComponent<Text>();
		if (GOLabel != null) {
			GOLabel.enabled = false;
		} else {
			Debug.LogError("No Game Over label has been found!");
			return;
		}

		SCLabel = GameObject.FindGameObjectWithTag ("ScoreLabel").GetComponent<Text>();
		if (SCLabel != null) {
			SCLabel.enabled = true;
		} else {
			Debug.LogError("No Score label has been found!");
			return;
		}

		playerPlane = GameObject.FindGameObjectWithTag ("Player");
		if (playerPlane == null) {
			Debug.LogError("No plane found!");
			return;
		}

		scoreOffset = playerPlane.transform.position.y;
	}

	void Update() {
		score = playerPlane.transform.position.y - scoreOffset;
		SCLabel.text = "Miles: " + score.ToString("F2");
	}
	
	public void showGameOver () {
		SCLabel.enabled = false;
		GOLabel.text = "GAME OVER!\nMILES: " + score.ToString ("F2");
		GOLabel.enabled = true;
	}
}
