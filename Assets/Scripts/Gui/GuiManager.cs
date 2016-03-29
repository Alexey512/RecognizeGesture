using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game;
using System;

namespace Gui
{

	public class GuiManager : MonoBehaviour
	{
		[SerializeField]
		private FigureView _FigureView;

		[SerializeField]
		private GameManager _GameManager;

		[SerializeField]
		private Text _TimeLabel;

		[SerializeField]
		private Text _ScoreLabel;

		[SerializeField]
		private Text _TotalScoreLabel;

		[SerializeField]
		private GameObject _StartPanel;

		[SerializeField]
		private GameObject _CompletePanel;

		[SerializeField]
		private GameObject _FailPanel;

		void Start()
		{
			_StartPanel.SetActive(true);
			_CompletePanel.SetActive(false);
			_FailPanel.SetActive(false);

			_GameManager.OnChangeFigure.AddListener(UpdateFigureView);
			_GameManager.OnLevelComplete.AddListener(ShowCompleteWindow);
			_GameManager.OnLevelFail.AddListener(ShowFailWindow);

			UpdateScore();
		}

		public void RestartGame()
		{
			_StartPanel.SetActive(false);
			_CompletePanel.SetActive(false);
			_FailPanel.SetActive(false);

			_GameManager.ResetFigure();
		}

		public void ShowCompleteWindow()
		{
			_StartPanel.SetActive(false);
			_CompletePanel.SetActive(true);
			_FailPanel.SetActive(false);

			_TotalScoreLabel.text = string.Format("Счет: {0}", _GameManager.Scores);

			UpdateScore();
		}

		public void ShowFailWindow()
		{
			_StartPanel.SetActive(false);
			_CompletePanel.SetActive(false);
			_FailPanel.SetActive(true);
		}

		public void UpdateFigureView()
		{
			if (_FigureView == null || _GameManager == null)
				return;

			_FigureView.SetFigure(_GameManager.GetSourceFigure());

			UpdateScore();
		}
	
		private void UpdateScore()
		{
			_ScoreLabel.text = string.Format("Счет: {0}", _GameManager.Scores);
		}

		private void UpdateTime()
		{
			TimeSpan t = TimeSpan.FromSeconds(_GameManager.LeftTime);
			_TimeLabel.text = string.Format("Время: {0:D2}s:{1:D3}ms", t.Seconds, t.Milliseconds);
		}

		// Update is called once per frame
		void Update()
		{
			UpdateTime();
		}
	}

}