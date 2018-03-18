using UnityEngine;
using System.Collections;

public class Buoyancy : MonoBehaviour {

	private Water waterScript;
	private int numBuoys;
	private Rigidbody rb;
	float mass;
	private Vector3 gravityForce, buoyancyForce;
	private GameObject[] buoys;
	// Use this for initialization
	void Start () {
		waterScript = GameObject.Find ("/Environment/Water").GetComponent<Water> (); // For getting surface height (taking into account waves.)

		rb = GetComponent<Rigidbody> ();
		mass = rb.mass;
		gravityForce = new Vector3 (0, -5f, 0);
		buoyancyForce = new Vector3 (0, 3f, 0);
		foreach (Transform child in transform) {
			if (child.gameObject.tag == "Buoy") {
				numBuoys++;
			}
		}
		buoys = new GameObject[numBuoys];

		int i = 0;
		foreach (Transform child in transform) {
			if (child.gameObject.tag == "Buoy") {
				GameObject g = child.gameObject;
			
				buoys [i] = g;
				i++;
			}
		}
	}

	void FixedUpdate() {
		move ();
	}

	float surface;
	private void move() {
		float d = 0;
		float ad = 0;
		for (int i = 0; i < buoys.Length; i++) {
			//rb.AddForceAtPosition (new Vector3(0, 10000f, 0) / (float)numBuoys, buoys[i].transform.position);
			float a = waterScript.amountSubmerged(buoys[i].transform.position);
			//a *= a;
			//gravityForce = new Vector3 (0, -5f * a, 0);
			//buoyancyForce = new Vector3 (0, 3f * a, 0);
			if (waterScript.isSubmerged (buoys[i].transform.position)) { // While underwater.
				
				rb.AddForceAtPosition (((Vector3.up * a * mass / numBuoys) + Vector3.up * 3 * mass), buoys[i].transform.position);
				Debug.DrawRay (buoys [i].transform.position, new Vector3(0, a, 0), Color.red);
				//Debug.Log (buoys [i].transform.position);
				d += 1f;
				ad += 1f;
			
			} else { // While in air.
				//rb.AddForceAtPosition (new Vector3(0, -100f, 0), buoys[i].transform.position);
				Debug.DrawRay (buoys [i].transform.position, new Vector3(0, a, 0), Color.green);

			}
			rb.drag = d;
			rb.angularDrag = ad;
		}
	}
}
