namespace Island.Actor
{
    using Island.Camera;
    using UnityEngine;

    public enum GroundState
    {
        Grounded,
        Falling,
        Raising
    }

    public enum ControllerState
    {
        Idle,
        Walking,
        Sprinting,
        Crouching,
        Sliding,
        Dashing,
        Strafing,
        Jumping
    }


    public class ActorBrain : MonoBehaviour
    {
        [Header("Global Options :")]
        [SerializeField] private bool _lockInput;
        [SerializeField] private bool _turnInPlace; // NOT CORRECTLY IMPLEMENTED YET!
        [SerializeField] private bool _allowJumping = true;

        [Header("External Forces: ")]
        [SerializeField] private float _gravityForce;
        [SerializeField] private float _airResistance;
        [SerializeField] private float _groundFriction;

        [Header("Controller Variables :")]
        private GroundState _groundState;
        private ControllerState _controllerState;
        private Vector3 _newVelocity, _currentVelocity;
        private Vector3 _newDirection, _currentDirection;
        private Quaternion _rotation;
        private Vector3 _momentum;
        private float _acceleration;
        private float _desiredSpeed;

        private bool _isBreaking;
        private bool _isTurning;
        private bool _isJumping;

        [Header("Camera Variables :")]
        [SerializeField] private bool _useCameraDirection;
        [SerializeField] private CameraHandler _cameraHandler;

        [Header("Actor Variables :")]
        [SerializeField] private Actor _actor;

        #region Getters
        internal Vector3 NewVelocity => _newVelocity;
        internal Vector3 Direction => _newDirection;
        internal float Acceleration => _acceleration;
        internal Vector3 Momentum => _momentum;
        internal GroundState GroundState => _groundState;
        #endregion

        private void Start()
        {
            // TESTING
            _controllerState = ControllerState.Walking;
            _groundState = GroundState.Grounded;

            //if (_useCameraDirection)
            //    _cameraHandler.MouseRotEnabled = true;
        }

        private void AssignActor(Actor ac) { _actor = ac; }

        private void FixedUpdate()
        {
            // Update the controller
            CheckInput();
            UpdateController();
        }

        private void CheckInput()
        {
            Vector3 dir = Vector3.zero;
            dir += transform.right * Input.GetAxisRaw("Horizontal");
            dir += transform.forward * Input.GetAxisRaw("Vertical");

            // JUST FOR TESTING
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _desiredSpeed = _actor.RunningForce;
                _controllerState = ControllerState.Sprinting;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                _desiredSpeed = _actor.CrouchingForce;
                _controllerState = ControllerState.Crouching;
            }
            else if (dir != Vector3.zero)
            {
                _desiredSpeed = _actor.WalkingForce;
                _controllerState = ControllerState.Walking;
            }
            else
            {
                _desiredSpeed = 0;
                _controllerState = ControllerState.Idle;
            }

            // TODO: Not yet working
            if (_allowJumping && Input.GetKey(KeyCode.Space) && !_isJumping && _groundState == GroundState.Grounded)
            {
                _isJumping = true;
            }
        }

        private void UpdateController()
        {
            // Only allow movement if we say so
            if (_lockInput || _actor == null) { return; }

            CheckControllerState();

            // Save the current velocity before changing it
            _currentVelocity = _newVelocity;
            _currentDirection = _newDirection;

            // Get the horizontal and vertical velocity
            _newDirection = GetDirection();

            // Apply acceleration if there is 
            _acceleration = CalculateAcceleration();

            // Calculate the new velocity we are supposed to get
            _newVelocity = CalculateVelocity(_newDirection);

            // Lerp to the desired velocity, should be based on friction
            //_newVelocity = _newVelocity.normalized * Mathf.Lerp(_currentVelocity.magnitude, _desiredSpeed, Time.deltaTime);

            // Calculate the rotation we are supposed to get
            _rotation = CalculateRotation(_newDirection);

            // Calculate the momentum
            //_momentum = CalculateMomentum();

            // Set the velocity of the assigned actor
            _actor.SetVelocity(_newVelocity);
            _actor.SetRotation(_rotation);
        }

        private Vector3 GetDirection()
        {
            // Get input for movement
            Vector3 inputDirection = GetInputDirection();

            // Enable or disable breaking based on input
            UpdateBreakingStatus(inputDirection);

            // Check if we have finished turning
            CheckTurningStatus(inputDirection);

            return inputDirection;
        }

        private Vector3 GetInputDirection()
        {
            Vector3 inputDirection = Vector3.zero;

            // Read input for horizontal and vertical movement
            inputDirection += transform.right * Input.GetAxisRaw("Horizontal");
            inputDirection += transform.forward * Input.GetAxisRaw("Vertical");

            // Add vertical input for jumping if allowed
            if (_allowJumping && Input.GetKey(KeyCode.Space))
            {
                inputDirection += transform.up;
            }

            return inputDirection;
        }

        private void UpdateBreakingStatus(Vector3 inputDirection)
        {
            // Enable or disable breaking based on input
            if (!_isBreaking && !_isTurning && _currentVelocity != Vector3.zero && Vector3.Dot(_currentVelocity.normalized, inputDirection) <= -0.99f)
            {
                _isBreaking = true;
            }
            else if (_isBreaking && _newVelocity.magnitude <= 0)
            {
                // Disable breaking if we have come to a stop
                _isBreaking = false;
                _isTurning = true;
                Debug.Log("Finished breaking");
            }
        }

