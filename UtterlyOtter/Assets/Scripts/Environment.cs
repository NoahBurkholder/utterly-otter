using UnityEngine;
using System.Collections;

public class Environment : MonoBehaviour {

//	[SerializeField]
//	[Range(3, 1000)]
//	public int worldSize;

//	[SerializeField]
//	[Range(3, 1000)]
//	public int worldPoints;


	[SerializeField]
	[Range(5, 100)]
	public int worldWidth;

	[SerializeField]
	[Range(5, 100)]
	public int worldHeight;

	[SerializeField]
	[Range(3, 200)]
	public int worldDepth;

	[SerializeField]
	[Range(0.0f, 1.0f)]
	public float amplitude;

}
