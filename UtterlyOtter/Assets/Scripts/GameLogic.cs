using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public float tickrate;

	public bool debugWater;


	GameObject owl;
	Owl owlScript;

	GameObject otter;
	Player otterScript;
	Image panel;
	Text score;
	Text incomingScore;


	void Start() {

		panel = GameObject.Find ("Canvas/Fade").GetComponent<Image>();
		score = GameObject.Find ("Canvas/Score").GetComponent<Text> ();
		incomingScore = GameObject.Find ("Canvas/IncomingScore").GetComponent<Text> ();

		owl = GameObject.Find ("Owl");
		owlScript = owl.GetComponent<Owl> ();

		otter = GameObject.Find ("Player");
		otterScript = otter.GetComponent<Player> ();


	}

	public void setIncomingScore(string s, float x) {
		incomingScore.text = s;
		float dx = x * 0.5f;
		incomingScore.fontSize = 16 + (int)dx;
	}

	public void setScore(string s) {
		score.text = s;
	}

	void FixedUpdate() {
		if (owlScript.isGrabbing) {
			if (otter.transform.position.y > 10) {
				panel.color = new Color (1f, 1f, 1f, (otter.transform.position.y - 10f) / 10f);
				if (otter.transform.position.y > 20f) {
					if (SceneManager.GetActiveScene().name.Equals("Level1")) {
						SceneManager.LoadScene("Level2");
					}
					if (SceneManager.GetActiveScene().name.Equals("Level2")) {
						SceneManager.LoadScene("Level1");
					}

				}
			} else {

			}
		}
	}


	public static float map(float s, float a1, float a2, float b1, float b2)
	{
		return b1 + (s-a1)*(b2-b1)/(a2-a1);
	}


}
