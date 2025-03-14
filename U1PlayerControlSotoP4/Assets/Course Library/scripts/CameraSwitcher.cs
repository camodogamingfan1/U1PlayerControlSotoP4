using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera Cam1;
    public Camera Cam2;
    public KeyCode switchKey = KeyCode.C; // Change this to any key you prefer

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial state of the cameras
        Cam1.enabled = true;
        Cam2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Switch cameras when the key is pressed
        if (Input.GetKeyDown(switchKey))
        {
            Cam1.enabled = !Cam1.enabled;
            Cam2.enabled = !Cam2.enabled;
        }
    }
}

