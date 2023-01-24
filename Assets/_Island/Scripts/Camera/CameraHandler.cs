using Project.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private enum CameraPresets
    {
        Custom = 0,
        TopDown = 1,
        ThirdPerson = 2,
        SideScroller = 3,
        //FirstPerson = 4 // NOT YET IMPLEMENTED
    }

    [SerializeField] private Vector3 _forward;

    [SerializeField] private float _offset;
    [SerializeField] private float _horizontalRot, _verticalRot;

    [SerializeField] private bool _focusTarget;
    [SerializeField] private Transform _target;

    private Vector3 _targetPos; // NOT FINISHED
    [SerializeField] private Vector3 _targetOffset;

    [Header("Settings")]
    [SerializeField] private bool _enableMouseRot;
    [SerializeField] private bool _holdMouseToRotate;
    [SerializeField] private bool _hideCursor;
    [SerializeField] private float _mouseSensitivity = 200;
    [SerializeField] private bool _enableScrollZoom = true;
    [SerializeField] private float _mouseScrollSensitivity = 0.1f;
    [SerializeField] private float _minScroll, _maxScroll;
    [SerializeField] private bool _flipVerticalMouseInput;

    [SerializeField] private CameraPresets _preset;

    [Header("Camera Collision")]
    [SerializeField] private bool _enableCollision;
    [SerializeField] private Vector2 _collisionRange;
    [SerializeField] private float _collisionSmooth;
    private Vector3 _normalizedPos;

    public bool MouseRotEnabled { 
        get { return _enableMouseRot; }
        set { _enableMouseRot = value; }
    }
    
    private Vector2[] _presetValues
    {
        get
        {
            return new Vector2[]
            {
                new Vector2(0, 0), // Custom
                new Vector2(180, 270), // Top Down
                new Vector2(180, 340), // Third Person
                new Vector2(90, 360),  // Side Scroller
                new Vector2(0, 0) // First Person
            };
        }
    }

    private void Awake()
    {
        _normalizedPos = transform.localPosition.normalized;
    }

    private void Start()
    {
        SetPreset();
    }

    private void Update()
    {
        UpdateOffset();
        UpdateRotation();
        
        if (_enableCollision)
            UpdateCollision();

        if (_enableMouseRot)
        {
            if (Input.GetMouseButton(1) || !_holdMouseToRotate)
                UpdateMousePosition();
            
            if (_enableScrollZoom)
            {
                float scroll = -Input.mouseScrollDelta.y * _mouseScrollSensitivity;
                float newOffset = _offset + scroll;
                if (newOffset < _minScroll)
                {
                    newOffset = _minScroll;
                }
                else if (newOffset > _maxScroll)
                {
                    newOffset = _maxScroll;
                }

                _offset = newOffset;
            }
        }
    }

    private void SetPreset()
    {
        if (_preset != CameraPresets.Custom)
        {
            _horizontalRot = _presetValues[(int)_preset].x;
            _verticalRot = _presetValues[(int)_preset].y;
        }
    }

    private void UpdateMousePosition()
    {
        int flip = _flipVerticalMouseInput ? -1 : 1;

        _horizontalRot += Input.GetAxis("Mouse X") * Time.deltaTime * _mouseSensitivity;
        _verticalRot += Input.GetAxis("Mouse Y") * Time.deltaTime * _mouseSensitivity * flip;
    }

    internal void UpdateOffset()
    {
        _horizontalRot = CapRotation(_horizontalRot);
        _verticalRot = CapRotation(_verticalRot);        

        Vector3 offset = _forward * _offset;
        transform.position = RotateAroundTarget(offset) + _target.position + _targetOffset;
    }

    internal void UpdateRotation()
    {
        transform.LookAt(_target.position + _targetOffset);
    }

    private Vector3 RotateAroundTarget(Vector3 origin)
    {
        float phi = (Mathf.PI / 180) * _horizontalRot;
        float theta = (Mathf.PI / 180) * _verticalRot;
        return new Vector3(
            (origin.x * Mathf.Cos(phi) + origin.y * Mathf.Sin(phi) * Mathf.Cos(theta) + origin.z * Mathf.Sin(phi) * Mathf.Cos(theta)),
            (origin.y * Mathf.Cos(theta) - origin.z * Mathf.Sin(theta)),
            (origin.x * Mathf.Sin(phi) + origin.y * Mathf.Cos(phi) * Mathf.Sin(theta) + origin.z * Mathf.Cos(phi) * Mathf.Cos(theta))
            );
    }

    internal void SetTarget(Transform target)
    {
        if (_focusTarget || target == null)
        {
            _targetPos = _target == null ? _targetPos : _target.position;
            _target = null;
        } else
            _targetPos =  _target.position;
        
        _target = target;
    }

    private void UpdateCollision()
    {
        //Vector3 desiredCameraPos = transform.TransformPoint(_normalizedPos * _collisionRange.y);
        //float distance;
        //RaycastHit hit;
        
        //if (Physics.Linecast (transform.position, desiredCameraPos, out hit))
        //{
        //    distance = Mathf.Clamp(hit.distance * 0.8f, _collisionRange.x, _collisionRange.y);
        //} else
        //{
        //    distance = _collisionRange.y;
        //}

        //transform.localPosition = Vector3.Lerp(transform.localPosition, _normalizedPos * distance, Time.deltaTime * _collisionSmooth);
    }

    #region Editor

    private void OnValidate()
    {
        if (Application.isEditor)
        {
            SetPreset();
            UpdateOffset();
            UpdateRotation();

            if (_enableScrollZoom)
            {
                if (_offset < _minScroll)
                    _offset = _minScroll;
                else if (_offset > _maxScroll)
                    _offset = _maxScroll;
            }
        }
    }

    private float CapRotation(float rot)
    {
        if (rot > 360)
            rot = rot - 360;
        else if (rot < 0)
        {
            rot = rot + 360;
        }

        return rot;
    }

    private void OnDrawGizmosSelected()
    {
        _targetPos = _target == null ? Vector3.zero : _target.position;
        GizmosUtils.DrawHorizontalCircle(_targetPos + _targetOffset, _offset, Color.red);
        GizmosUtils.DrawVerticalCircle(_targetPos + _targetOffset, _offset, Color.blue);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (_hideCursor)
            Cursor.visible = !focus;
    }
    #endregion
}
