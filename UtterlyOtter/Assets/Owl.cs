using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour {

	private Water waterScript;

	private Animator anim;
	private Rigidbody rb;
	public Collider talons;
	private ConfigurableJoint grabJoint;

	private Vector3 nextLevel;
	private Vector3 idlePos;
	private GameObject otter;
	private Player otterScript;
	private Vector3 otterPos;
	private Vector3 currentPos;

	public bool isGrabbing;

	// Use this for initialization
	void Start () {
		waterScript = GameObject.Find ("/Environment/Water").GetComponent<Water> (); // For getting surface height (taking into account waves.)

		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		talons = transform.FindChild ("Head/Chest/Tail/TailEnd").GetComponent<Collider> ();
		grabJoint = GetComponent<ConfigurableJoint> ();

		nextLevel = new Vector3 (20f, 30f, 3f);
		idlePos = new Vector3 (20f, 5f, 3f);

		otter = GameObject.Find ("Player");
		otterScript = otter.GetComponent<Player> ();
		otterPos = otter.transform.position;
		rb.drag = 1f;

	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag.Equals("Player")) {
			if (!isGrabbing) {
				isGrabbing = true;
				otterScript.setGrabbed (true);
				grabJoint.connectedBody = c.attachedRigidbody;
				grabJoint.xMotion = ConfigurableJointMotion.Limited;
				grabJoint.yMotion = ConfigurableJointMotion.Limited;
				grabJoint.zMotion = ConfigurableJointMotion.Limited;
				//grabJoint.breakForce = 100f;

			}
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		orient ();
		move ();
	}

	void move() {
		otterPos = otter.transform.position;
		if (transform.position.y < 5f) {
			rb.AddForce (new Vector3(0, 20f - (4 * transform.position.y), 0));
		}
		if (waterScript.isSubmerged (otterPos)) {
			currentPos = idlePos;
		}

		if (isGrabbing) {
			currentPos = nextLevel;
		} else {
			currentPos = otterPos;
		}
		Vector3 vel = (currentPos - transform.position);
		float strength = 10f;
		float biggest = Mathf.Abs(vel.x);
		if (Mathf.Abs(vel.y) > Mathf.Abs(vel.x)) {
			biggest = Mathf.Abs(vel.y);
		}
		vel = vel / biggest;
		rb.AddForce (vel * strength);
		Debug.DrawRay (transform.position, vel);
	}

	private readonly VectorPid angularVelocityController = new VectorPid(2f, 0, 0.01f);
	private readonly VectorPid headingController = new VectorPid(4f, 0, 0.01f);
	//public Transform target;
	private void orient () {

		// Quaternion orientation code.
		Vector3 angularVelocityError = rb.angularVelocity * -1f;
		Debug.DrawRay (transform.position, rb.angularVelocity * 10, Color.black);

		Vector3 angularVelocityCorrection = angularVelocityController.Update (angularVelocityError, Time.deltaTime);
		Debug.DrawRay (transform.position, angularVelocityCorrection, Color.green);

		rb.AddTorque (angularVelocityCorrection);

		Vector3 desiredHeading = Vector3.up;
		Debug.DrawRay (transform.position, desiredHeading, Color.magenta);

		Vector3 currentHeading = transform.up;
		Debug.DrawRay (transform.position, currentHeading * 15, Color.blue);

		Vector3 headingError = Vector3.Cross (currentHeading, desiredHeading);
		Vector3 headingCorrection = headingController.Update (headingError, Time.deltaTime);

		rb.AddTorque (headingCorrection);

	}
	private class VectorPid
	{
		public float pFactor, iFactor, dFactor;

		private Vector3 integral;
		private Vector3 lastError;

		public VectorPid(float pFactor, float iFactor, float dFactor)
		{
			this.pFactor = pFactor;
			this.iFactor = iFactor;
			this.dFactor = dFactor;
		}

		public Vector3 Update(Vector3 currentError, float timeFrame)
		{
			integral += currentError * timeFrame;
			var deriv = (currentError - lastError) / timeFrame;
			lastError = currentError;
			return currentError * pFactor
				+ integral * iFactor
				+ deriv * dFactor;
		}
	}
}
