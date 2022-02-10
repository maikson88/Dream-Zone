using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CollisionSenses : MonoBehaviour
{
    [SerializeField] private PlayerCore playerCore;

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
    public LayerMask wallRunLayer;

    public Vector3 dotGround { get; private set; }

    public bool CheckStep()
    {
        if (CheckTheresSlopeNear()) return false;
        if (playerCore.playerController.currentState != PlayerController.playerStates.groundMoving) return false;

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
        if (CheckTouchingGround() && cachedNormal.y >= dotGround)
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

    public bool CheckTheresSlopeNear()
    {
        RaycastHit hitFo;
        Vector3 cachedNormal;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitFo, rayDistance, groundLayers))
        {
            if (CheckIsSlope()) return true;
        }

        else if (Physics.Raycast(transform.position, transform.TransformDirection(1.5f, 0, 1), out hitFo, rayDistance, groundLayers))
        {
            if (CheckIsSlope()) return true;
        }

        else if (Physics.Raycast(transform.position,-transform.TransformDirection(-1.5f, 0, 1), out hitFo, rayDistance, groundLayers))
        {
            if (CheckIsSlope()) return true;
        }

        else if (Physics.Raycast(chestCheck.position, transform.TransformDirection(Vector3.down), out hitFo, 2.5f, groundLayers))
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
            float dotSlope = Vector3.Dot(hitFo.normal, Vector3.up); ;

            //Comparing if raycast is considered Ground
            if (Mathf.Abs(dotSlope) > 0.7 && Mathf.Abs(dotSlope) < 0.95)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public bool CheckTouchingGround()
    {
        if (Physics.CheckSphere(groundCheck.position + groundCheckOffset, groundCheckRadius, groundLayers))
            return true;
        else return false;
    }

    public bool CheckWallRun()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), rayDistance, wallRunLayer))
            return true;
        else return false;
    }

    public bool CheckWallRight(float distanceToWall)
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), distanceToWall, wallRunLayer);
    }

    public Vector3 GetWallRightNormal()
    {
        RaycastHit hitFo;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitFo, 1f, wallRunLayer))
        {
            return hitFo.normal;
        }
        else return Vector3.zero;
    }

    public Vector3 GetWallRightPosition()
    {
        RaycastHit hitFo;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitFo, 1f, wallRunLayer))
        {
            return hitFo.point;
        }
        else return Vector3.zero;
    }

    public bool CheckWallLeft(float distanceToWall)
    {
        return Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), distanceToWall, wallRunLayer);
    }

    public Vector3 GetWallLeftNormal()
    {
        RaycastHit hitFo;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitFo, 1f, wallRunLayer))
        {
            return hitFo.normal;
        }
        else return Vector3.zero;
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
        if(CheckTouchingGround()) Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);
    }
}
