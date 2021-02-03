using UnityEngine;

namespace Dog.Game
{
    public class GameManager : MonoBehaviour
    {
        private Player _player;
        private GameControls _controls;

        private void Awake()
        {
            _controls = new GameControls();
            _player = GameObject.Find("Player").GetComponent<Player>();
        }

        private void Start()
        {
            _controls.Gameplay.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            var move = _controls.Gameplay.Move.ReadValue<Vector2>();
            var look = _controls.Gameplay.Look.ReadValue<Vector2>();

            _player.Move(move, speed: 10f, gravityModifier: 2f, Time.deltaTime);
            _player.Look(look, sensitivity: 50f, Time.deltaTime);
        }
    }
}