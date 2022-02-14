using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmatureModifier : MonoBehaviour
{
    [SerializeField]
    private Transform weaponArmBone;

    [SerializeField]
    private Transform weaponForeArmBone;

    [SerializeField]
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        Quaternion b = Camera.main.transform.rotation * Quaternion.Euler(0f, -90f, 180f);
        weaponArmBone.rotation = Quaternion.Lerp(weaponArmBone.rotation, b, 1f);
    }
}
