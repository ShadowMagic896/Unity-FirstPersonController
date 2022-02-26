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

    private float DoubleJumpForce = 2f;
    private bool HasDoubleJump;
    private bool IsGrounded;

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

    private bool ToggleCrouch = false; // Whether to toggle or hold keys for crouching
    private bool ToggleWalk = false; // Whether to toggle or hold keys for walking
    private bool ToggleSprint = false; // Whether to toggle or hold keys for sprinting
    private bool OverrideControls = true; // Whether keys can override other ones or not (walking / running)

    private float VeloX, VeloY, VeloZ;

    private bool LVaultPossible;
    private bool HVaultPossible;
    private bool BadVaultPossible;

    public static int DisplayControlScheme = 2; // 1 is First Person, 2 is Third Person


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


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            IsJumping=true;
        }
        IsForward = Input.GetKey(KeyCode.W);
        
        // Key is pressed, and either we are not walking or we override the walking

        // Seperate actions for whether toggle or hold.

        // If it's toggle, we turn off walking and toggle the sprinting.

        // If it isn't toggle, then we turn it on when the key is pressed, and then add the 
        // extra clause at the bottom to turn it off when the key is not being pressed

        if (Input.GetKey(KeyCode.LeftShift) && (!IsSprinting || OverrideControls)){
            if (ToggleSprint) {
                IsWalking = false;
                IsSprinting = !IsSprinting;
            } else if (!ToggleSprint && (!IsWalking || OverrideControls)) {
                IsWalking = false;
                IsSprinting = true;
            }
        } else if ((!Input.GetKey(KeyCode.LeftShift))) {
            IsSprinting = false;
        }

        if (Input.GetKey(KeyCode.LeftAlt) && (!IsWalking || OverrideControls)){
            if (ToggleWalk) {
                IsSprinting = false;
                IsWalking = !IsWalking;
            } else if (!ToggleWalk && (!IsSprinting || OverrideControls)) {
                IsSprinting = false;
                IsWalking = true;
            }
        } else if ((!Input.GetKey(KeyCode.LeftAlt))) {
            IsWalking = false;
        }

        if (Input.GetKey(KeyCode.LeftControl)){
            if (ToggleCrouch) {
                IsCrouching = !IsCrouching;
            } else {
                IsCrouching = true;
            }
        } else if ((!Input.GetKey(KeyCode.LeftControl) && !ToggleCrouch)) {
            IsCrouching = false;
        }

        HorizontalInput = transform.right * Input.GetAxis("Horizontal") * MovementSpeed + transform.forward * Input.GetAxis("Vertical") * MovementSpeed;


    }

    void FixedUpdate()
    {

        // These test if a sphere placed where the test spheres are touch anything other than the Player (ground, wall, etc)
        IsGrounded = Physics.OverlapSphere(GroundCheckTransform.position, 0.75f).Length > 1;

        /*
        Unused ATM

        LVaultPossible = Physics.OverlapSphere(LowerVaultTransform.position, 0.5f).Length > 1;
        HVaultPossible = Physics.OverlapSphere(HigherVaultTransform.position, 0.5f).Length > 1;
        BadVaultPossible = Physics.OverlapSphere(BadVaultTransform.position, 1f).Length > 1;
        */


        if (IsJumping) {
            if (IsGrounded) {
                Player.AddForce(Vector3.up*JumpForce, ForceMode.Impulse);
                IsJumping = false;
                HasDoubleJump = true;
            } 

            else if (HasDoubleJump) {
                Player.AddForce(Vector3.up*DoubleJumpForce, ForceMode.Impulse);
                IsJumping = false;
                HasDoubleJump = false;

                
                VeloX = Player.velocity.x; // Side to side
                VeloY = Player.velocity.y; // Up and down
                VeloZ = Player.velocity.z; // Front to back

                Player.AddForce(
                    VeloX * 2f, 
                    7f, 
                    VeloZ * 2f, 
                    ForceMode.Impulse
                );
            }
        }
        /*
        if (!BadVaultPossible && Input.GetKeyDown(KeyCode.W)){
            if (HVaultPossible){
                Player.AddForce(
                    Vector3.up * 500f,
                    ForceMode.Impulse
                );
            } else if (LVaultPossible) {
                Player.AddForce(
                    Vector3.up * 1.25f,
                    ForceMode.Impulse
                );
            }
        }

        Debug.Log(
            "BadVault: " + BadVaultPossible + ", LVault: " + LVaultPossible + ", HVault: " + HVaultPossible + ", WKEY: " + IsForward
        );
        */


        float totalSpeed = 1f;
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

        LogInfo(BooleanValues, StringValues);
    }
}

