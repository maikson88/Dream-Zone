using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Tools JumpBufferCheck;
    private Tools AttackBufferCheck;
    private Tools ActionBufferCheck;
    public Vector2 RawMovementInput;
    public Vector2 NormalizedMovementInput;

    public float isJumpPressed;

    [SerializeField]
    private float JumpBuffer = 0.15f;
    //[SerializeField]
    //private float ActionBuffer = 0.2f;
    //[SerializeField]
    //private float AttackBuffer = 0.2f;


    public bool JumpInput { get; private set; }
    public bool CanJump { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool ActionInput { get; private set; }
    public bool CanCombo { get; private set; }
    public Vector2 LookInput { get; private set; }
    public Vector2 NormalizedLookInput { get; private set; }
    public bool AimInput;
    public bool AttackInput { get; private set; }

    public void Start()
    {
        JumpBufferCheck = new Tools();
        AttackBufferCheck = new Tools();
        ActionBufferCheck = new Tools();
        AimInput = false;
    }

    private void Update()
    {
        //InputBuffers();
    }



    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormalizedMovementInput = RawMovementInput.normalized;

        //Debug.Log(Vector3.Angle(NormalizedMovementInput, Vector2.up));


        //if (RawMovementInput >= new Vector2(-0.7f, 0.7f))
        //    Debug.Log("runningFoward");
    }

    public void OnAimInput(InputAction.CallbackContext context)
    {
        AimInput = true;

        if (context.canceled)
            AimInput = false;
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValue<float>();

        if (context.started)
        {
            JumpInput = true;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
        }

        if (context.canceled)
        {
            AttackInput = false;
        }
    }

    public void OnActionInput(InputAction.CallbackContext context)
    {
        if (context.started)
            ActionInput = true;

        if (context.canceled)
            ActionInput = false;
    }

    string pathToYourFile = @"C:\UnityScreenshots\";
    public void OnScreenshotInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ScreenCapture.CaptureScreenshot(pathToYourFile + "Screenshot__" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png", 2);
        }
    }

    public void InputBuffers()
    {
        if (JumpInput && JumpBufferCheck.TimeHasPassed(JumpBuffer))
        {
            JumpInput = false;
        }
    }

    public void SetJump(bool option) => JumpInput = option;
}
