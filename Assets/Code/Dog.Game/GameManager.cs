using System.Collections.Generic;
using UnityEngine;

namespace Dog.Game
{
	public class Game
	{
		public GameConfig Config;
		public GameUI UI;
		public Camera Camera;
		public GameControls Controls;
		public GameState State;
		public GameObject PlayerArm;
	}

	public class GameState
	{
		public Character Player;
		public List<Character> Doggos = new List<Character>();
		public Character SelectedCharacter;
	}

    public class GameManager : MonoBehaviour
    {
        private GameStateMachine _machine;

        private void Awake()
        {
	        var game = new Game();
            game.Config = Resources.Load<GameConfig>("Game Config");
            game.Controls = new GameControls();
            game.Camera = Camera.main;
            game.UI = FindObjectOfType<GameUI>();
            game.PlayerArm = GameObject.Find("Arm");
            game.State = new GameState();

	        _machine = new GameStateMachine(true, game);
        }

        private void Start()
        {
	        _machine.Start();
        }

        private void Update()
        {
			_machine.Tick();
        }
    }
}
