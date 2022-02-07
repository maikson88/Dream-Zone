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
            RaycastHit hitUpper;
            if (!Physics.Raycast(smallStepUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, rayDistance))
            {
                //Debug.Log("Detected1");
                return true;
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(smallStepLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, rayDistance))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(smallStepUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, rayDistance))
            {
                //Debug.Log("Detected2");
                return true;
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(smallStepLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, rayDistance))
        {
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(smallStepUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, rayDistance))
            {
                //Debug.Log("Detected3");
                return true;
            }
        }

        return false;
    }

    public Vector3 GetGroundNormal()
    {
        RaycastHit hitFo;
        Vector3 cachedNormal;


        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitFo, rayDistance))
        return Vector3.up;

        cachedNormal = hitFo.normal;

        //Converting angle to dot
        float minGroundAngle = 40;
        float angleToRadians = minGroundAngle * Mathf.Deg2Rad;
        float dotGround = Mathf.Cos(angleToRadians);

        //Comparing if raycast is considered Ground
        if (CheckIfTouchingGround() && cachedNormal.y >= dotGround)
            return cachedNormal;
        else return Vector3.up;
    }

    public bool CheckIfTheresSlopeNear()
    {
        RaycastHit hitFo;
        Vector3 cachedNormal;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitFo, rayDistance * 2f))
            if(CheckIsSlope()) return true;
        else if (Physics.Raycast(transform.position, transform.TransformDirection(1.5f, 0, 1), out hitFo, rayDistance * 2f))
            if(CheckIsSlope()) return true;
        else if (Physics.Raycast(transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitFo, rayDistance * 2f))
            if(CheckIsSlope()) return true;
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitFo, rayDistance * 2f))
            if(CheckIsSlope()) return true;

        return false;

        bool CheckIsSlope()
        {
            Debug.Log("is Slope");

            cachedNormal = hitFo.normal;
            //Converting angle to dot
            float minGroundAngle = 25;
            float angleToRadians = minGroundAngle * Mathf.Deg2Rad;
            float dotGround = Mathf.Cos(angleToRadians);
            //Comparing if raycast is considered Ground
            if (cachedNormal.y < dotGround)
                return true;
            else return false;
        }
    }

    public bool CheckIfTouchingGround()
    {
        if (Physics.CheckSphere(groundCheck.position + groundCheckOffset, groundCheckRadius, 11)) return true;
        else return false;
    }

    private void OnDrawGizmos()
    {
        //TO_DO making rays green when detected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
        Gizmos.DrawRay(smallStepLower.transform.position, transform.TransformDirection(Vector3.forward * rayDistance));
        Gizmos.DrawRay(smallStepUpper.transform.position, transform.TransformDirection(Vector3.forward * rayDistance));

        Gizmos.DrawRay(smallStepLower.transform.position, transform.TransformDirection(new Vector3(1.5f, 0, 1) * rayDistance));
        Gizmos.DrawRay(smallStepUpper.transform.position, transform.TransformDirection(new Vector3(1.5f, 0, 1) * rayDistance));

        Gizmos.DrawRay(smallStepLower.transform.position, transform.TransformDirection(new Vector3(-1.5f, 0, 1) * rayDistance));
        Gizmos.DrawRay(smallStepUpper.transform.position, transform.TransformDirection(new Vector3(-1.5f, 0, 1) * rayDistance));

        Gizmos.color = Color.red;
        if(CheckIfTouchingGround()) Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }
}
