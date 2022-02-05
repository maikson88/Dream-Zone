using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollisionSenses : MonoBehaviour
{
    [Header("Small Step Climb")]
    [SerializeField]
    private Transform smallStepUpper;
    [SerializeField]
    private Transform smallStepLower;
    [SerializeField]
    private Vector3 climbVelocity = Vector3.up * 5f;
    [SerializeField]
    private float rayDistance = 0.5f;

    [Header("GroundCheck")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Vector3 groundCheckOffset;
    [SerializeField]
    private float groundCheckRadius;

    public bool StepCheck()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(smallStepLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, rayDistance))
        {
            Debug.Log("Detected Lower 1");
            RaycastHit hitUpper;
            if (!Physics.Raycast(smallStepUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, rayDistance))
            {
                Debug.Log("Not Detected upper 1");
                return true;
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(smallStepLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, rayDistance))
        {
            Debug.Log("Detected Lower 2");
            RaycastHit hitUpper45;
            if (!Physics.Raycast(smallStepUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, rayDistance))
            {
                Debug.Log("Detected Upper 2");
                return true;
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(smallStepLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, rayDistance))
        {
            Debug.Log("Detected Lower 3");
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(smallStepUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, rayDistance))
            {
                Debug.Log("Detected Upper 3");
                return true;
            }
        }

        return false;
    }

    public bool CheckIfTouchingGround()
    {
        if (Physics.CheckSphere(groundCheck.position + groundCheckOffset, groundCheckRadius, 11)) return true;
        else return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
        

        Gizmos.DrawRay(smallStepLower.transform.position, transform.TransformDirection(Vector3.forward * rayDistance));
        Gizmos.DrawRay(smallStepUpper.transform.position, transform.TransformDirection(Vector3.forward * rayDistance));

        Gizmos.DrawRay(smallStepLower.transform.position, transform.TransformDirection(new Vector3(1.5f, 0, 1) * rayDistance));
        Gizmos.DrawRay(smallStepUpper.transform.position, transform.TransformDirection(new Vector3(1.5f, 0, 1) * rayDistance));

        Gizmos.DrawRay(smallStepLower.transform.position, transform.TransformDirection(new Vector3(-1.5f, 0, 1) * rayDistance));
        Gizmos.DrawRay(smallStepUpper.transform.position, transform.TransformDirection(new Vector3(-1.5f, 0, 1) * rayDistance));

    }
}
