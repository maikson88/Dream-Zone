using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum rbMode { Velocity, Teleport, Gravity };
    public rbMode movementMode;
    public float rbMaxFallSpeed { get; private set; }

    [SerializeField] private float rbVelocityMultiplier = 6f;
    [SerializeField] private float sizeOfSteps = -15f;

    private PlayerCore playerCore;
    public Vector3 playerMovement;
    private Tools rbModeTimer;

    private Vector3 oldVelocity;


    private void Start()
    {
        playerCore = GetComponent<PlayerCore>();
        rbModeTimer = new Tools();
    }

    int stepsSinceLastGrounded;
    private void FixedUpdate()
    {
        if(!playerCore.playerController.isJumping) SnapToGround();
        ChangeRbMode();
        ClampVelocity();
    }



    public void ClampVelocity()
    {
        if (!playerCore.collisionSenses.CheckIfTouchingGround() && playerCore.playerController.rb.velocity.y < -5f)
        {
            if (playerCore.playerController.rb.velocity.magnitude > rbMaxFallSpeed)
                playerCore.playerController.rb.velocity = Vector3.ClampMagnitude(playerCore.playerController.rb.velocity, rbMaxFallSpeed);
        }
    }

    private bool SnapToGround()
    {
        if (!Physics.Raycast(playerCore.playerController.rb.position + Vector3.up * 0.5f, transform.TransformDirection(Vector3.down), out RaycastHit hit, 1.5f))
        {
            return false;
        }
        if (hit.normal.y > playerCore.collisionSenses.dotGround || hit.transform.gameObject.layer == 12)
        {
            return false;
        }

        float speed = playerCore.playerController.rb.velocity.magnitude;
        float dot = Vector3.Dot(playerCore.playerController.rb.velocity, hit.normal);
        if (dot > 0f)
            playerCore.playerController.rb.velocity = (playerCore.playerController.rb.velocity - hit.normal * dot).normalized * speed;

        return true;
    }

    public void Movement(float playerSpeed)
    {
        if (movementMode == rbMode.Teleport)
        {
            playerMovement = playerCore.playerController.xDirection + playerCore.playerController.yDirection;
            playerMovement.Normalize();
            playerMovement.y = 0;
            playerMovement *= playerSpeed;
            playerCore.playerController.rb.MovePosition(playerCore.playerController.rb.position + playerMovement * Time.fixedDeltaTime);
        }

        if (movementMode == rbMode.Velocity)
        {
            playerMovement = playerCore.playerController.xDirection + playerCore.playerController.yDirection;
            oldVelocity = new Vector3(playerMovement.x, playerCore.playerController.rb.velocity.y, playerMovement.z);

            Vector3 groundNormal = playerCore.collisionSenses.GetGroundNormal();

            playerMovement *= (playerSpeed + rbVelocityMultiplier);

            //Displacing Player movement directly To the ground Normal
            Vector3 projectedMovement = playerMovement - groundNormal * Vector3.Dot(groundNormal, playerMovement);
            Vector3 newMovement = new Vector3(projectedMovement.x, playerCore.playerController.rb.velocity.y, projectedMovement.z);
            playerCore.playerController.rb.velocity = newMovement;
        }
    }

    public void FlyMovement(float playerSpeed)
    {
        playerMovement = playerCore.playerController.xDirection + playerCore.playerController.yDirection;
        playerMovement *= playerSpeed + rbVelocityMultiplier;
        playerCore.playerController.rb.velocity = new Vector3(playerMovement.x, playerMovement.y, playerMovement.z);
    }

    public void PlayerRotation()
    {
        if (playerMovement != Vector3.zero && !playerCore.playerController.playerInput.AimInput)
        {
            if (playerCore.animEvents.isSuperJump) return;

            Quaternion targetRotation = Quaternion.LookRotation(playerCore.playerMovement.playerMovement);
            Quaternion smoothRotation = Quaternion.Lerp(playerCore.playerController.transform.rotation, targetRotation, 9f * Time.deltaTime);
            playerCore.playerController.transform.rotation = Quaternion.Euler(0, smoothRotation.eulerAngles.y, 0);

        }
        if (playerCore.playerController.playerInput.AimInput)
        {
            Quaternion targetRotation = Quaternion.Euler(0, playerCore.cameraTransform.eulerAngles.y, 0);
            Quaternion smoothRotation = Quaternion.Lerp(playerCore.playerController.transform.rotation, targetRotation, 9f * Time.deltaTime);
            playerCore.playerController.transform.rotation = smoothRotation;
        }
    }

    public void JumpFoward(float jumpForce)
    {
        playerCore.playerController.rb.velocity = transform.up * jumpForce;
    }

    public void JumpUpward(float jumpForce)
    {
        playerCore.playerController.rb.velocity = transform.up * jumpForce;
    }

    //The movement is based on how realistic I need rigidbody to be in wich occasion 
    public void ChangeRbMode()
    {
        if (playerCore.collisionSenses.StepCheck() && playerCore.playerController.playerInput.NormalizedMovementInput != Vector2.zero)
        {
            playerCore.playerController.rb.velocity = Vector3.zero;
            movementMode = rbMode.Teleport;
            if (!SnapToGround() && !playerCore.collisionSenses.CheckIfTheresSlopeNear() && playerCore.collisionSenses.CheckIfTouchingGround())
            {
                //Debug.LogError("FLYing into the night");
                playerCore.playerController.rb.position -= new Vector3(0f, sizeOfSteps * Time.deltaTime, 0f);
            }
            rbModeTimer.ResetTime();
        }
        else
        {
            if (rbModeTimer.TimeHasPassed(0.7f))
            {
            movementMode = rbMode.Velocity;
            }
        }
    }

    private void OnDrawGizmos()
    {
        //Showing the projection on Plane (For documentation pourposes)

        //Gizmos.color = Color.cyan;
        //Gizmos.DrawLine(transform.position, transform.position + oldVelocity.normalized);
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(transform.position, transform.position + playerCore.playerController.rb.velocity.normalized);
        //Debug.Log(playerCore.playerController.rb.velocity.normalized);
    }

    public void setFallVelocity(float fallVelocity) => rbMaxFallSpeed = fallVelocity;
}
