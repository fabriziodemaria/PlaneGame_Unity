using UnityEngine;
using System.Collections;

public class ResetStats : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DeleteAll() {
		PlayerPrefs.DeleteAll();
	}
}
