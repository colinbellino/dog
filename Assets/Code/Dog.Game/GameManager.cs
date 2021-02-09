using UnityEngine;

namespace Dog.Game
{
	public class GameManager : MonoBehaviour
    {
        private GameStateMachine _machine;

        public Game Game { get; private set; }

        private void Awake()
        {
	        Game = new Game();
            Game.Config = Resources.Load<GameConfig>("Game Config");
            Game.Controls = new GameControls();
            Game.Camera = Camera.main;
            Game.UI = FindObjectOfType<GameUI>();
            Game.PlayerArm = GameObject.Find("Arm");
            Game.State = new GameState();

	        _machine = new GameStateMachine(true, Game);
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
