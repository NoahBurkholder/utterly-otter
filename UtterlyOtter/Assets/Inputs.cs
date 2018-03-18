using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Inputs : MonoBehaviour {

	private Color sWhite = new Color (1f, 1f, 1f, 1f);
	private Color tWhite = new Color (1f, 1f, 1f, 0.5f);


	private RawImage keyZ, keyX, keyC, keyA, keyS, keyD;
	private Sprite imgZ, imgX, imgC, imgA, imgS, imgD;

	void Start () {
		keyZ = transform.FindChild("KeyZ").GetComponent<RawImage> ();
		keyX = transform.FindChild("KeyX").GetComponent<RawImage> ();
		keyC = transform.FindChild("KeyC").GetComponent<RawImage> ();
		keyA = transform.FindChild("KeyA").GetComponent<RawImage> ();
		keyS = transform.FindChild("KeyS").GetComponent<RawImage> ();
		keyD = transform.FindChild("KeyD").GetComponent<RawImage> ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Pose1")) {
			keyZ.color = sWhite;
		} else {
			keyZ.color = tWhite;
		}
		if (Input.GetButton ("Pose2")) {
			keyX.color = sWhite;
		} else {
			keyX.color = tWhite;
		}
		if (Input.GetButton ("Pose3")) {
			keyC.color = sWhite;
		} else {
			keyC.color = tWhite;
		}
		if (Input.GetButton ("Pose4")) {
			keyA.color = sWhite;
		} else {
			keyA.color = tWhite;
		}
		if (Input.GetButton ("Pose5")) {
			keyS.color = sWhite;
		} else {
			keyS.color = tWhite;
		}
		if (Input.GetButton ("Pose6")) {
			keyD.color = sWhite;
		} else {
			keyD.color = tWhite;
		}

		if (Input.GetButton ("Escape")) {
			SceneManager.LoadScene ("Menu");
		}
	
	}
}
