using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    [SerializeField] GameObject MagicStaff;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 cameraDirection = Camera.main.transform.position - Camera.main.transform.forward;
        //transform.rotation = Quaternion.FromToRotation(MagicStaff.transform.forward, cameraDirection) * transform.rotation;

        //Quaternion b = Camera.main.transform.rotation * Quaternion.Euler(0f, -90f, 180f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, b, 1f);
    }

    private void LateUpdate()
    {
        Quaternion b = Camera.main.transform.rotation * Quaternion.Euler(0f, -90f, 180f);
        transform.rotation = Quaternion.Lerp(transform.rotation, b, 1f);
    }
}
