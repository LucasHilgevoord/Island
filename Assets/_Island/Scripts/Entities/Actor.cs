namespace Island.Actor
{
    using UnityEngine;
    public class Actor : MonoBehaviour
    {
        internal Rigidbody Rigidbody => _rigidbody;
        internal Collider Collider => _collider;

        [Header("Component References :")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        [Header("Actor Variables :")]
        [SerializeField] private float _walkingForce = 1;
        [SerializeField] private float _runningForce = 2;
        [SerializeField] private float _crouchingForce = 0.5f;
        [SerializeField] private float _turningSpeed = 1;

        [SerializeField, Tooltip("How much Kinetic Energy")] private float _jumpCharge = 0.1f;
        [SerializeField] private float _jumpForce = 1;

        /// The higher the number, the longer it takes
        [SerializeField] private float _accelerationTime = 1;
        [SerializeField] private float _decelerationTime = 1;
        [SerializeField] private float _breakTime = 1;


        #region Getters
        internal float WalkingForce => _walkingForce;
        internal float RunningForce => _runningForce;
        internal float CrouchingForce => _crouchingForce;
        internal float TurningSpeed => _turningSpeed;
        internal float JumpForce => _jumpForce;
        internal float AccelerationTime => _accelerationTime;
        internal float DecelerationTime => _decelerationTime;
        internal float BreakTime => _breakTime;
        internal Vector3 CurrentVelocity => _rigidbody.velocity;
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
            _rigidbody.velocity = velocity;
        }

        /// <summary>
        /// Method to set the rotation of this Actor
        /// </summary>
        /// <param name="rotation">New Rotation</param>
        internal void SetRotation(Quaternion rotation)
        {
            if (rotation == transform.rotation) { return; }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _turningSpeed * Time.deltaTime);
        }

        internal bool IsGrounded()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, Vector3.down * 0.01f, Color.red);
            return Physics.Raycast(transform.position, Vector3.down, out hit, 0.01f);
        }
    }
}