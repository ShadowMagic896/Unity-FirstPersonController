using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPC_Cont : MonoBehaviour
{
    private float VerticalSensitivity = 2f;
    private float HorizontalSensitivity = 2f;

    private bool InvertXAxis = false;
    private bool InvertYAxis = false;

    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Rigidbody Player;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        

        if (MovementController.DisplayControlScheme == 1) {
            float XIn = Input.GetAxis("Mouse X");
            float YIn = Input.GetAxis("Mouse Y");

            if (InvertXAxis) {
                XIn *= -1;
            }
            if (InvertYAxis) {
                YIn *= -1;
            }

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
}


