using UnityEngine;
using UnityEngine.UI;

namespace Dog.Game
{
	public class GameUI : MonoBehaviour
	{
		[SerializeField] private Text _objectivesText;

		public void SetObjectives(int current, int max)
		{
			_objectivesText.text = $"Pet the doggos: {current.ToString()} / {max.ToString()}";
		}
	}
}
