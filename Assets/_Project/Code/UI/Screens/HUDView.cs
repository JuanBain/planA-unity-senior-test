using UnityEngine;

namespace PLanA.UI
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI scoreText;
        [SerializeField] TMPro.TextMeshProUGUI movesText;

        public void UpdateScoreAndMoves(int score, int moves)
        {
            if (scoreText) scoreText.text = score.ToString();
            if (movesText) movesText.text = moves.ToString();
        }
    }
}