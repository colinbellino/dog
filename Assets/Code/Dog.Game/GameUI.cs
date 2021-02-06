using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Dog.Game
{
	public class GameUI : MonoBehaviour
	{
		[Header("Gameplay")]
		[SerializeField] private GameObject _gameplayRoot;
		[SerializeField] private VerticalLayoutGroup _objectivesLayout;
		[SerializeField] private Text _objectivesText;
		[SerializeField] private VerticalLayoutGroup _timerLayout;
		[SerializeField] private Text _timerText;
		[Header("Victory")]
		[SerializeField] private GameObject _victoryRoot;
		[Header("Defeat")]
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

		public async void SetObjective(string name, int current, int objective)
		{
			_objectivesText.text = $"{name}: {current.ToString()} / {objective.ToString()}";

			_objectivesLayout.enabled = false;
			await UniTask.NextFrame();
			_objectivesLayout.enabled = true;
		}

		public void SetTimer(double value)
		{
			_timerText.text = value.ToString("0.0");
		}
	}
}
