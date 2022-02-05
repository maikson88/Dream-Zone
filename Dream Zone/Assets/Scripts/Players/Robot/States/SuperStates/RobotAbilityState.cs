using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAbilityState : RobotState
{
    //protected bool isAbilityDone;

    //protected bool isGrounded;
    //protected bool isJumping;

    //public RobotAbilityState(Robot robot, RobotStateMachine stateMachine, RobotData playerData, string animBoolName) : base(robot, stateMachine, playerData, animBoolName)
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
    //    isJumping = robot.InAirState.isJumping;
    //}

    //public override void Enter()
    //{
    //    base.Enter();
    //    isAbilityDone = false;
    //}

    //public override void Exit()
    //{
    //    base.Exit();
    //}

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();

    //    if (isAbilityDone)
    //    {
    //        if (isGrounded && core.Movement.Velocity.y < 0.01f && !isJumping)
    //        {
    //            stateMachine.ChangeState(robot.IdleState);
    //        }
    //        else
    //        {
    //            stateMachine.ChangeState(robot.InAirState);
    //        }
    //    }
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //}
}
