using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headTrigger : MonoBehaviour {
    private Animator anim;
    private bool isPose;


    // Use this for initialization
    void Start () {
        anim = transform.GetComponentInParent<Animator>();
        isPose = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!isPose)
        {
            isPose = true;
            anim.SetInteger("Pose", 7);
        }     
    }

    IEnumerator OnTriggerExit(Collider other)
    {
        yield return new WaitForSeconds(3);
        isPose = false;
    }
}
