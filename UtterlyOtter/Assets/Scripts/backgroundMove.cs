using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMove : MonoBehaviour
{
    private GameObject whale;
    public bool turn;

    // Use this for initialization
    void Start()
    {
        whale = GameObject.Find("whale");
        turn = false;
    }

    // Update is called once per frame
    void Update()
    {
        float x = whale.transform.position.x;
        float yRot = whale.transform.rotation.y;
        if (whale.transform.position.x < 80 && !turn && whale.transform.position.x > 24.95)
        {
            whale.transform.position = new Vector3((float)(x + 0.05), whale.transform.position.y, whale.transform.position.z);
        }
        else if (whale.transform.position.x > 14.94 && !turn && whale.transform.position.x < 24.95)
        {
            whale.transform.Rotate(new Vector3(whale.transform.rotation.x, (float) 0.85, whale.transform.rotation.z));
            whale.transform.position = new Vector3((float)(x + 0.05), whale.transform.position.y, whale.transform.position.z);
        }

        if (whale.transform.position.x >= 79 && !turn)
        {
            turn = true;
        }

        if (turn && whale.transform.position.x > 15)
         {
                whale.transform.Rotate(new Vector3(whale.transform.rotation.x, (float)(whale.transform.rotation.y + 0.16), whale.transform.rotation.z));
                whale.transform.position = new Vector3((float)(x - 0.05), whale.transform.position.y, whale.transform.position.z);
            }

        if (whale.transform.position.x <= 15 && turn)
        {
            turn = false;
        }

    }
}
