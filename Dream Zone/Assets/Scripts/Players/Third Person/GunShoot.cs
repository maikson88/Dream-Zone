using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [SerializeField] private PlayerCore playerCore;
    private GameObject _instance;
    private RaycastHit hit;
    private Tools shootBuffer;
    [SerializeField]
    private float shootBufferTime = 2f;
    [SerializeField]
    private float bulletHitMissDistance = 100f;
    private GameObject spawnPoint;
    private bool buffering;
    ShootFireBehavior shootFireBehavior;

    void Start()
    {
        shootBuffer = new Tools();
        spawnPoint = gameObject.transform.GetChild(0).gameObject;

    }

    void Update()
    {
        if (buffering)
            if (shootBuffer.TimeHasPassed(shootBufferTime))
                buffering = false;

        if (playerCore.playerController.playerInput.AttackInput && !buffering)
        {
            if(_instance == null)
            {
            buffering = true;
            Trigger();
            }
        }
    }

    private void Trigger()
    {
        playerCore.playerController.anim.SetTrigger("Shooting");
        playerCore.animEvents.isShooting = true;
        GetFromPool();
        //Make it an interface so it triggers any behavior if it does have any
        shootFireBehavior = _instance.GetComponent<ShootFireBehavior>();

        if (Physics.Raycast(playerCore.cameraTransform.position, playerCore.cameraTransform.forward, out hit, Mathf.Infinity))
        {
            if (!hit.collider.CompareTag("Player"))
                HitShot();
            else
                HitMiss();
        }
        else
            HitMiss();
    }

    private void HitShot()
    {
        Debug.Log(string.Concat(" TARGET FOUND :   ", hit.collider.name));
        shootFireBehavior.target = hit.point;
        shootFireBehavior.hit = true;
        _instance = null;
    }

    private void HitMiss()
    {
        shootFireBehavior.target = playerCore.cameraTransform.position + playerCore.cameraTransform.forward * bulletHitMissDistance;
        shootFireBehavior.hit = false;
        _instance = null;
    }

    private void GetFromPool()
    {
        _instance = ShootFirePool.Instance.GetFromPool();
        _instance.transform.position = spawnPoint.transform.position;
    }
}
