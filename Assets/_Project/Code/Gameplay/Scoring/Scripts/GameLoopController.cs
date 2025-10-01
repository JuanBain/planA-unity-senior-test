using PlanA.Core;
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
        [SerializeField] Button makeMoveButton;   // temporal en Task 2
        [SerializeField] Button replayButton;
        [Header("Config")]
        [SerializeField] int startMoves = 5;

        private IScoringService scoring;
        private IMoveCounter moves;
        private int score;

        void Awake()
        {
            scoring = new LinearScoringService();
            moves = new MoveCounter(startMoves);
            score = 0;
            RefreshUI();

            // Wire buttons
            if (makeMoveButton) makeMoveButton.onClick.AddListener(SimulateMove);
            if (replayButton) replayButton.onClick.AddListener(Replay);
        }

        private void SimulateMove()
        {
            if (moves.IsOver()) return;

            score = scoring.Add(score, 10);
            moves.UseOne();
            RefreshUI();

            if (moves.IsOver())
            {
                gameOverPanel.SetActive(true);
                if (makeMoveButton) makeMoveButton.interactable = false;
            }
        }

        public void Replay()
        {
            moves.Reset(startMoves);
            score = 0;
            gameOverPanel.SetActive(false);
            if (makeMoveButton) makeMoveButton.interactable = true;
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (hudView != null) hudView.UpdateScoreAndMoves(score, moves.Current);
        }
    }
}