        private void CheckTurningStatus(Vector3 inputDirection)
        {
            if (_isTurning)
            {
                if (_turnInPlace)
                {
                    // Check if we are rotated to the direction we want to go
                    float angleToTarget = Quaternion.Angle(_actor.transform.rotation, Quaternion.LookRotation(inputDirection));
                    if (angleToTarget < 5.0f)
                        _isTurning = false;
                }
                else if (Vector3.Dot(_currentVelocity.normalized, inputDirection) >= 0.99f)
                {
                    _isTurning = false;
                }
            }
        }

        private Vector3 CalculateVelocity(Vector3 dir)
        {
            Vector3 velocity = Vector3.zero;
            if (_turnInPlace && _isTurning)
                return velocity;

            /// Apply horizontal forces
            velocity += _actor.transform.forward * _acceleration;


            // Apply vertical forces
            if (_isJumping)
            {
                velocity += _actor.transform.up * _actor.JumpForce;
                if (velocity.y >= _actor.JumpForce)
                    _isJumping = false;
            }

            if (_groundState == GroundState.Falling && !_isJumping)
            {
                velocity -= _actor.transform.up * _gravityForce;
            }

            //if (_groundState == GroundState.Grounded)
            //{
            //    if (Input.GetKey(KeyCode.Space))
            //    {
            //        velocity += _actor.transform.up * _actor.JumpForce;
            //        //_groundState = GroundState.Raising;
            //    }
            //}
            //else if (_groundState == GroundState.Falling)
            //{
            //    velocity -= _actor.transform.up * _gravityForce;
            //}

            // Apply friction
            if (_groundState == GroundState.Grounded)
            {
                velocity += _currentVelocity * -_groundFriction;
            }
            else
            {
                velocity += _currentVelocity * -_airResistance;
            }

            //if (velocity.magnitude > 1f)
            //    velocity.Normalize();

            //velocity *= GetMaxSpeed();
            return velocity;
        }

        private Quaternion CalculateRotation(Vector3 dir)
        {
            dir.y = 0;
            if (dir == Vector3.zero || _isBreaking) return _actor.transform.rotation;

            Quaternion rot = Quaternion.identity;

            // Check if we want to move based on the camera it's direction
            if (_useCameraDirection)
            {
                Vector3 fwd = _cameraHandler.transform.forward;
                rot = Quaternion.LookRotation(fwd, Vector3.up);
                rot.x = 0;
                rot.z = 0;
                //throw new NotImplementedException();
            }
            else
            {
                rot = Quaternion.LookRotation(dir, Vector3.up);
            }
            return rot;
        }

        private float CalculateAcceleration()
        {
            // Only ways to accelerate is to: Change speed, change direction or both.

            // No need to apply acceleration if there is no change in direction and we are standing still
            if (_newDirection == Vector3.zero && _acceleration == 0) return 0;
            float maxSpeed = GetMovementForce();

            // Lower the acceleration first to the max speed
            if (_acceleration > maxSpeed)
            {
                // Decrease the acceleration first before snapping the value to the max
                _acceleration -= (_acceleration / _actor.DecelerationTime) * Time.deltaTime;
                return _acceleration;
            }

            if (_isBreaking)
            {
                // Make the entity stop
                _acceleration -= (maxSpeed / _actor.BreakTime) * Time.deltaTime;
                _acceleration = Mathf.Max(_acceleration, 0);
            }
            else if (_newDirection != Vector3.zero && _acceleration != maxSpeed)
            {
                // increase the acceleration and hold it at the max speed
                _acceleration += (maxSpeed / _actor.AccelerationTime) * Time.deltaTime;
                _acceleration = Mathf.Min(_acceleration, maxSpeed);
            }
            else if (_newDirection == Vector3.zero)
            {
                // Decrease over time
                _acceleration -= (maxSpeed / _actor.DecelerationTime);// * Time.deltaTime;
                _acceleration = Mathf.Max(_acceleration, 0);
            }

            // SHOULD BE DETERMINED BY (a = F / m)
            return _acceleration;
        }

        private Vector3 CalculateMomentum()
        {
            Vector3 verticalMomentum = Vector3.zero;
            Vector3 horizontalMomentum = Vector3.zero;

            // Apply gravity
            verticalMomentum -= _actor.transform.up * _gravityForce * Time.deltaTime;
            return horizontalMomentum + verticalMomentum;
        }

        private Vector3 CalculateDrag()
        {
            Vector3 drag = Vector3.zero;
            return drag;
        }

        private Vector3 CalculateFriction()
        {
            Vector3 drag = Vector3.zero;
            return drag;
        }

        private void CalculateNormalForces()
        {

        }

        private bool IsGrounded() => _actor.IsGrounded();

        private float GetMovementForce()
        {
            switch (_controllerState)
            {
                default:
                case ControllerState.Idle:
                    return 0;
                case ControllerState.Walking:
                    return _actor.WalkingForce;
                case ControllerState.Sprinting:
                    return _actor.RunningForce;
                case ControllerState.Crouching:
                    return _actor.CrouchingForce;
            }
        }

        private void CheckControllerState()
        {
            if (IsGrounded())
            {
                _groundState = GroundState.Grounded;
            }
            else
            {
                _groundState = GroundState.Falling;
            }
        }
    }
}