using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravController : MonoBehaviour
{

    private bool IsDisablingGrav;
    private float GravDisableRegenPercentSec = 20f; // Regeneration per second when grounded
    private float GravDisableDrainPercentSec = 20f; // Drain per second when being used
    private float GravDisableRegenBuffer = 1.5f; // Time delay between landing and regenerating percentage

    private float GravDisableForce = 1f; // How much of gravity to disable (0 = none, 1 = all of it)
    private float GravDisablePercentMax = 100f; // Maximum percent of mass to remove

    private float GravDisablePercent = 100f;
    private bool IsGrounded;
    private float TimeOnGround = 0f;
    private float TimeNotGrav = 0f;
    
    [SerializeField] private bool ToggleGravDisable = true;

    private MovementController movementController;
    [SerializeField] private PowerTextController powerController;


    // Update is called once per frame

    void Start() {
        movementController = gameObject.GetComponent<MovementController>();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F)){

            if (GravDisablePercent <= 0.1f) {
                IsDisablingGrav = false;
            }

            else if (ToggleGravDisable) {
                IsDisablingGrav = !IsDisablingGrav;
            } 

            else {
                IsDisablingGrav = true;
            }

            TimeNotGrav = 0f;

        } else {
            if ((!Input.GetKey(KeyCode.F)) && !ToggleGravDisable) {
                IsDisablingGrav = false;
            }
            TimeNotGrav += Time.deltaTime;
        }

        powerController.SetEnergy(Mathf.RoundToInt(GravDisablePercent));
    }

    void FixedUpdate() {
        
        IsGrounded = MovementController.IsGrounded;

        if (IsGrounded) {
            TimeOnGround += Time.deltaTime;

            if ((TimeNotGrav > GravDisableRegenBuffer) && GravDisablePercent < GravDisablePercentMax && !IsDisablingGrav) {
                GravDisablePercent += GravDisableRegenPercentSec * (Time.deltaTime); // 20% / second
            }
            if (GravDisablePercent > GravDisablePercentMax) {
                GravDisablePercent = GravDisablePercentMax;
            }
        } else {
            TimeOnGround = 0f;
        }

        if (IsDisablingGrav) {
            Physics.gravity = new Vector3(0f, GravDisableForce / -9.81f, 0f);
            GravDisablePercent -= Time.deltaTime * GravDisableDrainPercentSec;

            if (GravDisablePercent <= 0.1f) {
                IsDisablingGrav = false;
            }

        } else {
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
        }
    }

}
