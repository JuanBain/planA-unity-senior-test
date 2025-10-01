
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "PlanA/GridConfig")]
    public class GridConfig:ScriptableObject
    {
        public int rows = 6;
        public int cols = 5;
        public int colorCount = 6;
        public float waitAfterCollectSeconds = 1f;
    }
