using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimator : MonoBehaviour
{
    [SerializeField] private Actor _actor;
    [SerializeField] private Animator _animator;

    internal void SetFloatValue(string name, float value)
    {
        _animator.SetFloat(name, value);
    }
}
