using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacter : MonoBehaviour
{
	//[SerializeField, Range(0f, 100f)]
	//float maxSpeed = 10f;

	//[SerializeField, Range(0f, 100f)]
	//float maxAcceleration = 10f, maxAirAcceleration = 1f;

	//[SerializeField, Range(0f, 10f)]
	//float jumpHeight = 2f;

	//[SerializeField, Range(0, 5)]
	//int maxAirJumps = 0;

	//[SerializeField, Range(0, 90)]
	//float maxGroundAngle = 25f;

	//Rigidbody body;

	//public Vector3 velocity, desiredVelocity;

	//public Vector3 contactNormal;

	//bool desiredJump;

	//int groundContactCount;

	//bool OnGround => groundContactCount > 0;

	//int jumpPhase;

	//float minGroundDotProduct;

	//public Robot robot;

	//void OnValidate()
	//{
	//	minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
	//}

	//void Awake()
	//{
	//	body = GetComponent<Rigidbody>();
	//	OnValidate();
	//}

	//void Update()
	//{
	//	Vector2 playerInput;
	//	playerInput.x = robot.Input.RawMovementInput.x;
	//	playerInput.y = robot.Input.RawMovementInput.y;
	//	playerInput = Vector2.ClampMagnitude(playerInput, 1f);

	//	desiredVelocity =
	//		new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

	//	desiredJump |= robot.Input.JumpInput;
	//}

	//void FixedUpdate()
	//{
	//	UpdateState();
	//	AdjustVelocity();

	//	if (desiredJump)
	//	{
	//		desiredJump = false;
	//		Jump();
	//	}

	//	body.velocity = velocity;
	//	ClearState();
	//}

	//void ClearState()
	//{
	//	groundContactCount = 0;
	//	contactNormal = Vector3.zero;
	//}

	//void UpdateState()
	//{
	//	velocity = body.velocity;
	//	if (OnGround)
	//	{
	//		jumpPhase = 0;
	//		if (groundContactCount > 1)
	//		{
	//			contactNormal.Normalize();
	//		}
	//	}
	//	else
	//	{
	//		contactNormal = Vector3.up;
	//	}
	//}

	//void AdjustVelocity()
	//{
	//	Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
	//	Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

	//	float currentX = Vector3.Dot(velocity, xAxis);
	//	float currentZ = Vector3.Dot(velocity, zAxis);

	//	float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
	//	float maxSpeedChange = acceleration * Time.deltaTime;

	//	float newX =
	//		Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
	//	float newZ =
	//		Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

	//	velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
	//}

	//void Jump()
	//{
	//	if (OnGround || jumpPhase < maxAirJumps)
	//	{
	//		jumpPhase += 1;
	//		float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
	//		float alignedSpeed = Vector3.Dot(velocity, contactNormal);
	//		if (alignedSpeed > 0f)
	//		{
	//			jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
	//		}
	//		velocity += contactNormal * jumpSpeed;
	//	}
	//}

	//void OnCollisionEnter(Collision collision)
	//{
	//	EvaluateCollision(collision);
	//}

	//void OnCollisionStay(Collision collision)
	//{
	//	EvaluateCollision(collision);
	//}

	//void EvaluateCollision(Collision collision)
	//{
	//	for (int i = 0; i < collision.contactCount; i++)
	//	{
	//		Vector3 normal = collision.GetContact(i).normal;
	//		if (normal.y >= minGroundDotProduct)
	//		{
	//			groundContactCount += 1;
	//			contactNormal += normal;
	//		}
	//	}
	//}

	//Vector3 ProjectOnContactPlane(Vector3 vector)
	//{
	//	return vector - contactNormal * Vector3.Dot(vector, contactNormal);
	//}
}
