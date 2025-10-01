using PlanA.Core;

namespace PLanA.GamePlay.Scoring
{
    public class MoveCounter:IMoveCounter
    {
        public int Current { get;private set; }

        public MoveCounter(int start)
        {
            Reset(start);
        }
        public bool IsOver()
        {
            return Current <= 0;
        }

        public void Reset(int start)
        {
            Current = start;
        }

        public void UseOne()
        {
            if (Current > 0) Current--;
        }
    }
}

