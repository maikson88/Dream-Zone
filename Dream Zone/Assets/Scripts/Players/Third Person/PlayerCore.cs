using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerCore : MonoBehaviour
{
    public Transform cameraTransform { get; private set; }
    public CollisionSenses collisionSenses { get; private set; }
    public PlayerVfxController playerVfx { get; private set; }
    public PlayerMovement playerMovement { get; private set; }
    public PlayerController playerController;
    public NinianeAnimEvents animEvents;
    public PlayerData playerData;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        collisionSenses = GetComponentInChildren<CollisionSenses>();
        playerVfx = GetComponentInChildren<PlayerVfxController>();
        playerMovement = GetComponent<PlayerMovement>();
    }
}
