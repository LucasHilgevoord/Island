using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private GameObject _debugUI;
    [SerializeField] private Actor _actor;
    [SerializeField] private ActorBrain _actorBrain;

    [Header("Debug UI elements")]
    [SerializeField] private TextMeshProUGUI _fps;
    [SerializeField] private TextMeshProUGUI _velocity;
    [SerializeField] private TextMeshProUGUI _momentum;
    [SerializeField] private TextMeshProUGUI _inputDir;
    [SerializeField] private TextMeshProUGUI _groundState;


    private void Start()
    {
        _debugUI.SetActive(true);
    }


    private void Update()
    {
        _fps.text = "fps: " + (1f / Time.deltaTime).ToString("F0");
        _velocity.text = "velocity: " + _actor.CurrentVelocity.ToString("F2");
        _inputDir.text = "input dir: " + _actorBrain.Direction.ToString("F2");
        _momentum.text = "momentum: " + _actorBrain.Direction.ToString("F2");
        _groundState.text = "ground state: " + _actorBrain.GroundState.ToString();
    }

}
