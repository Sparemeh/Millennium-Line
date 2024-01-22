using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 camera_position;
    [Header("Camera settings")]
    public float camera_speed;

    // Start is called before the first frame update
    void Start()
    {
        camera_position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            camera_position.x -= camera_speed / 10;
        }
        if (Input.GetKey(KeyCode.D))
        {
            camera_position.x += camera_speed / 10;
        }

        this.transform.position = camera_position;
    }
}
