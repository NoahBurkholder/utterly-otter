using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (AudioSource))]

public class Video : MonoBehaviour {
    public MovieTexture movie;

	// Use this for initialization
	void Start () {
        movie = ((MovieTexture)GetComponent<Renderer>().material.mainTexture);
        movie.Play();

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

    }

    // Update is called once per frame
    private void Update()
    {
        if (!movie.isPlaying)
        {
            StartCoroutine(sceneChange());
        }
    }

    IEnumerator sceneChange()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Menu");
    }
}
