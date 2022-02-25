using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public static float Abs(float x){
        if (x < 0){
            return -x;
        } return x;
    }

    private Rigidbody Player;

    private bool IsJumping;
    private float JumpForce = 7f;

    private bool IsForward;

    private float DoubleJumpForce = 2f;
    private bool HasDoubleJump;
    private bool IsGrounded;

    private bool IsWalking;
    private float WalkSpeed = 1f;


    private Vector3 HorizontalInput;
    private float MovementSpeed = 2f;
    
    private bool IsSprinting;
    private float SprintSpeed = 4f;

    private bool ToggleWalk = false; // Whether to toggle or hold keys for walking
    private bool ToggleSprint = false; // Whether to toggle or hold keys for sprinting
    private bool OverrideControls = false; // Whether keys can override other ones or not (walking / running)

    private float VeloX, VeloY, VeloZ;

    private bool LVaultPossible;
    private bool HVaultPossible;
    private bool BadVaultPossible;


    [SerializeField] private Transform GroundCheckTransform;
    [SerializeField] private Transform HigherVaultTransform;
    [SerializeField] private Transform LowerVaultTransform;
    [SerializeField] private Transform BadVaultTransform;

    private string StrTimesInt(string str, int times) {
        string end = "";
        for (int i = 0; i < times; i++) {
            end += str;
        }
        return end;
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
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && (OverrideControls || !IsWalking)){
            IsWalking = false;
            IsSprinting = !IsSprinting;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt) && (OverrideControls || !IsSprinting)){
            IsSprinting = false;
            IsWalking = !IsWalking;
        }

        HorizontalInput = transform.right * Input.GetAxis("Horizontal") * MovementSpeed + transform.forward * Input.GetAxis("Vertical") * MovementSpeed;


    }

    void FixedUpdate()
    {

        // These test if a sphere placed where the test spheres are touch anything other than the player (ground, wall, etc)
        IsGrounded = Physics.OverlapSphere(GroundCheckTransform.position, 0.75f).Length > 1;

        LVaultPossible = Physics.OverlapSphere(LowerVaultTransform.position, 0.5f).Length > 1;
        HVaultPossible = Physics.OverlapSphere(HigherVaultTransform.position, 0.5f).Length > 1;
        BadVaultPossible = Physics.OverlapSphere(BadVaultTransform.position, 1f).Length > 1;



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
        // Debug.Log(
        //     "BadVault: " + BadVaultPossible + ", LVault: " + LVaultPossible + ", HVault: " + HVaultPossible + ", WKEY: " + IsForward
        // );
            
        if (IsSprinting) {
            Player.velocity = new Vector3(HorizontalInput.x*SprintSpeed, Player.velocity.y, HorizontalInput.z*SprintSpeed);
        }

        else if (IsWalking) {
            Player.velocity = new Vector3(HorizontalInput.x*WalkSpeed, Player.velocity.y, HorizontalInput.z*WalkSpeed);
        }

        else {
            Player.velocity = new Vector3(HorizontalInput.x*MovementSpeed, Player.velocity.y, HorizontalInput.z*MovementSpeed);
        }
        Debug.Log((IsWalking ? "Walk" : "NoWalk") + " " + (IsSprinting ? "Sprint" : "NoSprint"));
    }
}