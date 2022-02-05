using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotJumpState : RobotAbilityState
{
    //public int amountOfJumpsLeft;
    //public float jumpSpeed;
    //private float alignedSpeed;

    //public RobotJumpState(Robot robot, RobotStateMachine stateMachine, RobotData playerData, string animBoolName) : base(robot, stateMachine, playerData, animBoolName)
    //{
    //    amountOfJumpsLeft = playerData.maxJumps;
    //}

    //public override void AnimationFinishTrigger()
    //{
    //    base.AnimationFinishTrigger();
    //}

    //public override void AnimationTrigger()
    //{
    //    base.AnimationTrigger();
    //}

    //public override void EarlyPhysicsUpdate()
    //{
    //    base.EarlyPhysicsUpdate();
    //    if(!isGrounded) core.CollisionSenses.contactNormal = Vector3.up;
    //}

    //public override void Enter()
    //{
    //    base.Enter();

    //}

    //public override void Exit()
    //{
    //    base.Exit();
    //}

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //    ApplyJump();
    //    core.LatePhysicsUpdate();
    //    robot.Input.UseJumpInput();
    //    robot.InAirState.SetIsJumping(true);
    //    amountOfJumpsLeft--;
    //    SetAbilityDone();
    //}

    //public void ApplyJump()
    //{
    //    jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * robotData.jumpHeight);
    //    alignedSpeed = Vector3.Dot(core.Movement.Velocity, core.CollisionSenses.contactNormal);
    //    if (alignedSpeed > 0f)
    //    {
    //        jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
    //    }
    //    core.Movement.SetVelocity(core.Movement.Velocity + jumpSpeed * core.CollisionSenses.contactNormal);
    //}

    //public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = robotData.maxJumps;

    //public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;

    //public void SetAbilityDone() => isAbilityDone = true;

    //public bool CanJump => amountOfJumpsLeft > 0;
}
