using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanA.Core
{
    public interface IMoveCounter
    {
        public int Current { get; }
        public bool IsOver();
        public void Reset(int start);
        public void UseOne();
    }
}
