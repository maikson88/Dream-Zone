using UnityEngine;
using Cinemachine;
using System;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimCinemachine;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private int cameraPriority;
    [SerializeField] private Canvas aimCanvas;
    private int defaultPriority;


    private void Awake()
    {
        defaultPriority = aimCinemachine.Priority;
        aimCanvas.enabled = false;
    }

    private void Update()
    {
        if (inputHandler.AimInput)
        {
            StartAim();
            aimCanvas.enabled = true;
        }
        if (!inputHandler.AimInput)
        {
            CancelAim();
            aimCanvas.enabled = false;
        }
    }

    private void StartAim() => aimCinemachine.Priority = 11;

    private void CancelAim() => aimCinemachine.Priority = defaultPriority;
}
