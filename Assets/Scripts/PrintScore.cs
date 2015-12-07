using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrintScore : MonoBehaviour {

	private GameObject goldText;
	private GameObject silverText;
	private GameObject bronzeText;
	private GameObject recordLabel;
	
	// Use this for initialization
	void Start () {
		goldText = GameObject.Find("GoldText");
		silverText = GameObject.Find("SilverText");
		bronzeText = GameObject.Find("BronzeText");
		recordLabel = GameObject.Find("Highscore");
		goldText.GetComponent<Text>().text = ("X " + PlayerPrefs.GetInt("gold"));
		silverText.GetComponent<Text>().text = ("X " + PlayerPrefs.GetInt("silver"));
		bronzeText.GetComponent<Text>().text = ("X " + PlayerPrefs.GetInt("bronze"));
		recordLabel.GetComponent<Text>().text = ("Highscore: " + PlayerPrefs.GetFloat("highscore").ToString("F2"));
	}
}
