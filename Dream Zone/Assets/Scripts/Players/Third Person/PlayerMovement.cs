using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum rbMode { Velocity, Teleport, Gravity };
    public rbMode movementMode;
    public float rbMaxFallSpeed { get; private set; }
    public Vector3 externalForce { get; private set; }
    public float ClimbHardness;

    private PlayerCore playerCore;
    public Vector3 playerMovement;
    private Tools rbModeTimer;

    private float forceTime;
    private Vector3 forceDirection;

    private void Start()
    {
        playerCore = GetComponent<PlayerCore>();
        rbModeTimer = new Tools();
        externalForce = Vector3.zero;
    }

    int stepsSinceLastGrounded;
    private void FixedUpdate()
    {
        ApplyExternalForce();
        if (!playerCore.playerController.isJumping)
            SnapToGround();
        ChangeRbMode();
        ClampVelocity();
        GravityMultiplier();
    }

    private void GravityMultiplier()
    {
        //Default
        SetGravity(playerCore.playerData.gravityForce);

        if (playerCore.playerController.currentState == PlayerController.playerStates.idleJumping)
            playerCore.playerController.rb.mass = 15f;
        else
            playerCore.playerController.rb.mass = 1f;
    }

    private void SetGravity(float gravityForce)
    {
        if (playerCore.playerController.rb.velocity.y < 0)

            playerCore.playerController.rb.velocity = new Vector3(
                playerCore.playerController.rb.velocity.x,
                playerCore.playerController.rb.velocity.y * gravityForce * playerCore.playerData.gravityForceSpeed * Time.fixedDeltaTime,
                playerCore.playerController.rb.velocity.z);
    }

    public void ClampVelocity()
    {
        if (!playerCore.collisionSenses.CheckTouchingGround() && playerCore.playerController.rb.velocity.y < -5f)
        {
            if (playerCore.playerController.rb.velocity.magnitude > playerCore.playerData.maxFallVelocity)
                playerCore.playerController.rb.velocity = Vector3.ClampMagnitude(playerCore.playerController.rb.velocity, playerCore.playerData.maxFallVelocity);
        }

        if(playerCore.playerController.currentState != PlayerController.playerStates.superJumping && playerCore.playerController.rb.velocity.magnitude > 15)
        {
            playerCore.playerController.rb.velocity = Vector3.ClampMagnitude(playerCore.playerController.rb.velocity, 15f);
        }
    }

    private bool SnapToGround()
    {
        if (!Physics.Raycast(playerCore.playerController.rb.position + Vector3.up * 0.5f, transform.TransformDirection(Vector3.down), out RaycastHit hit, 1.5f))
        {
            return false;
        }

        if (playerCore.collisionSenses.CheckTheresSlopeNear() || hit.transform.gameObject.layer == 12)
        {
            float speed = playerCore.playerController.rb.velocity.magnitude;
            float dot = Vector3.Dot(playerCore.playerController.rb.velocity, hit.normal);
            if (dot > 0f)
            {
                playerCore.playerController.rb.velocity = (playerCore.playerController.rb.velocity - hit.normal * dot).normalized * speed;
            }
            return true;
        }
        else return false;
    }

        bool noMovement;
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
            Vector3 groundNormal = playerCore.collisionSenses.GetGroundNormal();
            playerMovement *= (playerSpeed + playerCore.playerData.rbVelocityMultiplier);
            Vector3 projectedMovement = playerMovement - groundNormal * Vector3.Dot(groundNormal, playerMovement.normalized);
            Vector3 newMovement = new Vector3(projectedMovement.x, playerCore.playerController.rb.velocity.y, projectedMovement.z);
            if (playerCore.playerController.rb.velocity.y > 0)
                newMovement = new Vector3(projectedMovement.x * (groundNormal.y - ClimbHardness), playerCore.playerController.rb.velocity.y, projectedMovement.z * (groundNormal.y - ClimbHardness));

            playerCore.playerController.rb.velocity = new Vector3(newMovement.x, playerCore.playerController.rb.velocity.y, newMovement.z) + externalForce;
        }
    }

    public void FlyMovement(float playerSpeed)
    {
        playerMovement = playerCore.playerController.xDirection + playerCore.playerController.yDirection;
        playerMovement *= playerSpeed + playerCore.playerData.rbVelocityMultiplier;
        playerCore.playerController.rb.velocity = new Vector3(playerMovement.x, playerMovement.y, playerMovement.z);
    }

    public void PlayerRotation()
    {
        if (playerMovement != Vector3.zero && !playerCore.playerController.playerInput.AimInput)
        {
            if (playerCore.animEvents.isSuperJump) return;
            float yVel = playerCore.playerController.rb.velocity.y;
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
        float alignedSpeed = Vector3.Dot(playerCore.playerController.rb.velocity, playerCore.collisionSenses.GetGroundNormal());
        playerCore.playerController.rb.velocity += playerCore.collisionSenses.GetGroundNormal() * jumpForce;
    }

    public void DirectionalVelocity(Vector3 direction, float jumpForce, bool momentum)
    {
        if(momentum)
            playerCore.playerController.rb.velocity += direction * jumpForce;
        else
            playerCore.playerController.rb.velocity = direction * jumpForce;
    }

    public void ChangeRbMode()
    {
        if (!playerCore.playerController.isJumping)
        {
            if (SnapToGround())
            {
                movementMode = rbMode.Velocity;
                return;
            }
        }


        if (playerCore.collisionSenses.CheckStep() && playerCore.playerController.playerInput.NormalizedMovementInput != Vector2.zero)
        {
            playerCore.playerController.rb.velocity = Vector3.zero;
            movementMode = rbMode.Teleport;
            if (playerCore.collisionSenses.CheckTouchingGround() && playerCore.playerController.currentState == PlayerController.playerStates.groundMoving)
            {
                playerCore.playerController.rb.position -= new Vector3(0f, playerCore.playerData.maxSizeOfStairs * Time.deltaTime, 0f);
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

    public void SetExternalForce(Vector3 direction, float timer)
    {
        forceTime = timer;
        forceDirection = direction;
    }

    private void ApplyExternalForce()
    {
        if (forceTime < 0) return;
        forceTime -= Time.deltaTime;

        if (forceTime > 0)
            externalForce = forceDirection;
        else
            externalForce = Vector3.zero;
    }

    public void setFallVelocity(float fallVelocity) => rbMaxFallSpeed = fallVelocity;
}
