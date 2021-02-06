﻿using Cysharp.Threading.Tasks;

namespace Dog.Game
{
	public class VictoryState : BaseGameState
	{
		public VictoryState(GameStateMachine machine, Game game) : base(machine, game) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_ui.ShowVictory();
		}

		public override async UniTask Exit()
		{
			await base.Exit();

			_ui.HideVictory();
		}
	}
}
