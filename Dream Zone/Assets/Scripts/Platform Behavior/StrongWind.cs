using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongWind : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 windDirection;
    public float windForce;
    public Vector3 playerDefault;

    private void OnTriggerStay(Collider other)
    {
        other.GetComponent<PlayerController>().playerCore.playerMovement.externalForce = windDirection * windForce;
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerController>().playerCore.playerMovement.externalForce = Vector3.zero;
    }
}
