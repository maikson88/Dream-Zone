using UnityEngine;

public class MovingSphere : MonoBehaviour {

	public InputHandler Input { get; private set; }

	[SerializeField]
	Transform playerInputSpace = default, ball = default;

	[SerializeField, Range(0f, 100f)]
	float maxSpeed = 10f, maxClimbSpeed = 4f, maxSwimSpeed = 5f;

	[SerializeField, Range(0f, 100f)]
	float
		maxAcceleration = 10f,
		maxAirAcceleration = 1f,
		maxClimbAcceleration = 40f,
		maxSwimAcceleration = 5f;

	[SerializeField, Range(0f, 10f)]
	float jumpHeight = 2f;

	[SerializeField, Range(0, 5)]
	int maxAirJumps = 0;

	[SerializeField, Range(0, 90)]
	float maxGroundAngle = 25f, maxStairsAngle = 50f;

	[SerializeField, Range(90, 170)]
	float maxClimbAngle = 140f;

	[SerializeField, Range(0f, 100f)]
	float maxSnapSpeed = 100f;

	[SerializeField, Min(0f)]
	float probeDistance = 1f;

	[SerializeField]
	float submergenceOffset = 0.5f;

	[SerializeField, Min(0.1f)]
	float submergenceRange = 1f;

	[SerializeField, Min(0f)]
	float buoyancy = 1f;

	[SerializeField, Range(0f, 10f)]
	float waterDrag = 1f;

	[SerializeField, Range(0.01f, 1f)]
	float swimThreshold = 0.5f;

	[SerializeField]
	LayerMask probeMask = -1, stairsMask = -1, climbMask = -1, waterMask = 0;

	[SerializeField]
	Material
		normalMaterial = default,
		climbingMaterial = default,
		swimmingMaterial = default;

	[SerializeField, Min(0.1f)]
	float ballRadius = 0.5f;

	[SerializeField, Min(0f)]
	float ballAlignSpeed = 180f;

	[SerializeField, Min(0f)]
	float
		ballAirRotation = 0.5f,
		ballSwimRotation = 2f;

	Rigidbody body, connectedBody, previousConnectedBody;

	Vector3 playerInput;

	Vector3 velocity, connectionVelocity;

	Vector3 connectionWorldPosition, connectionLocalPosition;
	
	Vector3 upAxis, rightAxis, forwardAxis;

	bool desiredJump, desiresClimbing;

	Vector3 contactNormal, steepNormal, climbNormal, lastClimbNormal;

	Vector3 lastContactNormal, lastSteepNormal, lastConnectionVelocity;

	int groundContactCount, steepContactCount, climbContactCount;

	bool OnGround => groundContactCount > 0;

	bool OnSteep => steepContactCount > 0;

	bool Climbing => climbContactCount > 0 && stepsSinceLastJump > 2;

	bool InWater => submergence > 0f;

	bool Swimming => submergence >= swimThreshold;

	float submergence;

	int jumpPhase;

	float minGroundDotProduct, minStairsDotProduct, minClimbDotProduct;

	int stepsSinceLastGrounded, stepsSinceLastJump;

	MeshRenderer meshRenderer;

	public void PreventSnapToGround () {
		stepsSinceLastJump = -1;
	}

	void OnValidate () {
		minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
		minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
		minClimbDotProduct = Mathf.Cos(maxClimbAngle * Mathf.Deg2Rad);
	}

	void Awake () {
		body = GetComponent<Rigidbody>();
		body.useGravity = false;
		meshRenderer = ball.GetComponent<MeshRenderer>();
		Input = GetComponent<InputHandler>();
		OnValidate();
	}

	void Update () {
		playerInput.x = Input.NormalizedMovementInput.x;
		playerInput.z = Input.NormalizedMovementInput.y;
		//playerInput.y = Swimming ? Input.GetAxis("UpDown") : 0f;
		playerInput = Vector3.ClampMagnitude(playerInput, 1f);

		if (playerInputSpace) {
			//rightAxis = ProjectDirectionOnPlane(playerInputSpace.right, upAxis);
			//forwardAxis =
			//	ProjectDirectionOnPlane(playerInputSpace.forward, upAxis);
		}
		else {
			rightAxis = ProjectDirectionOnPlane(Vector3.right, upAxis);
			forwardAxis = ProjectDirectionOnPlane(Vector3.forward, upAxis);
		}

		if (Swimming) {
			desiresClimbing = false;
		}
		else {
			desiredJump |= Input.JumpInput;
			desiresClimbing = Input.ActionInput;
		}

		UpdateBall();
	}

