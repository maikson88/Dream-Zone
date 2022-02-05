using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    //public Rigidbody RB { get; private set; }
    //public float maxAcceleration { get; private set; }
    //public Vector3 Velocity;
    //public Vector3 DesiredVelocity { get; private set; }
    //private Vector3 workspace;

    //protected override void Awake()
    //{
    //    base.Awake();

    //    RB = GetComponentInParent<Rigidbody>();
    //}

    //public void EarlyLogicUpdate()
    //{

    //}

    //public void EarlyPhysicsUpdate()
    //{
        
    //}

    //public void LatePhysicsUpdate()
    //{
    //    RB.velocity = Velocity;
    //    core.CollisionSenses.ClearSenses();
    //}


    ///// <summary>
    ///// Adds weight over object movement
    ///// This Function Controls how much time until DesiredVelocity reaches final Velocity
    ///// </summary>
    //public void SetMaxAcceleration(float maxAcceleration)
    //{
    //    this.maxAcceleration = maxAcceleration;
    //}

    ///// <summary>
    ///// This Function ignores MaxAcceleration will immediately reach applied Velocity
    ///// </summary>
    ///// <param name="velocity"></param>
    //public void SetVelocity(Vector3 velocity)
    //{
    //    workspace.Set(velocity.x, velocity.y, velocity.z);
    //    Velocity = workspace;
    //}

    //public void SetVelocityX(float velocity)
    //{
    //    workspace.Set(velocity, Velocity.y, Velocity.z);
    //    Velocity = workspace;
    //}

    //public void SetVelocityY(float velocity)
    //{
    //    workspace.Set(Velocity.x, velocity, Velocity.z);
    //    Velocity = workspace;
    //}

    //public void SetVelocityZ(float velocity)
    //{
    //    workspace.Set(Velocity.x, Velocity.y, velocity);
    //    Velocity = workspace;
    //}

    ///// <summary>
    ///// This Function will make the velocity lags towards MaxAceleration that will be applied by the time the next frame occurs
    ///// </summary>
    ///// <param name="desiredVelocity"></param>
    //public void SetDesiredVelocity(Vector3 desiredVelocity) => this.DesiredVelocity = desiredVelocity;



    //public void AdjustVelocity()
    //{
    //    Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
    //    Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

    //    float currentX = Vector3.Dot(Velocity, xAxis);
    //    float currentZ = Vector3.Dot(Velocity, zAxis);

    //    float maxSpeedChange = maxAcceleration * Time.deltaTime;

    //    float newX =
    //        Mathf.MoveTowards(currentX, DesiredVelocity.x, maxSpeedChange);
    //    float newZ =
    //        Mathf.MoveTowards(currentZ, DesiredVelocity.z, maxSpeedChange);

    //    Velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    //}

    //Vector3 ProjectOnContactPlane(Vector3 vector)
    //{
    //    return vector - core.CollisionSenses.contactNormal * Vector3.Dot(vector, core.CollisionSenses.contactNormal);
    //}
}
