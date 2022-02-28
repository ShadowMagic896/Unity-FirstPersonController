using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravController : MonoBehaviour
{

    private bool IsDisablingGrav;
    [SerializeField, Range(0f, 100f)] private float GDRegenPercentSec = 20f; // Regeneration per second when grounded
    [SerializeField, Range(0f, 100f)] private float GDDrainPercentSec = 20f; // Drain per second when being used
    [SerializeField, Range(0f, 10f)] private float GDRegenBuffer = 1.5f; // Time delay between landing and regenerating percentage

    [SerializeField, Range(0f, 2f)] private float GDForce = 1f; // How much of gravity to disable (0 = none, 1 = all of it)
    [SerializeField, Range(0f, 1000f)] private float GDEnergyLevelMax = 100f; // Maximum percent energy

    private float GDEnergyLevel = 100f; // Starting percentage
    private bool IsGrounded;
    private float TimeOnGround = 0f;
    private float TimeNotGrav = 0f;
    
    [SerializeField] private bool ToggleGD = false;

    private MovementController movementController;
    [SerializeField] private PowerTextController powerController;


    // Update is called once per frame

    void Start() {
        movementController = gameObject.GetComponent<MovementController>();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F)){

            if (GDEnergyLevel <= 0.1f) {
                IsDisablingGrav = false;
            }

            else if (ToggleGD) {
                IsDisablingGrav = !IsDisablingGrav;
            } 

            else {
                IsDisablingGrav = true;
            }

            TimeNotGrav = 0f;

        } else {
            if ((!Input.GetKey(KeyCode.F)) && !ToggleGD) {
                IsDisablingGrav = false;
            }
            TimeNotGrav += Time.deltaTime;
        }

        powerController.SetEnergy(Mathf.RoundToInt(GDEnergyLevel));
    }

    void FixedUpdate() {
        
        IsGrounded = MovementController.IsGrounded;

        if (IsGrounded) {
            TimeOnGround += Time.deltaTime;

            if ((TimeNotGrav > GDRegenBuffer) && GDEnergyLevel < GDEnergyLevelMax && !IsDisablingGrav) {
                GDEnergyLevel += GDRegenPercentSec * (Time.deltaTime); // 20% / second
            }
            if (GDEnergyLevel > GDEnergyLevelMax) {
                GDEnergyLevel = GDEnergyLevelMax;
            }
        } else {
            TimeOnGround = 0f;
        }

        if (IsDisablingGrav) {
            Physics.gravity = new Vector3(0f, -9.81f * (1 - GDForce), 0f);
            GDEnergyLevel -= Time.deltaTime * GDDrainPercentSec;

            if (GDEnergyLevel <= 0.1f) {
                IsDisablingGrav = false;
            }

        } else {
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
        }
    }

}
