using System.Collections;
using PlanA.Core;
using PlanA.Gameplay.Grid;
using PLanA.GamePlay.Scoring;
using UnityEngine;
using UnityEngine.UI;
using PLanA.UI;

namespace PlanA.Gameplay.Scoring
{
    public class GameLoopController:MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] HUDView hudView;
        [SerializeField] GameObject gameOverPanel;
        [SerializeField] Button replayButton;

        [Header("Config")]
        [SerializeField] int startMoves = 5;
        [SerializeField] float gameOverDelaySeconds = 1f;

        [Header("Grid")]
        [SerializeField] GridPresenter gridPresenter;

        private IScoringService scoring;
        private IMoveCounter moves;
        private int score;
        private Coroutine gameOverCoroutine;

        void Awake()
        {
            scoring = new LinearScoringService();
            moves = new MoveCounter(startMoves);
            score = 0;
            RefreshUI();
            if (replayButton) replayButton.onClick.AddListener(Replay);
        }

        public void OnBlocksCollected(int count)
        {
            if (moves.IsOver()) return;

            score = scoring.Add(score, count);
            moves.UseOne();
            RefreshUI();

            if (moves.IsOver())
            {
                if (gridPresenter) gridPresenter.enabled = false;

                if (gameOverCoroutine != null) StopCoroutine(gameOverCoroutine);
                gameOverCoroutine = StartCoroutine(GameOverAfterDelay());
            }
        }

        private IEnumerator GameOverAfterDelay()
        {
            yield return new WaitForSeconds(gameOverDelaySeconds);
            gameOverPanel.SetActive(true);
        }
        
        public void Replay()
        {
            if (gameOverCoroutine != null)                   
            {
                StopCoroutine(gameOverCoroutine);
                gameOverCoroutine = null;
            }
            moves.Reset(startMoves);
            score = 0;
            gameOverPanel.SetActive(false);
            if (gridPresenter)
            {
                gridPresenter.ResetGrid();
                gridPresenter.enabled = true;              
            }
            RefreshUI();
        }

        private void RefreshUI() => hudView.UpdateScoreAndMoves(score, moves.Current);
    }
}