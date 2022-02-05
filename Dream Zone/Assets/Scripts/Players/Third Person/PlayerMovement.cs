using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum rbMode { Velocity, Teleport };
    public rbMode movementMode;
    public float rbMaxFallSpeed { get; private set; }

    [SerializeField] private float rbVelocityMultiplier = 6f;

    private PlayerCore playerCore;
    public Vector3 playerMovement;
    private Tools rbModeTimer;

    private void Start()
    {
        playerCore = GetComponent<PlayerCore>();
        rbModeTimer = new Tools();
    }

    private void FixedUpdate()
    {
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
            playerMovement *= playerSpeed + rbVelocityMultiplier;
            playerCore.playerController.rb.velocity = new Vector3(playerMovement.x, playerCore.playerController.rb.velocity.y, playerMovement.z);
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
        if (playerCore.collisionSenses.StepCheck())
        {
            playerCore.playerController.rb.velocity = Vector3.zero;
            movementMode = rbMode.Teleport;
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

    public void setFallVelocity(float fallVelocity) => rbMaxFallSpeed = fallVelocity;
}
