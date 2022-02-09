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
        onGround = playerCore.collisionSenses.CheckTouchingGround();
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

        if(playerInput.NormalizedMovementInput != Vector2.zero)
            playerCore.playerMovement.Movement(playerCore.playerData.playerSpeed);


        if (playerCore.collisionSenses.CheckWallLeft() && playerInput.ActionInput)
        {
            currentState = playerStates.wallRunning;
        }

        if (playerCore.collisionSenses.CheckWallRight() && playerInput.ActionInput)
        {
            currentState = playerStates.wallRunning;
        }


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
        rb.useGravity = false;


        playerCore.animEvents.SkipAnimationTo("Movement");
        anim.SetFloat("Running Speed", 1);


        //Stick to wall and Run
        if (playerCore.collisionSenses.CheckWallRight())
        {  
            rb.AddForce(transform.TransformDirection(Vector3.right * 2)); //Stick to Wall

            Quaternion playerRotation = Quaternion.LookRotation(playerCore.collisionSenses.GetWallRightNormal());
            playerRotation *= Quaternion.LookRotation(Vector3.right) * Quaternion.Euler(0, 0, 30);  //Rotate
            transform.rotation = playerRotation;

            playerCore.playerMovement.DirectionalVelocity(transform.TransformDirection(Vector3.forward), playerCore.playerData.jumpForce, false); //Run
        }
        else if (playerCore.collisionSenses.CheckWallLeft())
        {
            rb.AddForce(transform.TransformDirection(Vector3.left * 2));  //Stick to Wall

            Quaternion playerRotation = Quaternion.LookRotation(playerCore.collisionSenses.GetWallLeftNormal()); 
            playerRotation *= Quaternion.LookRotation(Vector3.left) * Quaternion.Euler(0, 0, -30); //Rotate
            transform.rotation = playerRotation;

            playerCore.playerMovement.DirectionalVelocity(transform.TransformDirection(Vector3.forward), playerCore.playerData.jumpForce, false); //Run
        }

        //Jump in Opposit Direction
        if ( (playerCore.collisionSenses.CheckWallRight() || playerCore.collisionSenses.CheckWallLeft() ) && playerInput.isJumpPressed > 0)
        {
            rb.useGravity = true;
            Vector3 wallNormal;
            if ((playerCore.collisionSenses.CheckWallRight())) wallNormal = playerCore.collisionSenses.GetWallRightNormal();
            else wallNormal = playerCore.collisionSenses.GetWallLeftNormal();

            Vector3 JumpDirection = (
                wallNormal * playerCore.playerData.normalWallJumpMultiplier
                + transform.TransformDirection(Vector3.up) * playerCore.playerData.upWallJumpMultiplier
                + transform.TransformDirection(Vector3.forward) * playerCore.playerData.fowardWallJumpMultiplier);

            playerCore.playerMovement.DirectionalVelocity(JumpDirection, playerCore.playerData.jumpForce, true);


            anim.SetBool("isJumping", true);
            anim.SetBool("isGroundMoving", false);
            currentState = playerStates.runJumping;
        }




        if (!playerCore.collisionSenses.CheckWallRight() && !playerCore.collisionSenses.CheckWallLeft())
        {
            rb.useGravity = true;
            currentState = playerStates.onAir;
        }

    }

    public bool IsJumpingReset() => isJumping = false;


    #region Gizmos
#if UNITY_EDITOR
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle();

        string currentStateDebug = currentState.ToString(); 
        GUI.Box(new Rect(0, 0, 200, 25), currentStateDebug);

        string checkGround = "Ground : " + playerCore.collisionSenses.CheckTouchingGround();
        GUI.Box(new Rect(335, 0, 125, 25), checkGround);

        string checkGrabbable = "Slope : " + playerCore.collisionSenses.CheckTheresSlopeNear(); ;
        GUI.Box(new Rect(205, 0, 125, 25), checkGrabbable);

        string checkWallAbove = "Stairs: " + playerCore.collisionSenses.CheckStep(); ;
        GUI.Box(new Rect(465, 0, 125, 25), checkWallAbove);


        string checkWallRun = "Wall Front : " + playerCore.collisionSenses.CheckWallRun(); ;
        GUI.Box(new Rect(625, 160, 125, 25), checkWallRun);

        string checkWallRight = "Wall Right: " + playerCore.collisionSenses.CheckWallRight();
        GUI.Box(new Rect(700, 200, 125, 25), checkWallRight);

        string checkWallLeft = "Wall Left: " + playerCore.collisionSenses.CheckWallLeft();
        GUI.Box(new Rect(550, 200, 125, 25), checkWallLeft);

        if (currentState.ToString() != _previousState)
            //Debug.Log(string.Concat("State : ", currentStateDebug));

        _previousState = currentStateDebug;
    }
#endif
    #endregion
}