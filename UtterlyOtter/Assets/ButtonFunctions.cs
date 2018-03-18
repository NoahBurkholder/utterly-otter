using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonFunctions : MonoBehaviour {

	public void Play () {
		SceneManager.LoadScene ("Level1");
	}

	public void Video () {
		SceneManager.LoadScene ("Video");
	}

	public void Exit () {
		Application.Quit ();
	}
}
