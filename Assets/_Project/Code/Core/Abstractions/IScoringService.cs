using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanA.Core
{
    public interface IScoringService
    {
        public int Add(int currentScore, int delta);
    }
}
