using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRobotData", menuName = "Data/Robot Data/Base Robot Data")]
public class RobotData : ScriptableObject
{
    [Header("Movement")]
    [SerializeField, Range(0f, 100f)]
    public float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    public float maxAcceleration = 10f, maxAirAcceleration = 8f;

    [Header("Jump State")]
    public float jumpHeight = 10f;
    public int maxJumps = 2;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;
}
