using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
//    public RobotStateMachine StateMachine { get; private set; }
//    public RobotIdleState IdleState { get; private set; }
//    public RobotMoveState MoveState { get; private set; }
//    public RobotInAirState InAirState { get; private set; }
//    public RobotJumpState JumpState { get; private set; }

//    [SerializeField]
//    public RobotData robotData;
//    public RobotInput Input { get; private set; }
//    public Animator Anim { get; private set; }
//    public Rigidbody RB { get; private set; }

//    public Core Core { get; private set; }

//    Vector3 velocity, desiredVelocity;

//    private string _previousState;
//    private bool _canLog;
//    public Vector3 xAxis = Vector3.right;

//    private void Awake()
//    {
//        Core = GetComponentInChildren<Core>();
//        StateMachine = new RobotStateMachine();
//        IdleState = new RobotIdleState(this, StateMachine, robotData, "idle");
//        MoveState = new RobotMoveState(this, StateMachine, robotData, "move");
//        JumpState = new RobotJumpState(this, StateMachine, robotData, "jump");
//        InAirState = new RobotInAirState(this, StateMachine, robotData, "inAir");
//    }

//    void Start()
//    {
//        RB = GetComponent<Rigidbody>();
//        Input = GetComponent<RobotInput>();
//        StateMachine.Initialize(IdleState);
//    }

//    //private void Update()
//    //{
//    //    Core.LogicUpdate();
//    //    StateMachine.CurrentState.LogicUpdate();
//    //}

//    //private void FixedUpdate()
//    //{
//    //    Core.PrePhysicsUpdate();
//    //    StateMachine.CurrentState.PhysicsUpdate();
//    //    Core.LatePhysicsUpdate();
//    //}

//    //void OnCollisionEnter(Collision collision)
//    //{
//    //    Core.CollisionSenses.EvaluateCollision(collision);
//    //}

//    //void OnCollisionStay(Collision collision)
//    //{
//    //    Core.CollisionSenses.EvaluateCollision(collision);
//    //}

//    //private void OnCollisionExit(Collision collision)
//    //{
//    //    Core.CollisionSenses.groundContactCount = 0;
//    //}

//#if UNITY_EDITOR
//    void OnGUI()
//    {
//        string parentState = "SUPERSTATE  " + StateMachine.CurrentState.ParentToString();
//        GUI.Box(new Rect(0, 0, 240, 25), parentState);

//        string currentState = "SUBSTATE  " + StateMachine.CurrentState.ToString();
//        GUI.Box(new Rect(245, 0, 200, 25), currentState);

//        if (GUI.Button(new Rect(0, Screen.height - 25, 200, 25), $"Create States Log  {_canLog}")) _canLog = !_canLog;

//        if (_previousState != currentState && _canLog) Debug.Log(parentState + " : " + currentState);

//        _previousState = currentState;
//    }
//#endif

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.blue;
//        Gizmos.DrawRay(transform.position, Vector3.forward);
//        Gizmos.color = Color.red;
//        Gizmos.DrawRay(transform.position, Vector3.right);

//        //ProjectOnContactPlane();
//    }
//    //private Vector3 ProjectOnContactPlane(Vector3 vector)
//    //{
//    //    //float scalar = Vector3.Dot(vector, Core.CollisionSenses.contactNormal);
//    //    //Vector3 projectedDirection = Core.CollisionSenses.contactNormal * scalar;
//    //    //return vector - projectedDirection;
//    //}
}
