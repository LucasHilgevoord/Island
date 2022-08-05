using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    internal Rigidbody Rigidbody => _rigidbody;
    internal Collider Collider => _collider;
    
    [Header("Component References :")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;

    [Header("Actor Variables :")]
    [SerializeField] private float _walkingSpeed = 1;
    [SerializeField] private float _runningSpeed = 1;
    [SerializeField] private float _crouchingSpeed = 1;
    [SerializeField] private float _turningSpeed = 1;

    /// How higher the number, how longer it takes
    [SerializeField] private float _accelerationTime = 1;
    [SerializeField] private float _decelerationTime = 1;
    [SerializeField] private float _breakTime = 1;

    #region Getters
    internal float WalkingSpeed => _walkingSpeed;
    internal float RunningSpeed => _runningSpeed;
    internal float CrouchingSpeed => _crouchingSpeed;
    internal float TurningSpeed => _turningSpeed;
    internal float AccelerationTime => _accelerationTime;
    internal float DecelerationTime => _decelerationTime;
    internal float BreakTime => _breakTime;

    #endregion

    //[Header("Movement Options :")]
    // TODO: Move everything to here so we can have different values for reach actor
    private void Awake()
    {
        if (_rigidbody == null)
            _rigidbody = GetComponent<Rigidbody>();
        
        if (_collider == null)
            _collider = GetComponent<Collider>();
    }

    /// <summary>
    /// Methode to set the velocity of this Actor
    /// </summary>
    /// <param name="velocity">New Velocity</param>
    internal void SetVelocity(Vector3 velocity)
    {
        if (velocity == _rigidbody.velocity) { return; }
        _rigidbody.velocity = velocity; 
    }

    internal void SetRotation(Quaternion rotation) 
    {
        if (rotation == transform.rotation) { return; }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _turningSpeed * Time.deltaTime); 
    }
}
