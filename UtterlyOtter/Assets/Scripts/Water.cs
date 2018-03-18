using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {


	private GameObject parent;
	public GameLogic gameLogic;
	private Environment environment;
	// Mesh
	MeshFilter meshFilter;
	Mesh mesh;

	public GameObject[] waves;
	public Wave[] waveScripts;
	public Object wavePrefab;
    private float[] waveHeights;
	private float[] groundHeights;

	private Vector3[] vertices;
	private Vector3[] normals;
	private int[] trisFront, trisTop;
	private Vector2[] uvs, uvs2;

	public float baseHeight;
	public float width, depth;
	public int points;

	public int diagonalLength;
	public int widthPoints, depthPoints;
	public float widthStep, depthStep;


	public Renderer rn;
	public Material materialFront, materialTop;


    // Use this for initialization
	void Start () {

		parent = transform.parent.gameObject;

		// Get world objects.
		gameLogic = parent.GetComponent<GameLogic> (); 
		environment = parent.GetComponent<Environment> (); 

		rn = GetComponent<Renderer>();
		rn.materials = new Material[] {materialFront, materialTop};

		// Get the height of the water.
		baseHeight = environment.worldHeight;

		// Get width and depth of water.
		width = environment.worldWidth;
		depth = environment.worldDepth;

		// In case the amount of vertices ever differ from the world width/depth.
		widthPoints = environment.worldWidth;
		depthPoints = environment.worldDepth;

		// Get diagonal length of entire body of water (+1 for safe margin.)
		diagonalLength = 1 + (int)(Mathf.Sqrt((widthPoints * widthPoints) + (depthPoints * depthPoints)));

		// Wave
		waves = new GameObject[widthPoints + depthPoints];
		waveScripts = new Wave[widthPoints + depthPoints];

		// Create diagonal wave from left-front side to right-back side.
		for (int d = 0; d < diagonalLength; d++) {
			waveScripts [d] = new Wave ();
			waveScripts [d].index = d;
			waveScripts [d].frequency = 1f;
			waveScripts [d].amplitude = environment.amplitude;
			waveScripts [d].lag = ((float)waveScripts [d].index)/1.5f;

			//Debug.Log ("I am a wave with index: " + waveScripts[d].index + ", with a lag of: " + waveScripts[d].lag);

		}

		//InvokeRepeating("tick", 0f, (1f/gameLogic.tickrate)); // Call 'tick' method 60 (check in gameLogic) times a second. This replaces update.

		// Initialize bottom bounds of water.
		groundHeights = new float[widthPoints];

		// Mesh
		initializeMesh ();
	}

	// Complete standin for update function. Time-based.
	void FixedUpdate() {
		// Iterate along diagonal wave.
		for (int d = 0; d < diagonalLength; d++) {
			try {
				waveScripts [d].drawWave();
			} catch (UnityException e) {
				Debug.LogException (e);
			}
		}
		moveVertices ();
	}
		
	void initializeMesh () {
		
		meshFilter = gameObject.AddComponent<MeshFilter>();
		mesh = new Mesh();
		mesh.subMeshCount = 2; // Top and front meshes.
		meshFilter.mesh = mesh;
		mesh.name = "CombinedWaterMesh";


		vertices = new Vector3[(widthPoints * depthPoints)+widthPoints];
		Debug.Log ("vertices: " + vertices.Length);


		trisFront = new int[(widthPoints - 1) * 6];
		trisTop = new int[((widthPoints-1) * (depthPoints-1)) * 12];
		Debug.Log ("front tris: " + trisFront.Length);
		Debug.Log ("top tris: " + trisTop.Length);

		normals = new Vector3[vertices.Length];
		Debug.Log ("normals: " + normals.Length);


		uvs = new Vector2[vertices.Length];
		uvs2 = new Vector2[vertices.Length];
		Debug.Log ("uvs: " + uvs.Length);
		Debug.Log("Points: " + points + ", TrisFront: " + trisFront.Length + ", Vertices: " + vertices.Length);


		widthStep = width / widthPoints;
		depthStep = depth / depthPoints;


		int index = 0;
		// Set downwards extent of the water.
		for (int f = 0; f < widthPoints; f++) {
			float widthOffset = f * widthStep;
			vertices [(vertices.Length - widthPoints) + f] = new Vector3(widthOffset, groundHeights[f]-baseHeight, 0f);
			//Debug.Log((vertices.Length - widthPoints) + f + " = " + vertices[(vertices.Length - widthPoints) + f].ToString());

		}
		// Set the Upper extents.
		for (int i = 0; i < depthPoints; i++) {
			for (int j = 0; j < widthPoints; j++) {
				float widthOffset = j * widthStep;
				float depthOffset = i * depthStep;
				vertices [index] = new Vector3(widthOffset, 0f, depthOffset);
				//Debug.Log(index + " = " + vertices[index].ToString());

				uvs[index] = new Vector2(((float)j / (float)widthPoints), ((float)i / (float)depthPoints));
				//Debug.Log("UV ("+ j + ", " + i + ") = " + uvs[index].ToString());
				index ++;
			}
		}
	
		moveVertices ();
		makeTriangles ();
		mesh.uv = uvs;
		//mesh.RecalculateNormals();
	}



	/* This function iterates all the vertices on the surface of the water.
	 * 
	 * 
	 *
	 */
	void moveVertices() {
		// Match vertexes to dynamic wave heightmap.
		int index = 0;
		float widthOffset = 0f;
		float depthOffset = 0f;
		for (int i = 0; i < depthPoints; i++) {
			for (int j = 0; j < widthPoints; j++) {
				int d = (int)Mathf.Sqrt((j*j)+(i*i));
				try {
					vertices [index] = new Vector3(j * widthStep, waveScripts[d].getWaveHeight(), i * depthStep);
					uvs2[index] = new Vector2((1f/(float)j), (1f/(float)i));

					Debug.DrawLine(vertices[index], vertices[index] + new Vector3(0, 0.1f, 0), Color.black);

				} catch {
					Debug.LogError ("Index " + j + ", " + i + " threw an error.");

				}
				index ++;
			}
		}

		mesh.vertices = vertices;
		//mesh.RecalculateNormals();

	}


	// This method defines the polygons of the water.
	void makeTriangles() {
		
		// Create calm sea vertex locations.
		int index = 0;
		float widthOffset = 0f;
		float depthOffset = 0f;

		// Set cross-section triangles. (Front face.)
		for (int f = 0; f < widthPoints-1; f++) {

			int v1 = f; // Closer n.
			int v2 = v1 + 1; // Closer n+1.
			int v3 = (vertices.Length - widthPoints) + f; // Further n.
			int v4 = v3 + 1; // Further n+1.

			trisFront [3 * f] = v4; // Surface n.
			trisFront [3*f + 1] = v3; // Bottom n.
			trisFront [3*f + 2] = v1; // Bottom n+1.

			trisFront [(3 * f) + (trisFront.Length / 2)] = v1; // Surface n.
			trisFront [(3*f + 1) + (trisFront.Length / 2)] = v2; // Surface n+1.
			trisFront [(3*f + 2) + (trisFront.Length / 2)] = v4; // Bottom n+1.
		}

		// Set surface triangles.
		for (int i = 0; i < depthPoints-1; i++) {
			for (int j = 0; j < widthPoints-1; j++) {

				int v1 =((j) + ((i) * widthPoints));; // Closer n.
				int v2 = v1 + 1; // Closer n+1.
				int v3 = ((j) + ((i+1) * widthPoints)); // Further n.
				int v4 = v3 + 1; // Further n+1.

				int perVertex = 12;


				// Over face.
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))] = v3;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+1] = v2;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+2] = v1;

				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+3] = v4;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+4] = v2;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+5] = v3;


				// Under face.
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+6] = v4;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+7] = v3;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+8] = v1;

				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+9] = v1;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+10] = v2;
				trisTop [((perVertex*j)+((widthPoints-1) * i * perVertex))+11] = v4;

				//trisTop [((depthPoints*i)+(3*j)) + (trisTop.Length / 2)] = v1;
				//trisTop [((depthPoints*i)+(3*j) + 1) + (trisTop.Length / 2)] = v2;
				//trisTop [((depthPoints*i)+(3*j) + 2) + (trisTop.Length / 2)] = v4;
			}
		}
		mesh.SetTriangles (trisFront, 0);
		mesh.SetTriangles (trisTop, 1);
		//mesh.normals = normals;

	}

	public bool isSubmerged (Vector3 v) {
		int i = Mathf.FloorToInt(v.x);
		if (i >= 0) {
			if (i < vertices.Length) {
				if (v.y < vertices [i].y) {
					return true;
				}
			}
		}
		return false;
	}

	public float amountSubmerged (Vector3 v) {
		int i = Mathf.FloorToInt(v.x);
			float amount = vertices [i].y - v.y;
			return amount;
	}

}

