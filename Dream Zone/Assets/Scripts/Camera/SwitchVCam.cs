using UnityEngine;
using Cinemachine;
using System;

//[RequireComponent(typeof(InputHandler))]
public class SwitchVCam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    private int cameraPriority;
    private Canvas aimCanvas;
    private int defaultPriority;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        inputHandler = FindObjectOfType<InputHandler>();
        aimCanvas = GetComponentInChildren<Canvas>();
        defaultPriority = virtualCamera.Priority;
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

    private void StartAim() => virtualCamera.Priority = 11;


    private void CancelAim() => virtualCamera.Priority = defaultPriority;
}
