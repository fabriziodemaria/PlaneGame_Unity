using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LabelsManager : MonoBehaviour {

	private GameObject playerPlane;
	public Image medalImg;

	public int bronzeScore;
	public int silverScore;
	public int goldScore;

	private float score = 0;
	private float scoreOffset;
	private float highscore = 0;

	private Text GOLabel;
	private Text SCLabel;
	private GameObject RetryButton;

	public float Score {
		get {
			return score;
		}
	}

	// Use this for initialization
	void Start () {

		highscore = PlayerPrefs.GetFloat("highscore", 0);
		medalImg.enabled = false;

		GOLabel = GameObject.FindGameObjectWithTag ("GameOverLabel").GetComponent<Text>();
		if (GOLabel != null) {
			GOLabel.enabled = false;
		} else {
			Debug.LogError("No Game Over label has been found!");
			return;
		}

		RetryButton = GameObject.Find("RetryButton");
		if (RetryButton != null) {
			RetryButton.GetComponent<Button>().gameObject.SetActive(false);
		} else {
			Debug.LogError("No RetryButton found!");
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
		SCLabel.text = "Miles: " + score.ToString("F2") + "\nHighscore: " + highscore.ToString("F2");
	}
	
	public void showGameOver () {

		/* Do not execute this code if the game is over already */
		/* A bool system is probably more secure */
		if (GOLabel.IsActive())
			return;

		RetryButton.GetComponent<Button>().gameObject.SetActive(true);

		SCLabel.enabled = false;

		if (score > PlayerPrefs.GetFloat("highscore", 0)) {
			highscore = score;
			PlayerPrefs.SetFloat("highscore", highscore);
			GOLabel.text = "NEW RECORD!\nHIGHSCORE: " + highscore.ToString("F2");
		} else {
			GOLabel.text = "GAME OVER!\nMILES: " + score.ToString ("F2") + "\nHIGHSCORE: " + highscore.ToString("F2");
		}
		GOLabel.enabled = true;

		if (score < bronzeScore)
			return;

		if (score >= bronzeScore && score < silverScore) {
			int totalBronze = PlayerPrefs.GetInt("bronze", 0);
			medalImg.sprite = Resources.Load("Medals/bronze", typeof(Sprite)) as Sprite;
			PlayerPrefs.SetInt("bronze", totalBronze+1);
			Debug.Log("Bronze medals " + PlayerPrefs.GetInt("bronze"));
		} else if (score >= silverScore && score < goldScore) {
			int totalSilver = PlayerPrefs.GetInt("silver", 0);
			medalImg.sprite = Resources.Load("Medals/silver", typeof(Sprite)) as Sprite;
			PlayerPrefs.SetInt("silver", totalSilver+1);
			Debug.Log("Silver medals " + PlayerPrefs.GetInt("silver"));
		} else if (score >= goldScore) {
			int totalGold = PlayerPrefs.GetInt("gold", 0);
			medalImg.sprite = Resources.Load("Medals/gold", typeof(Sprite)) as Sprite;
			PlayerPrefs.SetInt("gold", totalGold+1);
			Debug.Log("Gold medals " + PlayerPrefs.GetInt("gold"));
		}
		medalImg.enabled = true;
	}
}