	void UpdateBall () {
		Material ballMaterial = normalMaterial;
		Vector3 rotationPlaneNormal = lastContactNormal;
		float rotationFactor = 1f;
		if (Climbing) {
			ballMaterial = climbingMaterial;
		}
		else if (Swimming) {
			ballMaterial = swimmingMaterial;
			rotationFactor = ballSwimRotation;
		}
		else if (!OnGround) {
			if (OnSteep) {
				rotationPlaneNormal = lastSteepNormal;
			}
			else {
				rotationFactor = ballAirRotation;
			}
		}
		meshRenderer.material = ballMaterial;

		Vector3 movement =
			(body.velocity - lastConnectionVelocity) * Time.deltaTime;
		movement -=
			rotationPlaneNormal * Vector3.Dot(movement, rotationPlaneNormal);

		float distance = movement.magnitude;

		Quaternion rotation = ball.localRotation;
		if (connectedBody && connectedBody == previousConnectedBody) {
			rotation = Quaternion.Euler(
				connectedBody.angularVelocity * (Mathf.Rad2Deg * Time.deltaTime)
			) * rotation;
			if (distance < 0.001f) {
				ball.localRotation = rotation;
				return;
			}
		}
		else if (distance < 0.001f) {
			return;
		}

		float angle = distance * rotationFactor * (180f / Mathf.PI) / ballRadius;
		Vector3 rotationAxis =
			Vector3.Cross(rotationPlaneNormal, movement).normalized;
		rotation = Quaternion.Euler(rotationAxis * angle) * rotation;
		if (ballAlignSpeed > 0f) {
			rotation = AlignBallRotation(rotationAxis, rotation, distance);
		}
		ball.localRotation = rotation;
	}

	Quaternion AlignBallRotation (
		Vector3 rotationAxis, Quaternion rotation, float traveledDistance
	) {
		Vector3 ballAxis = ball.up;
		float dot = Mathf.Clamp(Vector3.Dot(ballAxis, rotationAxis), -1f, 1f);
		float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
		float maxAngle = ballAlignSpeed * traveledDistance;

		Quaternion newAlignment =
			Quaternion.FromToRotation(ballAxis, rotationAxis) * rotation;
		if (angle <= maxAngle) {
			return newAlignment;
		}
		else {
			return Quaternion.SlerpUnclamped(
				rotation, newAlignment, maxAngle / angle
			);
		}
	}

	void FixedUpdate () {
		//Core Early PhysX (run first)
		Vector3 gravity = CustomGravity.GetGravity(body.position, out upAxis);
		//State DoChecks (run second)
		UpdateState();

		//??
		if (InWater) {
			velocity *= 1f - waterDrag * submergence * Time.deltaTime;
		}

		//State PhysX
		AdjustVelocity();

		//States
		if (desiredJump) {
			desiredJump = false;
			Jump(gravity);
		}

		if (Climbing) {
			velocity -=
				contactNormal * (maxClimbAcceleration * 0.9f * Time.deltaTime);
		}
		else if (InWater) {
			velocity +=
				gravity * ((1f - buoyancy * submergence) * Time.deltaTime);
		}
		else if (OnGround && velocity.sqrMagnitude < 0.01f) {
			velocity +=
				contactNormal *
				(Vector3.Dot(gravity, contactNormal) * Time.deltaTime);
		}
		else if (desiresClimbing && OnGround) {
			velocity +=
				(gravity - contactNormal * (maxClimbAcceleration * 0.9f)) *
				Time.deltaTime;
		}
		else {
			velocity += gravity * Time.deltaTime;
		}

		//State Physx update end
		body.velocity = velocity;
		ClearState();
	}

	void ClearState () {
		lastContactNormal = contactNormal;
		lastSteepNormal = steepNormal;
		lastConnectionVelocity = connectionVelocity;
		groundContactCount = steepContactCount = climbContactCount = 0;
		contactNormal = steepNormal = climbNormal = Vector3.zero;
		connectionVelocity = Vector3.zero;
		previousConnectedBody = connectedBody;
		connectedBody = null;
		submergence = 0f;
	}

