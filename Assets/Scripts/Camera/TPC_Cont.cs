using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPC_Cont : MonoBehaviour {
    private float VerticalSensitivity = 2f;
    private float HorizontalSensitivity = 2f;

    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private Rigidbody Player;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (MovementController.DisplayControlScheme == 2) {
            float XIn = Input.GetAxis("Mouse X");
            float YIn = Input.GetAxis("Mouse Y");

            if (Input.GetKey(KeyCode.Mouse1)) {
                PlayerCamera.transform.RotateAround(
                    Player.position,
                    Player.transform.right,
                    YIn * VerticalSensitivity
                );
            }
            if (Input.GetKey(KeyCode.Mouse1)){
                Player.transform.Rotate(
                    0f,
                    XIn * HorizontalSensitivity,
                    0f
                );
            }
        }
    }
}
