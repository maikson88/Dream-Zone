using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerVfxController : MonoBehaviour
{
    [SerializeField]
    private GameObject vfxSuperJump;
    [SerializeField]
    private GameObject vfxShockParticle;
    [SerializeField]
    private CinemachineImpulseSource superJumpImpulseSource;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vfxSuperJump.activeSelf) GenerateCameraShake(superJumpImpulseSource);
    }

    void GenerateCameraShake(CinemachineImpulseSource source)
    {
        source.GenerateImpulse();
    }

    public void SetVfxSuperJump(bool isActive) => vfxSuperJump.SetActive(isActive);
    public void SetvfxShockParticle(bool isActive) => vfxShockParticle.SetActive(isActive);
}
