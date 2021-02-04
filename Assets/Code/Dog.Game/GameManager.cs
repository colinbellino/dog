using UnityEngine;
using UnityEngine.InputSystem;
using static Dog.Game.Stuff;

namespace Dog.Game
{
    public class GameManager : MonoBehaviour
    {
        private GameConfig _config;
        private GameControls _controls;
        private Camera _camera;
        private Character _player;
        private GameObject _arm;
        private Character _pointedCharacter;
        private bool _busy;

        private void Awake()
        {
            _config = Resources.Load<GameConfig>("Game Config");
            _controls = new GameControls();
            _camera = Camera.main;
            _arm = GameObject.Find("Arm");
            _player = GameObject.Find("Player").GetComponent<Character>();
        }

        private void Start()
        {
            _controls.Gameplay.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _controls.Gameplay.Confirm.performed += OnConfirmPerformed;
        }

        private void Update()
        {
            var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
            var lookInput = _controls.Gameplay.Look.ReadValue<Vector2>();

            UpdateIsGrounded(_player, _config.GroundCheckMask);
            Move(_player, moveInput, speed: 10f, gravityModifier: 2f, Time.deltaTime);
            Look(_player, _camera, lookInput, sensitivity: 50f, Time.deltaTime);

            _camera.transform.position = _player.HeadTransform.position;
            _camera.transform.rotation = _player.HeadTransform.rotation;

            _pointedCharacter = GetCharacterPointedAt(_camera, _config.InteractionMask);
            Debug.Log("char: " + (_pointedCharacter == null ? "nope" : _pointedCharacter.Name));
        }

        private async void OnConfirmPerformed(InputAction.CallbackContext obj)
        {
	        if (_pointedCharacter == null || _busy)
	        {
		        return;
	        }

	        _busy = true;
	        Instantiate(_config.PetParticles, _pointedCharacter.BodyTransform.position, Quaternion.identity);
	        Debug.Log("petting " + _pointedCharacter.Name);
	        await AnimatePet(_arm.transform);
	        _busy = false;
        }
    }
}
