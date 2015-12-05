using UnityEngine;
using System.Collections;

public class NavigateController : MonoBehaviour {

	public void NewGame() {
		Application.LoadLevel("MainGame");
	}

	public void MainMenu() {
		Application.LoadLevel("MainMenu");
	}

	public void StatsMenu() {
		Application.LoadLevel("StatsPage");
	}
}
