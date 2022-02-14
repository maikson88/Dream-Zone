using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : MonoBehaviour
{
    [SerializeField] private float timesPerSec = 50f;
    [SerializeField] GameObject elementObject;

    void Update()
    {
        elementObject.transform.Rotate(0, 0, timesPerSec * Time.deltaTime);
    }
}
