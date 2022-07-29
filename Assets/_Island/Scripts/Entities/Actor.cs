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
    internal void SetVelocity(Vector3 velocity) { _rigidbody.velocity = velocity; }

    internal void SetRotation(Vector3 rotation) { transform.Rotate(rotation); }
}
