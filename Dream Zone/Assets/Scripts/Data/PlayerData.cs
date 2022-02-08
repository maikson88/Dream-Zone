using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("InAir State")]
    public float maxFallVelocity = 30f;

    [Header("Jump")]
    public float jumpForce = 10f;

    [Header("Movement")]
    public float playerSpeed = 9f;
    public float rbVelocityMultiplier = 1f;
    public float maxSizeOfStairs = -1f;
}
