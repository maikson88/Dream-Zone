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
    private float rayDistance = 0.5f;

    [Header("GroundCheck")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Vector3 groundCheckOffset;
    [SerializeField]
    private float groundCheckRadius;

    [Header("GeneralCheck")]
    [SerializeField]
    private Transform chestCheck;

    [Header("Slopes and Snapping")]
    public float minGroundAngle = 25;

    [Header("Layers")]
    public LayerMask groundLayers;
    public LayerMask wallRunUpLayer;

    public Vector3 dotGround { get; private set; }

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

    public Vector3 CheckGroundNormal()
    {
        RaycastHit hitFo;
        Vector3 cachedNormal;


        if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitFo, rayDistance))
        return Vector3.up;

        cachedNormal = hitFo.normal;

        //Converting angle to dot
        float minGroundAngle = 20;
        float angleToRadians = minGroundAngle * Mathf.Deg2Rad;
        float dotGround = Mathf.Cos(angleToRadians);

        //Comparing if raycast is considered Ground
        if (CheckIfTouchingGround() && cachedNormal.y >= dotGround)
        {
            Debug.Log(dotGround);
            return cachedNormal;
        }
        else return Vector3.up;
    }

    public Vector3 GetGroundNormal()
    {
        RaycastHit hitFo;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitFo, rayDistance))
        {
            dotGround = hitFo.normal;
            return dotGround;
        }
        else return Vector3.up;
    }

    public bool CheckIfTheresSlopeNear()
    {
        RaycastHit hitFo;
        Vector3 cachedNormal;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitFo, rayDistance))
        {
            if(CheckIsSlope()) return true;
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.forward), out hitFo, rayDistance))
        {
            if (CheckIsSlope()) return true;
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(1.5f, 0, 1), out hitFo, rayDistance))
        {
            if (CheckIsSlope()) return true;
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitFo, rayDistance))
        {
            if (CheckIsSlope()) return true;        
        }
        else if (Physics.Raycast(transform.position, -transform.TransformDirection(1.5f, 0, 1), out hitFo, rayDistance))
        {
            if (CheckIsSlope()) return true;
        }
        else if (Physics.Raycast(transform.position, -transform.TransformDirection(-1.5f, 0, 1), out hitFo, rayDistance))
        {
            if (CheckIsSlope()) return true;
        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up *0.3f + Vector3.down), out hitFo, rayDistance))
        {
            if (CheckIsSlope()) return true;
        }

        return false;

        bool CheckIsSlope()
        {
            cachedNormal = hitFo.normal;
            //Converting angle to dot
            float angleToRadians = minGroundAngle * Mathf.Deg2Rad;
            float dotGround = Mathf.Cos(angleToRadians);
            //Comparing if raycast is considered Ground
            if (cachedNormal.y < dotGround)
            {
                return true;
            }
            else return false;
        }
    }

    public bool CheckIfTouchingGround()
    {
        if (Physics.CheckSphere(groundCheck.position + groundCheckOffset, groundCheckRadius, groundLayers))
            return true;
        else return false;
    }

    public bool WallRunCheck()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), rayDistance, wallRunUpLayer))
            return true;
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