	//DoChecks // State INSIDE FIXEDUPDATE
	void UpdateState () {
		//State Dochecks
		stepsSinceLastGrounded += 1;
		stepsSinceLastJump += 1;
		velocity = body.velocity;
		//States Enter
		if (
			CheckClimbing() || CheckSwimming() ||
			OnGround || SnapToGround() || CheckSteepContacts()
		) {
			stepsSinceLastGrounded = 0;
			if (stepsSinceLastJump > 1) {
				jumpPhase = 0;
			}
			if (groundContactCount > 1) {
				contactNormal.Normalize();
			}
		}
		else {
			contactNormal = upAxis;
		}
		
		if (connectedBody) {
			if (connectedBody.isKinematic || connectedBody.mass >= body.mass) {
				UpdateConnectionState();
			}
		}
	}

	void UpdateConnectionState () {
		if (connectedBody == previousConnectedBody) {
			Vector3 connectionMovement =
				connectedBody.transform.TransformPoint(connectionLocalPosition) -
				connectionWorldPosition;
			connectionVelocity = connectionMovement / Time.deltaTime;
		}
		connectionWorldPosition = body.position;
		connectionLocalPosition = connectedBody.transform.InverseTransformPoint(
			connectionWorldPosition
		);
	}

	bool CheckClimbing () {
		if (Climbing) {
			if (climbContactCount > 1) {
				climbNormal.Normalize();
				float upDot = Vector3.Dot(upAxis, climbNormal);
				if (upDot >= minGroundDotProduct) {
					climbNormal = lastClimbNormal;
				}
			}
			groundContactCount = 1;
			contactNormal = climbNormal;
			return true;
		}
		return false;
	}

	bool CheckSwimming () {
		if (Swimming) {
			groundContactCount = 0;
			contactNormal = upAxis;
			return true;
		}
		return false;
	}

	bool SnapToGround () {
		if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2 || InWater) {
			return false;
		}
		float speed = velocity.magnitude;
		if (speed > maxSnapSpeed) {
			return false;
		}
		if (!Physics.Raycast(
			body.position, -upAxis, out RaycastHit hit,
			probeDistance, probeMask, QueryTriggerInteraction.Ignore
		)) {
			return false;
		}

		float upDot = Vector3.Dot(upAxis, hit.normal);
		if (upDot < GetMinDot(hit.collider.gameObject.layer)) {
			return false;
		}

