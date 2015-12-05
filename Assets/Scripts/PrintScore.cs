using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PrintScore : MonoBehaviour {

	GameObject goldText;
	GameObject silverText;
	GameObject bronzeText;
	
	// Use this for initialization
	void Start () {
		goldText = GameObject.Find("GoldText");
		silverText = GameObject.Find("SilverText");
		bronzeText = GameObject.Find("BronzeText");
		goldText.GetComponent<Text>().text = ("X " + PlayerPrefs.GetInt("gold"));
		silverText.GetComponent<Text>().text = ("X " + PlayerPrefs.GetInt("silver"));
		bronzeText.GetComponent<Text>().text = ("X " + PlayerPrefs.GetInt("bronze"));
	}
}
