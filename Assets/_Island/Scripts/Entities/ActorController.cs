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
    
    [Header("External Forces: ")]
    [SerializeField] private float _gravityForce;
    [SerializeField] private float _airResistance;
    [SerializeField] private float _groundFriction;

    [Header("Controller Variables :")]
    private ControllerState _controllerState;
    private MovementState _movementState;
    private Vector3 _newVelocity, _currentVelocity;
    private Vector3 _direction;
    private Vector3 _rotation;
    private Vector3 _momentum;
    private float _acceleration;

    [Header("Camera Options :")]
    [SerializeField] private bool _useCameraDirection;
    [SerializeField] private Camera _camera;

    [Header("Actor Options :")]
    [SerializeField] private Actor _actor;
    [SerializeField] private float _walkingSpeed = 1;
    [SerializeField] private float _runningSpeed = 1;
    [SerializeField] private float _crouchingSpeed = 1;
    [SerializeField] private float _turningSpeed = 1;
    
    /// How higher the number, how longer it takes
    [SerializeField] private float _accelerationTime = 1;
    [SerializeField] private float _decelerationTime = 1;
    [SerializeField] private float _breakTime = 1;

    private void Start()
    {
        // TESTING
        _movementState = MovementState.Walking;
    }

    private void AssignActor(Actor ac) { _actor = ac; }

    private void FixedUpdate()
    {
        // Update the controller
        UpdateController();
    }

    private void UpdateController()
    {
        // Only allow movement if we say so
        if (_lockInput || _actor == null) { return; }

        // Save the current velocity before changing it
        _currentVelocity = _newVelocity;

        // Get the horizontal and vertical velocity
        _direction = GetDirection();

        // Calculate the rotation we are supposed to get
        _rotation = CalculateRotation(_direction);

        // Apply acceleration if there is 
        _acceleration = CalculateAcceleration(_direction);
        _newVelocity = _actor.transform.forward * _acceleration;

        // Calculate the momentum
        _momentum = CalculateMomentum();

        // Set the velocity of the assigned actor
        _actor.SetVelocity(_newVelocity);
        _actor.SetRotation(_rotation);
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = Vector3.zero;
        // TODO: Create a input system

        // Check if we want to move based on the camera it's direction
        if (_useCameraDirection)
        {

        } else
        {
            dir += transform.right * Input.GetAxisRaw("Horizontal");
            dir += transform.forward * Input.GetAxisRaw("Vertical");
        }

        //if (_velocity.magnitude > 1f)
        //    _velocity.Normalize();

        // Add the movement speed
        //velocity *= GetMovementSpeed();
        return dir;
    }

    private Vector3 CalculateRotation(Vector3 dir)
    {
        if (dir.x == 0) return Vector3.zero;
        Debug.Log(dir);

        Vector3 rot = _actor.transform.up * (_turningSpeed * Time.deltaTime) * dir.x;
        return rot;
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

    private float CalculateAcceleration(Vector3 dir)
    {
        if (dir.z > 0)
        {
            // Increase acceleration
            _acceleration += (GetMaxSpeed() / _accelerationTime) * Time.deltaTime;
            _acceleration = Mathf.Min(_acceleration, GetMaxSpeed());
        }
        else if (dir.z == 0)
        {
            // Decrease over time
            _acceleration -= (GetMaxSpeed() / _decelerationTime) * Time.deltaTime;
            _acceleration = Mathf.Max(_acceleration, 0);
        }
        else
        {
            // Break
            _acceleration -= (GetMaxSpeed() / _breakTime) * Time.deltaTime;
            _acceleration = Mathf.Max(_acceleration, 0);
        }

        return _acceleration;
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
                return _walkingSpeed;
            case MovementState.Sprinting:
                return _runningSpeed;
            case MovementState.Crouching:
                return _crouchingSpeed;
        }
    }
}