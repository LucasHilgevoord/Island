using Project.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private enum CameraPresets
    {
        Custom = 0,
        TopDown = 1,
        ThirdPerson = 2,
        FirstPerson = 3,
        SideScroller = 4
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
    [SerializeField] private float _mouseSensitivity = 200;
    [SerializeField] private bool _enableScrollZoom;
    [SerializeField] private float _mouseScrollSensitivity = 0.1f;
    [SerializeField] private float _minScroll, _maxScroll;
    [SerializeField] private bool _flipMouseInput;

    [SerializeField] private CameraPresets _preset;

    private void Start()
    {
        switch (_preset)
        {
            case CameraPresets.Custom:
                break;
            case CameraPresets.TopDown:
                break;
            case CameraPresets.ThirdPerson:
                break;
            case CameraPresets.FirstPerson:
                break;
            case CameraPresets.SideScroller:
                break;
            default:
                break;
        }
    }

    private void SetRotation()
    {
        
    }

    private void Update()
    {
        UpdateOffset();
        UpdateRotation();

        if (_enableMouseRot)
        {
            if (Input.GetMouseButton(1))
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

    private void UpdateMousePosition()
    {
        int flip = _flipMouseInput ? -1 : 1;

        _horizontalRot += Input.GetAxis("Mouse X") * Time.deltaTime * _mouseSensitivity * flip;
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

    private void OnValidate()
    {
        if (Application.isEditor)
        {
            UpdateOffset();
            UpdateRotation();
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
}
