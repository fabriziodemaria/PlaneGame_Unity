using UnityEngine;
using System.Collections;

public class ResetStats : MonoBehaviour {

	public void DeleteAll() {
		PlayerPrefs.DeleteAll();
	}
}
