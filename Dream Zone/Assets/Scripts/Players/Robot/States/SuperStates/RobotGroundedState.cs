using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGroundedState : RobotState
{
    //protected Vector3 MovementInput;
    //protected bool JumpInput;
    //protected string SuperClass;
    //private bool isGrounded;

    //public RobotGroundedState(Robot robot, RobotStateMachine stateMachine, RobotData playerData, string animBoolName) : base(robot, stateMachine, playerData, animBoolName)
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
    //}

    //public override void Enter()
    //{
    //    base.Enter();
    //    robot.JumpState.ResetAmountOfJumpsLeft();
    //}

    //public override void Exit()
    //{
    //    base.Exit();
    //}

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();
    //    MovementInput = new Vector3(robot.Input.NormalizedInput.x, 0f, robot.Input.NormalizedInput.y);
    //    JumpInput = robot.Input.JumpInput;

        

    //    if (JumpInput && robot.JumpState.CanJump)
    //    {
    //        stateMachine.ChangeState(robot.JumpState);
    //    }
    //    else if (!isGrounded)
    //    {
    //        stateMachine.ChangeState(robot.InAirState);
    //    }
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //    core.Movement.SetMaxAcceleration(robotData.maxAcceleration);
    //    if (isGrounded)
    //        if (core.CollisionSenses.groundContactCount > 1)
    //        {
    //            core.CollisionSenses.contactNormal.Normalize();
    //        }
    //}
}
