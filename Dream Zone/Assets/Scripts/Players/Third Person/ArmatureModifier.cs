using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmatureModifier : MonoBehaviour
{
    private InputHandler inputHandler;
    private PlayerController playerController;

    [SerializeField]
    private Transform spine;    
    [SerializeField]
    private Transform spine1;    
    [SerializeField]
    private Transform spine2;

    [SerializeField]
    private Transform weaponForeArmBone;

    [SerializeField]
    private Transform freeForeArmBone;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 rotateForeArm;

    [SerializeField]
    private Vector3 rotateFreeForeArm;

    [SerializeField]
    private bool spine1b;
    [SerializeField]
    private bool spine2b;
    [SerializeField]
    private bool spine3b;

    // Start is called before the first frame update
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerController = GetComponent<PlayerController>();
    }

    Vector2 lastLookDirection;

    private float spineHorizontalDir;
    private float spineVerticalDir;

    [SerializeField]
    private float xLeftRotateClamp = -60f;
    [SerializeField]
    private float xRightRotateClamp = 60f;
    [SerializeField]
    private float yLeftRotateClamp = -60f;
    [SerializeField]
    private float yRightRotateClamp = 60f;

    private bool isRunning;

    private bool isShootingRotated;

    private void LateUpdate()
    {
        if (!playerController.anim.GetBehaviour<FiringRifleMecanim>().isShooting)
        {
            isShootingRotated = false;
            ResetArmature();
            return;
        }

        isRunning = playerController.playerInput.NormalizedMovementInput != Vector2.zero ? true : false;

        ApplyRotation();

        if (!isShootingRotated)
        {
            ReadShootingRotation();
        }
    }

    private void ReadShootingRotation()
    {
        if (playerController.playerCore.playerMovement.playerDirection == PlayerMovement.PlayerDirection.Foward)
        {
            if (isRunning)
            {
                rotateForeArm = new Vector3(-31.7f, 0, -47.8f);
                rotateFreeForeArm = new Vector3(32.6f, 0, 69f);
                spineHorizontalDir = 0f;
                spineVerticalDir = 7;
            }

            else
            {
                DefaultAimming();
            }
        }

        if (playerController.playerCore.playerMovement.playerDirection == PlayerMovement.PlayerDirection.UpLeft)
        {
            if (isRunning)
            {
                rotateForeArm = new Vector3(-31.7f, 0, -47.8f);
                rotateFreeForeArm = new Vector3(32.6f, 0, 69f);
                spineHorizontalDir = 14f;
                spineVerticalDir = 7;
            }
            else
            {
                rotateForeArm = new Vector3(-31f, 0, 0);
                rotateFreeForeArm = new Vector3(45f, 0, 25f);
                spineHorizontalDir = 15f;
                spineVerticalDir = -4;
            }
        }

        if (playerController.playerCore.playerMovement.playerDirection == PlayerMovement.PlayerDirection.Left)
        {
            if (isRunning)
            {
                rotateFreeForeArm = new Vector3(32.6f, 0, 69f);
                rotateForeArm = new Vector3(-31.7f, 0, -47.8f);
                spineHorizontalDir = 25f;
                spineVerticalDir = 0;
            }
            else
            {
                rotateForeArm = new Vector3(-31f, 0, 0);
                rotateFreeForeArm = new Vector3(45f, 0, 25f);
                spineHorizontalDir = 25f;
                spineVerticalDir = -4;
            }
        }

        if (playerController.playerCore.playerMovement.playerDirection == PlayerMovement.PlayerDirection.UpRight)
        {
            if (isRunning)
            {
                rotateFreeForeArm = Vector3.zero;
                rotateForeArm = Vector3.zero;
                spineHorizontalDir = 0f;
                spineVerticalDir = 0;
            }
            else
            {
                rotateForeArm = new Vector3(-31f, 0, 0);
                rotateFreeForeArm = new Vector3(45f, 0, 25f);
                spineHorizontalDir = -15f;
                spineVerticalDir = -4;
            }
        }

        if (playerController.playerCore.playerMovement.playerDirection == PlayerMovement.PlayerDirection.Right)
        {
            if (isRunning)
            {
                rotateFreeForeArm = Vector3.zero;
                rotateForeArm = Vector3.zero;
                spineHorizontalDir = -20f;
                spineVerticalDir = 0;
            }
            else
            {
                rotateForeArm = new Vector3(-31f, 0, 0);
                rotateFreeForeArm = new Vector3(45f, 0, 25f);
                spineHorizontalDir = -25f;
                spineVerticalDir = -4;
            }
        }

        if (playerController.playerCore.playerMovement.playerDirection == PlayerMovement.PlayerDirection.Downwards)
        {
            DefaultAimming();
        }

        isShootingRotated = true;
    }

    private void ApplyRotation()
    {
        lastLookDirection = new Vector2(-spineVerticalDir, spineHorizontalDir);
        lastLookDirection.x = Mathf.Clamp(lastLookDirection.x, xLeftRotateClamp, xRightRotateClamp);
        lastLookDirection.y = Mathf.Clamp(lastLookDirection.y, yLeftRotateClamp, yRightRotateClamp);


        freeForeArmBone.transform.rotation *= Quaternion.Euler(rotateFreeForeArm);
        weaponForeArmBone.transform.rotation *= Quaternion.Euler(rotateForeArm);

        if (spine1b)
            spine.Rotate(lastLookDirection);
        if (spine2b)
            spine1.Rotate(lastLookDirection);
        if (spine3b)
            spine2.Rotate(lastLookDirection);
    }

    private void DefaultAimming()
    {
        rotateForeArm = new Vector3(-31f, 0, 0);
        rotateFreeForeArm = new Vector3(45f, 0, 25f);
        spineHorizontalDir = 0f;
        spineVerticalDir = -4;
    }

    private void ResetArmature()
    {
        rotateForeArm = Vector3.zero;
        rotateFreeForeArm = Vector3.zero;
        spineHorizontalDir = 0f;
        spineVerticalDir = 0f;
    }
}
