using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotInput : MonoBehaviour
{
    private PlayerInput playerInput;
    public Vector2 RawMovementInput;

    public Vector2 NormalizedInput;

    [SerializeField]
    private float jumpInputMaxBufferTime = 0.2f;

    private float jumpInputStartBuffer;
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }

    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        JumpInputBuffer();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormalizedInput = RawMovementInput.normalized; 
        //NormalizedInput = Vector2.ClampMagnitude(RawMovementInput, 1f);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartBuffer = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    private void JumpInputBuffer()
    {
        if (Time.time >= jumpInputStartBuffer + jumpInputMaxBufferTime)
        {
            JumpInput = false;
        }
    }
}
