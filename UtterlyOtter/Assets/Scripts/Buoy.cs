using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buoy {

	private GameObject parent;
	private Rigidbody playerRB;
	private float buoyantForce, gravityForce;
	public Buoy(GameObject p, Rigidbody rb, float bf, float gf) {
		parent = p;
		playerRB = rb;
		buoyantForce = bf;
		gravityForce = gf;
	}

	public bool checkSubmerged(float surface) {
		if (parent.transform.position.y < surface) {
			playerRB.AddForceAtPosition (new Vector3 (0, buoyantForce, 0), parent.transform.position);
			return true;
		} else {
			playerRB.AddForceAtPosition (new Vector3 (0, gravityForce, 0), parent.transform.position);

			return false;
		}
	}
}
