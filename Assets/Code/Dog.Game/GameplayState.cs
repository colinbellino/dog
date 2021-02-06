using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dog.Game
{
	public class GameplayState : BaseGameState
	{
		public GameplayState(GameStateMachine machine, Game game) : base(machine, game) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_controls.Gameplay.Enable();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			var playerSpawn = GameObject.Find("Player Spawner").transform;
			var player = Stuff.SpawnCharacter(_config.PlayerPrefab, "Player", playerSpawn.position, playerSpawn.rotation);
			_state.Player = player;

			var spawners = GameObject.FindObjectsOfType<SpawnerComponent>();
			for (var spawnerIndex = 0; spawnerIndex < spawners.Length; spawnerIndex++)
			{
				var spawner = spawners[spawnerIndex].transform;
				var doggo = Stuff.SpawnCharacter(_config.DoggoPrefab, $"Doggo {spawnerIndex}", spawner.position, spawner.rotation);
				_state.Doggos.Add(doggo);
			}

			_ui.ShowGameplay();
			_ui.SetObjectives(0, _state.Doggos.Count);

			_controls.Gameplay.Confirm.performed += OnConfirmPerformed;
		}

		public override void Tick()
		{
			base.Tick();

			var moveInput = _controls.Gameplay.Move.ReadValue<Vector2>();
			var lookInput = _controls.Gameplay.Look.ReadValue<Vector2>();

			Debug.Log("Move input: " + moveInput);

			if (_state.Player.IsBusy == false)
			{
				Stuff.UpdateIsGrounded(_state.Player, _config.GroundCheckMask);
				Stuff.Move(_state.Player, moveInput, speed: 8f, gravityModifier: 3f, Time.deltaTime);
				Stuff.Look(_state.Player, _camera, lookInput, sensitivity: 50f, Time.deltaTime);
				Stuff.Follow(_camera.transform, _state.Player.Component.HeadTransform);

				_state.SelectedCharacter = GetCharacter(_state.Doggos, Stuff.GetCharacterPointedAt(_camera, maxDistance: 2f, _config.InteractionMask));
			}
		}

		public override async UniTask Exit()
		{
			await base.Exit();

			_ui.HideGameplay();
		}

		private static bool WasPetted(Character character)
		{
			return character.WasPetted;
		}

		private static Character GetCharacter(List<Character> characters, CharacterComponent component)
		{
			return characters.Find(character => character.Component == component);
		}

		private async void OnConfirmPerformed(InputAction.CallbackContext obj)
		{
			if (_state.SelectedCharacter == null || _state.Player.IsBusy)
			{
				Debug.Log("Nothing to pet!");
				return;
			}

			_state.Player.IsBusy = true;

			await Stuff.AnimatePet(_playerArm.transform);
			GameObject.Instantiate(_config.PetParticles, _state.SelectedCharacter.Component.RootTransform.position, _state.SelectedCharacter.Component.RootTransform.rotation);

			_state.Player.IsBusy = false;
			_state.SelectedCharacter.WasPetted = true;

			var current = _state.Doggos.Where(WasPetted).Count();
			var objective = _state.Doggos.Count();
			_ui.SetObjectives(current, objective);
			if (current >= objective)
			{
				_machine.Fire(GameStateMachine.Triggers.Victory);
			}
		}
	}
}
