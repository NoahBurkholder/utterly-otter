using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private GameLogic gameLogic;

	private GameObject camObject;
	private Camera camScript;

	private Water waterScript;

	private Animator anim;
	private Rigidbody rb;

	private float cameraZoom, cameraTarget;

	private Vector3 targetPos, currentPos;

	private Vector3 gravityForce, buoyancyForce;

	// Use this for initialization
	void Start () {
		gameLogic = GameObject.Find ("/Environment").GetComponent<GameLogic> (); // Get environment.
		waterScript = GameObject.Find ("/Environment/Water").GetComponent<Water> (); // For getting surface height (taking into account waves.)

		rb = GetComponent<Rigidbody> ();
		anim = transform.FindChild ("Otter").GetComponent<Animator> ();
		position = transform.position; // Initialize otter's position.

		currentPos = transform.position; // Position otter is currently looking at.

		// Camera stuff.
		camObject = GameObject.Find ("Player Camera");
		camScript = camObject.GetComponent<Camera> ();
		cameraTarget = -3.5f;
		//InvokeRepeating("tick", 0f, (1f/gameLogic.tickrate)); // Call 'tick' method 60 (check in gameLogic) times a second. This replaces update.

		gravityForce = new Vector3 (0, -5f, 0);
		buoyancyForce = new Vector3 (0, 0.5f, 0);

	}

	// Update is called once per frame
	private void FixedUpdate () {
		inputs ();
		checkConditions ();
		move ();
		checkForReset ();
		handleCamera ();
	}

	private bool isBreached;
	private float surface;
	private void checkConditions() {
		float distance = transform.position.z - Camera.main.transform.position.z;
		targetPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
		targetPos = Camera.main.ScreenToWorldPoint(targetPos);

		currentPos += (targetPos - currentPos) * 0.1f;
		currentPos = targetPos;


		//transform.LookAt (currentPos);
	}


	private bool leftMouse, rightMouse;
	private bool isPosing;
	private int pose;
	private void inputs() {
		isPosing = false;
		pose = 0;
		if (Input.GetMouseButton (0)) {
			leftMouse = true;
		} else {
			leftMouse = false;
		}
		if (Input.GetButton("Pose1")) {
			isPosing = true;
			pose = 1;
		}
		if (Input.GetButton("Pose2")) {
			isPosing = true;
			pose = 2;

		}
		if (Input.GetButton("Pose3")) {
			isPosing = true;
			pose = 3;
		}
		if (Input.GetButton("Pose4")) {
			isPosing = true;
			pose = 4;
		}
		if (Input.GetButton("Pose5")) {
			isPosing = true;
			pose = 5;
		}
		if (Input.GetButton("Pose6")) {
			isPosing = true;
			pose = 6;
		}
	}


	public int totalScore;
	private float orientStrength;
	Vector3 position, velocity, acceleration;
	bool isFlying, isGrabbed;
	int score;
	float deltaAngle;

	private void move() {

		anim.SetInteger ("Pose", pose);



		if (transform.position.y < surface) { // While underwater.
			if (isFlying) {
				totalScore += score;
				gameLogic.setScore (totalScore.ToString());
				gameLogic.setIncomingScore ("", 0);
				score = 0;
				isFlying = false;
			}
			anim.SetBool ("Underwater", true);
			//rb.AddForce(buoyancyForce);
			rb.drag = 1f;
			rb.angularDrag = 3f;
			orientStrength = 2.0f;
			orient ();
			swim();
		} else { // While in air.
			if (!isGrabbed) {
				int scoreIncrement = 0;
				float heightFactor = (transform.position.y / 2f);
				float raw = deltaAngle + heightFactor;
				if (isPosing) {
					raw = raw * raw;
				}

				score += (int)raw;
				gameLogic.setIncomingScore (score.ToString (), raw);
			} else {
				gameLogic.setIncomingScore ("", 0);
			}

			isFlying = true;
			anim.SetBool ("Underwater", false);
			rb.AddForce(gravityForce);
			rb.drag = 0.1f;
			rb.angularDrag = 0.1f;
			orientStrength = 0.5f;
			orient ();

		}
			
	}

	private float swimForce;
	private void swim () {
		
		if ((targetPos - transform.position).magnitude > 1f) {
			if (!isPosing) {
				swimForce = ((targetPos - transform.position).magnitude) * ((targetPos - transform.position).magnitude);
				if (swimForce > 16.0f) {
					swimForce = 16.0f;
				}
				rb.AddRelativeForce(new Vector3(swimForce, 0, 0));

				anim.SetFloat ("Magnitude", swimForce / 4f);

			}
		} else {
			anim.SetFloat ("Magnitude", 0);
		}
	}

	//private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
	//private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);
	private readonly VectorPid angularVelocityController = new VectorPid(0.5f, 0, 0.01f);
	private readonly VectorPid headingController = new VectorPid(2f, 0, 0.01f);
	//public Transform target;
	private void orient () {
		//Debug.Log("XQ = " + transform.localRotation.x + ", ZQ = " + transform.localRotation.z);

		if (!isPosing) {
			if (!isGrabbed) {
				// Point otter's back towards the sky when possible.
				if (transform.localRotation.x + transform.localRotation.z < -0.1f) {
					rb.AddRelativeTorque (2f, 0, 0);
			
				} else if (transform.localRotation.x + transform.localRotation.z > 0.1f) {
					rb.AddRelativeTorque (-2f, 0, 0);
				}

				// Quaternion orientation code.
				Vector3 angularVelocityError = rb.angularVelocity * -1f;
				Debug.DrawRay (transform.position, rb.angularVelocity * 10, Color.black);

				Vector3 angularVelocityCorrection = angularVelocityController.Update (angularVelocityError, Time.deltaTime);
				Debug.DrawRay (transform.position, angularVelocityCorrection, Color.green);

				rb.AddTorque (angularVelocityCorrection);
				deltaAngle = (angularVelocityCorrection.magnitude);

				Vector3 desiredHeading = targetPos - transform.position;
				Debug.DrawRay (transform.position, desiredHeading, Color.magenta);

				Vector3 currentHeading = transform.right;
				Debug.DrawRay (transform.position, currentHeading * 15, Color.blue);

				Vector3 headingError = Vector3.Cross (currentHeading, desiredHeading);
				Vector3 headingCorrection = headingController.Update (headingError, Time.deltaTime);

				rb.AddTorque (headingCorrection);
			}
		}
	}


	private void checkForReset() {
		if (Input.GetButtonDown ("Reset")) {
			//gameLogic.reset ();
		}
	}
	private void handleCamera() {
		if (Input.GetAxis ("Scroll") != 0) {
			cameraTarget *= 1f - Input.GetAxis ("Scroll");
		}
		if (cameraTarget > -1f) {
			cameraTarget = -1f;
		} else if (cameraTarget < -10f) {
			cameraTarget = -10f;
		}


		cameraZoom += ((cameraTarget) - (cameraZoom)) * 0.1f;


		camObject.transform.localPosition = new Vector3(transform.position.x, 1f + transform.position.y + -cameraZoom * 0.1f, cameraZoom);
		camScript.fieldOfView = GameLogic.map (cameraZoom, -100f, -2f, 40, 80);
		//camObject.transform.localPosition.z = cameraZoom;
	}

	public void setGrabbed (bool tf) {
		isGrabbed = tf;
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
