using System.Collections.Generic;

namespace Dog.Game
{
	public class GameState
	{
		public Character Player;
		public List<Character> Doggos = new List<Character>();
		public Character SelectedCharacter;
		public double TimeRemaining;
	}
}
