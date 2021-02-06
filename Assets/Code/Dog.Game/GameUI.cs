using UnityEngine;
using UnityEngine.UI;

namespace Dog.Game
{
	public class GameUI : MonoBehaviour
	{
		[SerializeField] private GameObject _gameplayRoot;
		[SerializeField] private Text _objectivesText;
		[SerializeField] private GameObject _victoryRoot;
		[SerializeField] private GameObject _defeatRoot;

		private void Awake()
		{
			HideGameplay();
			HideVictory();
			HideDefeat();
		}

		public void ShowGameplay() { _gameplayRoot.SetActive(true); }
		public void HideGameplay() { _gameplayRoot.SetActive(false); }
		public void ShowVictory() { _victoryRoot.SetActive(true); }
		public void HideVictory() { _victoryRoot.SetActive(false); }
		public void ShowDefeat() { _defeatRoot.SetActive(true); }
		public void HideDefeat() { _defeatRoot.SetActive(false); }

		public void SetObjectives(int current, int max)
		{
			_objectivesText.text = $"Pet the doggos: {current.ToString()} / {max.ToString()}";
		}
	}
}
