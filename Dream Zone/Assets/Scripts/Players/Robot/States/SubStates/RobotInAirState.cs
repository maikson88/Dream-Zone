using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInAirState : RobotState
{
    //protected Vector3 MovementInput;
    //public bool isJumping { get; private set; }
    //protected bool JumpInput;
    //protected bool JumpInputStop;
    //private bool isGrounded;

    //public RobotInAirState(Robot robot, RobotStateMachine stateMachine, RobotData playerData, string animBoolName) : base(robot, stateMachine, playerData, animBoolName)
    //{
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
    //    isGrounded = core.CollisionSenses.OnGround;
    //    core.CollisionSenses.contactNormal = Vector3.up;
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
    //    MovementInput = new Vector3(robot.Input.NormalizedInput.x, 0f, robot.Input.NormalizedInput.y);
    //    core.Movement.SetDesiredVelocity(MovementInput * robotData.maxAirAcceleration);
    //    JumpInput = robot.Input.JumpInput;
    //    JumpInputStop = robot.Input.JumpInputStop;

    //    CheckJumpMultiplier();

    //    if (isGrounded && core.Movement.Velocity.y < 0.01f && !isJumping)
    //    {
    //        stateMachine.ChangeState(robot.IdleState);
    //    }

    //    /*else*/
    //    if (JumpInput && robot.JumpState.CanJump)
    //    {
    //        stateMachine.ChangeState(robot.JumpState);
    //    }
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //    core.Movement.SetMaxAcceleration(robotData.maxAirAcceleration);
    //}

    //private void CheckJumpMultiplier()
    //{
    //    if (isJumping)
    //    {
    //        if (JumpInputStop)
    //        {
    //            core.Movement.SetVelocityY(core.Movement.Velocity.y * robotData.variableJumpHeightMultiplier);
    //            isJumping = false;
    //        }
    //        else if (core.Movement.Velocity.y <= 0f)
    //        {
    //            isJumping = false;
    //        }
    //    }
    //}

    //public void SetIsJumping(bool isJumping) => this.isJumping = isJumping;
}
