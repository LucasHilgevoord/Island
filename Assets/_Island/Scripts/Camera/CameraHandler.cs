using Project.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private Vector3 _forward;

    [SerializeField] private float _offset;
    [SerializeField] private float _horizontalRot, _verticalRot;

    [SerializeField] private bool _focusTarget;
    [SerializeField] private Transform _target;

    private Vector3 _targetPos; // NOT FINISHED
    [SerializeField] private Vector3 _targetOffset;

    private void Update()
    {
        UpdateOffset();
        UpdateRotation();
    }

    internal void UpdateOffset()
    {
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
        _horizontalRot = CapRotation(_horizontalRot);
        _verticalRot = CapRotation(_verticalRot);

        UpdateOffset();
        UpdateRotation();
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
