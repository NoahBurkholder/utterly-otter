using UnityEngine;
using System.Collections;

public class OldWave : MonoBehaviour {

	public bool isWaveZero, hasTask;
	public OldWave parent;
	float amplitude, frequency, timeline;
	float waveHeight;

	// Wave Zero
	public OldWave() {
		isWaveZero = true;
	}

	// Procedural Waves
	public OldWave(OldWave parent) {

	}
	// Update is called once per frame
	public void wavePass (float d) {
		
		if (hasTask) { // If the wave has a task.
			doTask (); // Do the task.
		} else { // Otherwise...
			if (!isWaveZero) { // Assuming this wave isn't wave zero.
				if (parent.hasTask) { // Access parent's current task.
					if (parent.timeline > 0) { // If parent has begun task...
						// Copy parent's task.
						amplitude = parent.amplitude;
						frequency = parent.frequency;
						timeline = parent.timeline;
						frequency = parent.frequency;

						// But lag behind.
						timeline -= Mathf.Sqrt(d);

						// Wave is now employed.
						hasTask = true;
					}
				}
			}
		}
		if ((timeline * frequency) >= (Mathf.PI * 2)) {
			
			hasTask = false;
		}
	}
	void doTask() {
		waveHeight = (Mathf.Sin (timeline * frequency)) * amplitude;
		timeline += 0.1f + frequency;
	}

	public void newTask(float a, float f) {
		amplitude = a;
		frequency = f;
		timeline = 0;
		hasTask = true;
	}
	public float getWaveHeight() {
		return waveHeight;
	}
}
