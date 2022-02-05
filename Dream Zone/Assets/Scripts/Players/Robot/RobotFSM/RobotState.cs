using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotState 
{
    //protected Core core;

    //protected Robot robot;
    //protected RobotStateMachine stateMachine;
    //protected RobotData robotData;

    //protected bool isAnimationFinished;
    //protected bool isExitingState;

    //protected float startTime;

    //private string animBoolName;

    //public RobotState(Robot robot, RobotStateMachine stateMachine, RobotData playerData, string animBoolName)
    //{
    //    this.robot = robot;
    //    this.stateMachine = stateMachine;
    //    this.robotData = playerData;
    //    this.animBoolName = animBoolName;
    //    core = robot.Core;
    //}

    //public virtual void Enter()
    //{
    //    EarlyPhysicsUpdate();
    //    //robot.Anim.SetBool(animBoolName, true);
    //    startTime = Time.time;
    //    //Debug.Log(animBoolName);
    //    isAnimationFinished = false;
    //    isExitingState = false;
    //}

    //public virtual void Exit()
    //{
    //    //robot.Anim.SetBool(animBoolName, false);
    //    isExitingState = true;
    //}

    //public virtual void LogicUpdate()
    //{
    //    //if (isExitingState) return;
    //}
    //public virtual void EarlyPhysicsUpdate() 
    //{
    //    core.Movement.Velocity = core.Movement.RB.velocity;
    //}

    //public virtual void PhysicsUpdate()
    //{
    //    EarlyPhysicsUpdate();
    //    core.Movement.AdjustVelocity();
    //}


    //public virtual void AnimationTrigger() { }

    //public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

    //public virtual string ParentToString()
    //{
    //    Type superClass = GetType().BaseType;
    //    return superClass.Name;
    //}
}
