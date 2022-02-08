using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    public enum playerStates { groundMoving, idleJumping, runJumping, superJumping, onAir, wallRunning, waiting };
    public playerStates currentState;

    [SerializeField]
    private PlayerCore playerCore;

    public InputHandler playerInput { get; private set; }
    public bool isJumping { get; private set; }
    public  Rigidbody rb { get; private set; }
    public Vector3 xDirection { get; private set; }
    public Vector3 yDirection { get; private set; }

    public bool canJump;

    private Tools JumpBuffer;
    private Animator anim;
    private string _previousState;
    private int jumpCharge;
    private bool onGround;

    private void Start()
    {
        playerInput = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        JumpBuffer = new Tools();
        currentState = playerStates.groundMoving;
        playerCore.playerMovement.setFallVelocity(playerCore.playerData.maxFallVelocity);
    }

    private void FixedUpdate()
    {
        onGround = playerCore.collisionSenses.CheckIfTouchingGround();
        playerCore.playerMovement.PlayerRotation();
        SwitchStates();
    }

    void Update()
    {
        xDirection = playerCore.cameraTransform.right.normalized * playerInput.NormalizedMovementInput.x;
        yDirection = playerCore.cameraTransform.forward.normalized * playerInput.NormalizedMovementInput.y;

        if (onGround)
        {
            if (JumpBuffer.TimeHasPassed(0.2f))
            {
                canJump = true;
            }
        }
        else if (!onGround)
        {
            canJump = false;
            playerInput.SetJump(false);
        }
    }

    private void SwitchStates()
    {
        switch (currentState)
        {
            case playerStates.groundMoving:
                GroundMoving();
                break;

            case playerStates.idleJumping:
                idleJumping();
                break;

            case playerStates.runJumping:
                RunJumping();
                break;

            case playerStates.onAir:
                onAir();
                break;

            case playerStates.superJumping:
                SuperJump();
                break;

            case playerStates.wallRunning:
                WallRun();
                break;

            case playerStates.waiting:
                break;
        }
    }



    private void GroundMoving()
    {
        anim.SetBool("isGroundMoving", true);

        playerCore.playerMovement.Movement(playerCore.playerData.playerSpeed);

        if (canJump && playerInput.JumpInput) isJumping = true;
        anim.SetBool("isJumping", isJumping);
        anim.SetFloat("Running Speed", playerInput.RawMovementInput.magnitude);

        if (!onGround)
        {
            anim.SetBool("isGroundMoving", false);
            playerCore.animEvents.SkipAnimationTo("Falling");
            currentState = playerStates.onAir;
        }

        else if (playerInput.RawMovementInput.magnitude < 0.5f & isJumping)
        {
            anim.SetBool("isGroundMoving", false);
            currentState = playerStates.idleJumping;
        }

        else if (playerInput.RawMovementInput.magnitude > 0.5f && isJumping)
        {
            anim.SetBool("isGroundMoving", false);
            currentState = playerStates.runJumping;
        }

        else if (playerCore.collisionSenses.WallRunCheck())
        {
            anim.SetBool("isGroundMoving", false);
            currentState = playerStates.wallRunning;
        }
    }

    private void RunJumping()
    {
        playerCore.playerMovement.JumpUpward(playerCore.playerData.jumpForce);
        currentState = playerStates.onAir;
    }

    private void idleJumping()
    {
        if (playerInput.isJumpPressed > 0)
        {
            playerCore.playerVfx.SetvfxShockParticle(true);
            jumpCharge++;
        }
        else
        {
            Jump();
        }
    }

    private void Jump()
    {
        playerCore.playerVfx.SetvfxShockParticle(false);
        anim.SetBool("isReadyToJump", false);
        

        if (jumpCharge < 70)
        {
            jumpCharge = 0;
            playerCore.animEvents.SuperJump(false);
            if (playerCore.animEvents.JumpUpFinished)
            {
                playerCore.playerMovement.JumpUpward(playerCore.playerData.jumpForce);
                currentState = playerStates.onAir;
            }
        }
        else if (jumpCharge >= 70)
        {
            jumpCharge = 0;
            playerCore.animEvents.SuperJump(true);
            currentState = playerStates.superJumping;
        }
    }

    private void onAir()
    {

        if (rb.velocity.y <= 0)
        {
            playerCore.animEvents.JumpReset();
            playerCore.animEvents.isSuperJump = false;
            anim.SetBool("isFalling", true);
        }

        playerCore.playerMovement.Movement(playerCore.playerData.playerSpeed);

        if (onGround && rb.velocity.y <= 0)
        {
            anim.SetBool("isFalling", false);
            currentState = playerStates.groundMoving;
        }
    }

    private void SuperJump()
    {
        if (onGround)
        {
            playerCore.playerMovement.JumpUpward(playerCore.playerData.jumpForce * 4);
            playerCore.playerVfx.SetVfxSuperJump(true);
            Quaternion targetRotation = Quaternion.Euler(-90, transform.rotation.y, 0);
            transform.rotation = targetRotation;
        }


        if(rb.velocity.y < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            playerCore.playerVfx.SetVfxSuperJump(false);
            currentState = playerStates.onAir;
        }
    }

    private void WallRun()
    {
        //Vector3 WallRunHeigh = Vector3.zero;
        //WallRunHeigh = Vector3.up * 50;
        //rb.velocity = Vector3.Lerp(rb.velocity, WallRunHeigh, 20);

    }

    public bool IsJumpingReset() => isJumping = false;


    #region Gizmos
#if UNITY_EDITOR
    void OnGUI()
    {
        string currentStateDebug = currentState.ToString(); 
        GUI.Box(new Rect(0, 0, 200, 25), currentStateDebug);

        string checkGround = "Ground : " + playerCore.collisionSenses.CheckIfTouchingGround();
        GUI.Box(new Rect(335, 0, 125, 25), checkGround);

        //TO_DO
        //string checkGrabbable = "Grabbable : " + CheckIfIsGrabbable();
        //GUI.Box(new Rect(205, 0, 125, 25), checkGrabbable);

        //string checkWall = "Wall : " + CheckIfTouchingWall();
        //GUI.Box(new Rect(465, 0, 125, 25), checkWall);

        //string checkWallAbove = "Wall Above: " + CheckIfTouchingWallAbove();
        //GUI.Box(new Rect(595, 0, 125, 25), checkWallAbove);

        //string checkWallBack = "Wall Back: " + CheckIfTouchingWallBack();
        //GUI.Box(new Rect(730, 0, 125, 25), checkWallBack);

        if (currentState.ToString() != _previousState)
            //Debug.Log(string.Concat("State : ", currentStateDebug));

        _previousState = currentStateDebug;
    }
#endif
    #endregion
}