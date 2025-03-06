using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 5, -7);
    public float turnspeed;
    public float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.position = player.transform.position + offset;
        transform.Translate(Vector3.right * Time.deltaTime * turnspeed * horizontalInput);
    }
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
