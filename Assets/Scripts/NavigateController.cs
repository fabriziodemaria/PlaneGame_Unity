using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateController : MonoBehaviour {

	public void NewGame() {
		SceneManager.LoadScene("MainGame");
	}

	public void MainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void StatsMenu() {
		SceneManager.LoadScene("StatsPage");
	}
}
