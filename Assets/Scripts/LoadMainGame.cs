using UnityEngine;
using System.Collections;

public class LoadMainGame : MonoBehaviour {

	public void NewGame() {
		Application.LoadLevel("MainGame");
	}
}
