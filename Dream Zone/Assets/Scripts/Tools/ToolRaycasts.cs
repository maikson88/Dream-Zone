using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolRaycasts : MonoBehaviour
{
    public RaycastHit Degree45RayFront(Vector3 origin, float raySize)
    {
        RaycastHit hitFo;

        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.forward), out hitFo, raySize))
        {
            return hitFo;
        }

        else if (Physics.Raycast(origin, transform.TransformDirection(1.5f, 0, 1), out hitFo, raySize))
        {
            return hitFo;
        }
        else if (Physics.Raycast(origin, transform.TransformDirection(-1.5f, 0, 1), out hitFo, raySize))
        {
            return hitFo;
        }

        return hitFo;
    }


    public RaycastHit Degree45RayFrontAndBack(Vector3 origin, float raySize)
    {
        RaycastHit hitFo;

        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.forward), out hitFo, raySize))
        {
            return hitFo;
        }
        else if (Physics.Raycast(origin, transform.TransformDirection(-Vector3.forward), out hitFo, raySize))
        {
            return hitFo;
        }
        else if (Physics.Raycast(origin, transform.TransformDirection(1.5f, 0, 1), out hitFo, raySize))
        {
            return hitFo;
        }
        else if (Physics.Raycast(origin, transform.TransformDirection(-1.5f, 0, 1), out hitFo, raySize))
        {
            return hitFo;
        }
        else if (Physics.Raycast(origin, -transform.TransformDirection(1.5f, 0, 1), out hitFo, raySize))
        {
            return hitFo;
        }
        else if (Physics.Raycast(origin, -transform.TransformDirection(-1.5f, 0, 1), out hitFo, raySize))
        {
            return hitFo;
        }
        return hitFo;
    }

    public RaycastHit DownRay(Vector3 origin, float raySize)
    {
        RaycastHit hitFo;

        if (Physics.Raycast(origin, transform.TransformDirection(Vector3.up * 0.3f + Vector3.down), out hitFo, raySize))
        {
            return hitFo;
        }
        return hitFo;
    }


}
