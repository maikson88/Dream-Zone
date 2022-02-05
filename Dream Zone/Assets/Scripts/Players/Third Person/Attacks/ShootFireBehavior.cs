using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFireBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;
    [SerializeField]
    private float speed = 50f;
    [SerializeField]
    private float lifeTime = 3f;
    public bool hit;
    private Tools lifeTimeCount;

    public Vector3 target;

    private Rigidbody rb;

    private void Awake()
    {
        lifeTimeCount = new Tools();
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0.0f;
    }

    void Update()
    {
        if (lifeTimeCount.TimeHasPassed(lifeTime))
            ReturnToPool();

        if (!hit && Vector3.Distance(transform.position, target) < 0.1f)
            ReturnToPool();
    }

    private void FixedUpdate() => rb.velocity = (target - transform.position).normalized * speed;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        bulletDecal = FireDecalPool.Instance.GetFromPool();
        bulletDecal.transform.position = contact.point;
        bulletDecal.transform.rotation = Quaternion.LookRotation(contact.normal);
        ReturnToPool();
    }

    private void ReturnToPool() => ShootFirePool.Instance.AddToPool(gameObject);
    private void OnEnable() => lifeTimeCount.ResetTime();

    private void OnDisable()
    {
        transform.position = Vector3.zero;
        target = Vector3.zero;
    }

}
