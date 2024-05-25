namespace Island.Debug
{
    using Island.Actor;
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
        [SerializeField] private TextMeshProUGUI _acceleration;
        [SerializeField] private TextMeshProUGUI _momentum;
        [SerializeField] private TextMeshProUGUI _inputDir;
        [SerializeField] private TextMeshProUGUI _groundState;


        private void Start()
        {
            if (_debugUI)
            {
                _debugUI.SetActive(true);
            }
        }


        private void Update()
        {
            if (_actor != null && _actorBrain != null)
            {
                _fps.text = "fps: " + (1f / Time.deltaTime).ToString("F0");
                _velocity.text = "velocity: " + _actorBrain.NewVelocity.ToString("F2");
                _acceleration.text = "acceleration: " + _actorBrain.Acceleration.ToString("F2");
                _momentum.text = "momentum: " + _actorBrain.Momentum.ToString("F2");
                _inputDir.text = "input: " + _actorBrain.Direction.ToString("F0");
                _groundState.text = "ground state: " + _actorBrain.GroundState.ToString();
            }
        }

    }
}
