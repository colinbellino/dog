using Cysharp.Threading.Tasks;

namespace Dog.Game
{
	public interface IState
	{
		UniTask Enter();
		UniTask Exit();
		void Tick();
	}
}
