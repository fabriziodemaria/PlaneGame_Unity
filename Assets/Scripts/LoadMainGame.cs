using UnityEngine;
using System.Collections;

public class LoadMainGame : MonoBehaviour {

	public void NewGame() {
		Debug.Log("Button was pressed");
		Application.LoadLevel("MainGame");
	}
}
