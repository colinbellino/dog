using Cysharp.Threading.Tasks;

namespace Dog.Game
{
	public class DefeatState : BaseGameState
	{
		public DefeatState(GameStateMachine machine, Game game) : base(machine, game) { }

		public override async UniTask Enter()
		{
			await base.Enter();

			_ui.ShowDefeat();
		}

		public override async UniTask Exit()
		{
			await base.Exit();

			_ui.HideDefeat();
		}
	}
}
