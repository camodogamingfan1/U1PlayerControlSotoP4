using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerX : MonoBehaviour
{
    public GameObject plane; // Make sure this is assigned in the Inspector
    public float sideOffset = 5f; // Side distance
    public float heightOffset = 3f; // Camera height
    public float backOffset = 10f; // Distance behind the plane

    private Vector3 offset;

    void Start()
    {
        if (plane == null)
        {
            Debug.LogError("FollowPlayerX: Plane object is not assigned!");
            return;
        }

        // Offset includes side, height, and behind distance
        offset = new Vector3(sideOffset, heightOffset, -backOffset);
    }

    void LateUpdate()
    {
        if (plane != null)
        {
            transform.position = plane.transform.position + offset;
            transform.LookAt(plane.transform); // Makes camera face the plane
        }
    }
}

