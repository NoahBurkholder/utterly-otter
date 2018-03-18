
using UnityEngine;
using System.Collections;

// This class can be thought of as a data structure for holding the water's sine wave data.

public class Wave {

	public float amplitude, frequency, timeline, lag; // For sine wave.
	float waveHeight; // Return value.
	public int index; // Index along the sine wave line.

	// Wave Zero
	public Wave() {
		loop = 0;
	}
	float loop;
	public void drawWave() {
		// Make a loop from TWO_PI to 0.
		if (loop > 0+lag) {
			// Make the loop time downwards based on time passed per frame * a frequency variable.
			loop -= Time.deltaTime*frequency;
		} else {
			// Reset to TWO_PI.
			loop = (Mathf.PI*2)+lag;
		}

		// This is the return value based on a sine-wave.
		waveHeight = (Mathf.Sin (loop)) * amplitude;
	}


	public float getWaveHeight() {
		return waveHeight;
	}
}