		groundContactCount = 1;
		contactNormal = hit.normal;
		float dot = Vector3.Dot(velocity, hit.normal);
		if (dot > 0f) {
			velocity = (velocity - hit.normal * dot).normalized * speed;
		}
		connectedBody = hit.rigidbody;
		return true;
	}

	bool CheckSteepContacts () {
		if (steepContactCount > 1) {
			steepNormal.Normalize();
			float upDot = Vector3.Dot(upAxis, steepNormal);
			if (upDot >= minGroundDotProduct) {
				steepContactCount = 0;
				groundContactCount = 1;
				contactNormal = steepNormal;
				return true;
			}
		}
		return false;
	}

	void AdjustVelocity () {
		float acceleration, speed;
		Vector3 xAxis, zAxis;
		//state
		if (Climbing) {
			acceleration = maxClimbAcceleration;
			speed = maxClimbSpeed;
			xAxis = Vector3.Cross(contactNormal, upAxis);
			zAxis = upAxis;
		}
		else if (InWater) {
			float swimFactor = Mathf.Min(1f, submergence / swimThreshold);
			acceleration = Mathf.LerpUnclamped(
				OnGround ? maxAcceleration : maxAirAcceleration,
				maxSwimAcceleration, swimFactor
			);
			speed = Mathf.LerpUnclamped(maxSpeed, maxSwimSpeed, swimFactor);
			xAxis = rightAxis;
			zAxis = forwardAxis;
		}
		else {
			acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
			speed = OnGround && desiresClimbing ? maxClimbSpeed : maxSpeed;
			xAxis = rightAxis;
			zAxis = forwardAxis;
		}
		//Physics
		xAxis = ProjectDirectionOnPlane(xAxis, contactNormal);
		zAxis = ProjectDirectionOnPlane(zAxis, contactNormal);

		Vector3 relativeVelocity = velocity - connectionVelocity;

		Vector3 adjustment;
		adjustment.x =
			playerInput.x * speed - Vector3.Dot(relativeVelocity, xAxis);
		adjustment.z =
			playerInput.z * speed - Vector3.Dot(relativeVelocity, zAxis);
		adjustment.y = Swimming ?
			playerInput.y * speed - Vector3.Dot(relativeVelocity, upAxis) : 0f;

		adjustment =
			Vector3.ClampMagnitude(adjustment, acceleration * Time.deltaTime);

		velocity += xAxis * adjustment.x + zAxis * adjustment.z;
		if (Swimming) {
			velocity += upAxis * adjustment.y;
		}
	}

	void Jump (Vector3 gravity) {
		Vector3 jumpDirection;
		if (OnGround) {
			jumpDirection = contactNormal;
		}
		else if (OnSteep) {
			jumpDirection = steepNormal;
			jumpPhase = 0;
		}
		else if (maxAirJumps > 0 && jumpPhase <= maxAirJumps) {
			if (jumpPhase == 0) {
				jumpPhase = 1;
			}
			jumpDirection = contactNormal;
		}
		else {
			return;
		}

		stepsSinceLastJump = 0;
		jumpPhase += 1;
		float jumpSpeed = Mathf.Sqrt(2f * gravity.magnitude * jumpHeight);
		if (InWater) {
			jumpSpeed *= Mathf.Max(0f, 1f - submergence / swimThreshold);
		}
		jumpDirection = (jumpDirection + upAxis).normalized;
		float alignedSpeed = Vector3.Dot(velocity, jumpDirection);
		if (alignedSpeed > 0f) {
			jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
		}
		velocity += jumpDirection * jumpSpeed;
	}

	void OnCollisionEnter (Collision collision) {
		EvaluateCollision(collision);
	}

	void OnCollisionStay (Collision collision) {
		EvaluateCollision(collision);
	}

	void EvaluateCollision (Collision collision) {
		if (Swimming) {
			return;
		}
		int layer = collision.gameObject.layer;
		float minDot = GetMinDot(layer);
		for (int i = 0; i < collision.contactCount; i++) {
			Vector3 normal = collision.GetContact(i).normal;
			float upDot = Vector3.Dot(upAxis, normal);
			if (upDot >= minDot) 
			{
				groundContactCount += 1;
				contactNormal += normal;
				connectedBody = collision.rigidbody;
			}
			else {
				if (upDot > -0.01f) 
				{
					steepContactCount += 1;
					steepNormal += normal;
					if (groundContactCount == 0) 
					{
						connectedBody = collision.rigidbody;
					}
				}
				if (desiresClimbing && upDot >= minClimbDotProduct && (climbMask & (1 << layer)) != 0) 
				{
					climbContactCount += 1;
					climbNormal += normal;
					lastClimbNormal = normal;
					connectedBody = collision.rigidbody;
				}
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if ((waterMask & (1 << other.gameObject.layer)) != 0) {
			EvaluateSubmergence(other);
		}
	}

	void OnTriggerStay (Collider other) {
		if ((waterMask & (1 << other.gameObject.layer)) != 0) {
			EvaluateSubmergence(other);
		}
	}

	void EvaluateSubmergence (Collider collider) {
		if (Physics.Raycast(
			body.position + upAxis * submergenceOffset,
			-upAxis, out RaycastHit hit, submergenceRange + 1f,
			waterMask, QueryTriggerInteraction.Collide
		)) {
			submergence = 1f - hit.distance / submergenceRange;
		}
		else {
			submergence = 1f;
		}
		if (Swimming) {
			connectedBody = collider.attachedRigidbody;
		}
	}

	Vector3 ProjectDirectionOnPlane (Vector3 direction, Vector3 normal) {
		return (direction - normal * Vector3.Dot(direction, normal)).normalized;
	}

	float GetMinDot (int layer) {
		return (stairsMask & (1 << layer)) == 0 ?
			minGroundDotProduct : minStairsDotProduct;
	}
}
