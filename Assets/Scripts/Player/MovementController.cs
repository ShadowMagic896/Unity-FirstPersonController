using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody Player;
    private Camera Camera;

    private bool IsJumping;
    private float JumpForce = 7f;

    private bool IsForward;

    private bool IsDisablingGrav;
    private float GravDisableRegenPerSec = 20f; // Regeneration per second when grounded
    private float GravDisableDrainPerSec = 20f; // Drain per second when being used
    private float GravDisableRegenBuffer = 1.5f; // Time delay between landing and regenerating percentage

    private float GravDisableForce = 0.60f; // How much of gravity to disable (0 = none, 1 = all of it)
    public float GravDisablePerMax = 100f; // Maximum percent of mass to remove

    public float GravDisablePer = 100f;
    private bool IsGrounded;
    private float TimeOnGround = 0f;

    // These are multiplicitive with the base movement speed, so crouching reduces speed by 50%, and sprinting
    // increases speed by twofold. This makes it easier to, say, walk or sprint while crouching
    public bool IsCrouching;
    private float CrouchMultiplier = 0.5f;

    public bool IsWalking;
    private float WalkMultiplier = 0.75f;
    
    public bool IsSprinting;
    private float SprintMultiplier = 2f;

    private Vector3 HorizontalInput;
    private float MovementSpeed = 5f; // Default speed

    private bool ToggleCrouch = false; // Whether to toggle or hold keys for x
    private bool ToggleWalk = false;
    private bool ToggleSprint = false;
    private bool ToggleGravDisable = false;
    private bool OverrideControls = true; // Whether keys can override other ones or not (walking / running)

    private float VeloX, VeloY, VeloZ;

    private bool LVaultPossible;
    private bool HVaultPossible;
    private bool BadVaultPossible;

    private bool Restarting;
    private bool DebugActive;

    public static int DisplayControlScheme = 2;


    [SerializeField] private Transform GroundCheckTransform;
    // [SerializeField] private Transform HigherVaultTransform;
    // [SerializeField] private Transform LowerVaultTransform;
    // [SerializeField] private Transform BadVaultTransform;

    public static float Abs(float x){
        if (x < 0){
            return -x;
        } return x;
    }
    private string StrTimesInt(string str, int times) {
        string end = "";
        for (int i = 0; i < times; i++) {
            end += str;
        }
        return end;
    }
    private void LogInfo(bool[] items, string[] names) {
        string end = "";
        for (int i = 0; i < items.Length; i++) {
            end += (items[i] ? names[i] : "Not-" + names[i]) + ", ";
        }
        Debug.Log(end);
    }


    void Start()
    {
        Player = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
    }


    void Update() {

        // JUMPING
        if (Input.GetKey(KeyCode.Space)){
            IsJumping = true;
        } else {
            IsJumping = false;
        }


        // SPRINTING
        if (Input.GetKey(KeyCode.LeftShift) && (!IsSprinting || OverrideControls)){
            if (ToggleSprint) {
                IsWalking = false;
                IsSprinting = !IsSprinting;
            } else if (!ToggleSprint && (!IsWalking || OverrideControls)) {
                IsWalking = false;
                IsSprinting = true;
            }
        } else if ((!Input.GetKey(KeyCode.LeftShift)) && !ToggleSprint) {
            IsSprinting = false;
        }


        // WALKING
        if (Input.GetKey(KeyCode.LeftAlt) && (!IsWalking || OverrideControls)){
            if (ToggleWalk) {
                IsSprinting = false;
                IsWalking = !IsWalking;
            } else if (!ToggleWalk && (!IsSprinting || OverrideControls)) {
                IsSprinting = false;
                IsWalking = true;
            }
        } else if ((!Input.GetKey(KeyCode.LeftAlt)) && !ToggleWalk) {
            IsWalking = false;
        }
        

        // CROUCHING
        if (Input.GetKey(KeyCode.LeftControl)){
            if (ToggleCrouch) {
                IsCrouching = !IsCrouching;
            } else {
                IsCrouching = true;
            }
        } else if ((!Input.GetKey(KeyCode.LeftControl) && !ToggleCrouch)) {
            IsCrouching = false;
        }


        // GRAVITY
        if (Input.GetKey(KeyCode.F)){
            if (GravDisablePer <= 0.1f) {
                IsDisablingGrav = false;
            }
            else if (ToggleGravDisable) {
                IsDisablingGrav = !ToggleGravDisable;
            } else {
                IsDisablingGrav = true;
            }
        } else if ((!Input.GetKey(KeyCode.F)) && !ToggleGravDisable) {
            IsDisablingGrav = false;
        }


        //RESTARTING
        if (Input.GetKey(KeyCode.F2)) {
            Restarting = true;
        }


        //DEBUGGING
        if (Input.GetKey(KeyCode.F1)) {
            DebugActive = true;
        } else {
            DebugActive = false;
        }


        HorizontalInput = transform.right * Input.GetAxis("Horizontal") * MovementSpeed + transform.forward * Input.GetAxis("Vertical") * MovementSpeed;
    }

    void FixedUpdate() {

        // These test if a sphere placed where the test spheres are touch anything other than the Player (ground, wall, etc)
        IsGrounded = Physics.OverlapSphere(GroundCheckTransform.position, 0.25f).Length > 1;

        
        // GRAVITY CALCULATIONS (Regen, drain, changing gravity, etc)
        if (IsGrounded) {
            TimeOnGround += Time.deltaTime;

            if (TimeOnGround > GravDisableRegenBuffer && GravDisablePer < GravDisablePerMax && !IsDisablingGrav) {
                GravDisablePer += GravDisableRegenPerSec * (Time.deltaTime); // 20% / second
            }
            if (GravDisablePer > GravDisablePerMax) {
                GravDisablePer = GravDisablePerMax;
            }
        } else {
            TimeOnGround = 0f;
        }

        if (IsDisablingGrav) {
            Physics.gravity = new Vector3(0f, -GravDisableForce, 0f);
            GravDisablePer -= Time.deltaTime * GravDisableDrainPerSec;

            if (GravDisablePer <= 0.1f) {
                Debug.Log("Is low" + IsDisablingGrav.ToString());
                IsDisablingGrav = false;
            }

        } else {
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
        }


        if (IsJumping) {
            if (IsGrounded) {
                Player.AddForce(Vector3.up*JumpForce, ForceMode.Impulse);
                IsJumping = false;
            }
        }

        


        float totalSpeed = 1f; //  Must be declared here to reset it back to 1 every frame, otherwise you will become VERY fast
        if (IsWalking) {
            totalSpeed *= WalkMultiplier;
        }
        if (IsSprinting) {
            totalSpeed *= SprintMultiplier;
        }
        // Note: Walking and sprinting cannot happen simultaneously

        if (IsCrouching) {
            // Slow down player
            totalSpeed *= CrouchMultiplier;
        }

        Player.velocity = new Vector3(HorizontalInput.x*totalSpeed, Player.velocity.y, HorizontalInput.z*totalSpeed);
        bool[] BooleanValues = {IsCrouching, IsWalking, IsSprinting};
        string[] StringValues = {"Crouching", "Walking", "Sprinting"};

        if (DebugActive) {
            LogInfo(BooleanValues, StringValues);
        }

        if (Restarting) {
            Restarting = false;
            Player.position = new Vector3(0f, 3f, 0f);
            Player.velocity = Vector3.zero;
        }
    }
}

