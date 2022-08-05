using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerState
{
    Grounded,
    Falling,
    Raising
}

public enum MovementState
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

public class ActorController : MonoBehaviour
{
    [Header("Global Options :")]
    [SerializeField] private bool _lockInput;
    [SerializeField] private bool _turnInPlace; // NOT CORRECTLY IMPLEMENTED YET!
    
    [Header("External Forces: ")]
    [SerializeField] private float _gravityForce;
    [SerializeField] private float _airResistance;
    [SerializeField] private float _groundFriction;

    [Header("Controller Variables :")]
    private ControllerState _controllerState;
    private MovementState _movementState;
    private Vector3 _newVelocity, _currentVelocity;
    private Vector3 _direction;
    private Quaternion _rotation;
    private Vector3 _momentum;
    private float _acceleration;
    
    private bool _isBreaking;
    private bool _isTurning;

    [Header("Camera Variables :")]
    [SerializeField] private bool _useCameraDirection;
    [SerializeField] private Camera _camera;

    [Header("Actor Variables :")]
    [SerializeField] private Actor _actor;

    private void Start()
    {
        // TESTING
        _movementState = MovementState.Walking;
        _controllerState = ControllerState.Grounded;
    }

    private void AssignActor(Actor ac) { _actor = ac; }

    private void FixedUpdate()
    {
        // Update the controller
        UpdateController();
        CheckInput();
    }

    private void CheckInput()
    {
        // JUST FOR TESTING
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _movementState = MovementState.Sprinting;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            _movementState = MovementState.Crouching;
        } else
        {
            _movementState = MovementState.Walking;
        }
    }

    private void UpdateController()
    {
        // Only allow movement if we say so
        if (_lockInput || _actor == null) { return; }

        // Save the current velocity before changing it
        _currentVelocity = _newVelocity;

        // Get the horizontal and vertical velocity
        _direction = GetDirection();

        // Apply acceleration if there is 
        _acceleration = CalculateAcceleration(_direction);

        // Calculate the new velocity we are supposed to get
        _newVelocity = CalculateVelocity();

        // Calculate the rotation we are supposed to get
        _rotation = CalculateRotation(_direction);

        // Calculate the momentum
        //_momentum = CalculateMomentum();

        // Set the velocity of the assigned actor
        _actor.SetVelocity(_newVelocity);
        _actor.SetRotation(_rotation);
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = Vector3.zero;

        // Check if we want to move based on the camera it's direction
        if (_useCameraDirection)
        {
            throw new NotImplementedException();
        }
        else
        {
            // TODO: Create a input system
            dir += transform.right * Input.GetAxisRaw("Horizontal");
            dir += transform.forward * Input.GetAxisRaw("Vertical");
        }

        // Enable the break if we go to the oppposite direction
        if (!_isBreaking && !_isTurning && _currentVelocity != Vector3.zero && Vector3.Dot(_currentVelocity.normalized, dir) <= -0.99f)
        {
            //Debug.Log("break: " + Vector3.Dot(_currentVelocity, dir));
            _isBreaking = true;
        }
        else if (_isBreaking && _newVelocity.magnitude <= 0)
        {
            // Disable the break if we have come to a normalizedstop
            _isBreaking = false;
            _isTurning = true;
        }

        if (_isTurning)
        {
            Debug.Log(Vector3.Dot(_currentVelocity.normalized, dir));
        }

        // Make sure we don't enable the breaking again if we are turning
        if (_isTurning && (Vector3.Dot(_currentVelocity.normalized, dir) >= 0.99f))
            _isTurning = false;

        return dir;
    }


    private Vector3 CalculateVelocity()
    {
        Vector3 velocity = Vector3.zero;

        if (_turnInPlace && _isTurning)
            return velocity;

        // Apply forward motion
        velocity += _actor.transform.forward * _acceleration;

        // Apply gravity
        if (_controllerState != ControllerState.Grounded)
            velocity -= _actor.transform.up * _gravityForce * Time.deltaTime;

        if (velocity.magnitude > 1f)
            velocity.Normalize();
        
        velocity *= GetMaxSpeed();
        return velocity;
    }

    private Quaternion CalculateRotation(Vector3 dir)
    {
        dir.y = 0;
        if (dir == Vector3.zero || _isBreaking) return _actor.transform.rotation;
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
        return rot;
    }

    private float CalculateAcceleration(Vector3 dir)
    {
        dir.y = 0;
        if (_acceleration == 0 && dir == Vector3.zero) return 0;
        float maxSpeed = GetMaxSpeed();
        
        if (_isBreaking)
        {
            // Break 
            _acceleration -= (maxSpeed / _actor.BreakTime) * Time.deltaTime;
            _acceleration = Mathf.Max(_acceleration, 0);
        } else if (dir.magnitude > 0)
        {
            // Increase acceleration
            if (_acceleration > maxSpeed)
            {
                // Decrease the acceleration first before snapping the value to the max
                _acceleration -= (maxSpeed / _actor.DecelerationTime) * Time.deltaTime;
            } else
            {
                // Hold the acceleration at the max speed
                _acceleration += (maxSpeed / _actor.AccelerationTime) * Time.deltaTime;
                _acceleration = Mathf.Min(_acceleration, maxSpeed);
            }
        }
        else if (dir.magnitude == 0)
        {
            // Decrease over time
            _acceleration -= (maxSpeed / _actor.DecelerationTime) * Time.deltaTime;
            _acceleration = Mathf.Max(_acceleration, 0);
        }
        

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

    protected bool IsGrounded()
    {
        return true;
    }

    private float GetMaxSpeed()
    {
        switch (_movementState)
        {
            default:
            case MovementState.Idle:
                return 0;
            case MovementState.Walking:
                return _actor.WalkingSpeed;
            case MovementState.Sprinting:
                return _actor.RunningSpeed;
            case MovementState.Crouching:
                return _actor.CrouchingSpeed;
        }
    }
}