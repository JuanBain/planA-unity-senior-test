using PlanA.Core;

namespace PlanA.Gameplay.Scoring
{
    public class LinearScoringService:IScoringService
    {
        public int Add(int currentScore, int delta)
        {
            currentScore += delta;
            return currentScore;
        }
    }
}
