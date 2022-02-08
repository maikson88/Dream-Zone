using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongWind : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 windDirection;
    public float windForce;

    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<Rigidbody>().AddForce(windDirection * windForce);
    }
}
