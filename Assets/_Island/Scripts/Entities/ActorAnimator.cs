using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimator : MonoBehaviour
{
    [SerializeField] private Actor _actor;
    [SerializeField] private Animator _animator;

    private string _currentAnim;

    private void Update()
    {
        if (_animator == null || _actor == null)
            return;

        Vector3 currentVelocity = _actor.CurrentVelocity;

        // TEMP TRASH
        if (currentVelocity == Vector3.zero && _currentAnim != "idle")
        {
            _currentAnim = "idle";
            _animator.Play(_currentAnim, 0);
        }
        
        if (currentVelocity != Vector3.zero && _currentAnim != "walk")
        {
            _currentAnim = "walk";
            _animator.Play(_currentAnim, 0);
        }
    }

    internal void SetFloatValue(string name, float value)
    {
        _animator.SetFloat(name, value);
    }
}
