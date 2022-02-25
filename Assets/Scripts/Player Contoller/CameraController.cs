using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float VerticalSensitivity = 2f;
    private float HorizontalSensitivity = 2f;

    [SerializeField] Camera PlayerCamera;
    Rigidbody Player;

    void Start()
    {
        Player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float XIn = Input.GetAxis("Mouse X");
        float YIn = Input.GetAxis("Mouse Y");

        PlayerCamera.transform.Rotate(
            -YIn * VerticalSensitivity,
            0f,
            0f
        );
        Player.transform.Rotate(
            0f,
            XIn*HorizontalSensitivity,
            0f
        );

    }

}


