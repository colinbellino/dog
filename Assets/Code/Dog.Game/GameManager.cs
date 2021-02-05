using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Dog.Game.Stuff;

namespace Dog.Game
{
    public class GameManager : MonoBehaviour
    {
        private GameConfig _config;
        private GameControls _controls;
        private GameUI _ui;
        private GameState _state;
        private Camera _camera;
        private GameObject _arm;
        private Character _selectedCharacter;

        public class GameState
        {
	        public Character Player;
	        public List<Character> Doggos;
        }

        private void Awake()
        {
            _config = Resources.Load<GameConfig>("Game Config");
            _controls = new GameControls();
            _camera = Camera.main;
            _arm = GameObject.Find("Arm");
            _ui = FindObjectOfType<GameUI>();

            _state = new GameState();
            _state.Doggos = new List<Character>();
        }

        private void Start()
        {
            _controls.Gameplay.Enable();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            var playerSpawn = GameObject.Find("Player Spawner").transform;
            var player = SpawnCharacter(_config.PlayerPrefab, "Player", playerSpawn.position, playerSpawn.rotation);
            _state.Player = player;

            var spawners = FindObjectsOfType<SpawnerComponent>();
            for (var spawnerIndex = 0; spawnerIndex < spawners.Length; spawnerIndex++)
            {
	            var spawner = spawners[spawnerIndex].transform;
	            var doggo = SpawnCharacter(_config.DoggoPrefab, $"Doggo {spawnerIndex}", spawner.position, spawner.rotation);
	            _state.Doggos.Add(doggo);
            }

            _ui.SetObjectives(0, _state.Doggos.Count);

            _controls.Gameplay.Confirm.performed += OnConfirmPerformed;
        }

        private void Update()
        {
            var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
            var lookInput = _controls.Gameplay.Look.ReadValue<Vector2>();

            Debug.Log("Move input: " + moveInput);

            if (_state.Player.IsBusy == false)
            {
	            UpdateIsGrounded(_state.Player, _config.GroundCheckMask);
	            Move(_state.Player, moveInput, speed: 8f, gravityModifier: 3f, Time.deltaTime);
	            Look(_state.Player, _camera, lookInput, sensitivity: 50f, Time.deltaTime);
	            Follow(_camera.transform, _state.Player.Component.HeadTransform);

	            _selectedCharacter = GetCharacter(_state.Doggos, GetCharacterPointedAt(_camera, maxDistance: 2f, _config.InteractionMask));
	        }
        }

        public static bool WasPetted(Character character)
        {
	        return character.WasPetted;
        }
        public static Character GetCharacter(List<Character> characters, CharacterComponent component)
        {
	        return characters.Find(character => character.Component == component);
        }

        private async void OnConfirmPerformed(InputAction.CallbackContext obj)
        {
	        if (_selectedCharacter == null || _state.Player.IsBusy)
	        {
		        Debug.Log("Nothing to pet!");
		        return;
	        }

	        _state.Player.IsBusy = true;

	        await AnimatePet(_arm.transform);
	        Instantiate(_config.PetParticles, _selectedCharacter.Component.RootTransform.position, _selectedCharacter.Component.RootTransform.rotation);

	        _state.Player.IsBusy = false;
	        _selectedCharacter.WasPetted = true;

	        _ui.SetObjectives(_state.Doggos.Where(WasPetted).Count(), _state.Doggos.Count());
        }
    }
}
