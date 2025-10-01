using System.Collections;
using System.Collections.Generic;
using PLanA.Code.Grid;
using PlanA.Core;
using PlanA.Gameplay.Scoring;
using UnityEngine;
using UnityEngine.UI;


namespace PlanA.Gameplay.Grid
{
    public class GridPresenter : MonoBehaviour
    {
        [Header("Wiring")] [SerializeField] private GridConfig config;
        [SerializeField] private RectTransform gridParent; 
        [SerializeField] private BlockView blockPrefab; 
        [SerializeField] private Sprite[] colorSprites;
        [SerializeField] private GameLoopController gameLoop; 

        private IGridService _grid;
        private BlockView[,] _views;
        private bool _busy;

        void Awake()
        {
            _grid = new GridService();
        }

        void Start()
        {
            BuildNewGrid();
        }

        public void BuildNewGrid()
        {
            int usableColors = Mathf.Min(config.colorCount, colorSprites.Length);
            _grid.Init(config.rows, config.cols, usableColors);
            BuildViews();
            RefreshAll();
        }

        private void BuildViews()
        {
            for (int i = gridParent.childCount - 1; i >= 0; i--)
                Destroy(gridParent.GetChild(i).gameObject);

            _views = new BlockView[_grid.Rows, _grid.Cols];

            var layout = gridParent.GetComponent<GridLayoutGroup>();
            if (!layout) layout = gridParent.gameObject.AddComponent<GridLayoutGroup>();
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = _grid.Cols;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.spacing = Vector2.zero;
            layout.cellSize = new Vector2(128, 112);

          
            for (int r = 0; r < _grid.Rows; r++)
            for (int c = 0; c < _grid.Cols; c++)
            {
                var v = Instantiate(blockPrefab, gridParent);
                v.OnClicked += OnBlockClicked;
                _views[r, c] = v;

                
                int renderRow = (_grid.Rows - 1 - r);
                v.SetSortingForRow(renderRow, _grid.Cols, c);
            }
        }

        private void RefreshAll()
        {
            for (int r = 0; r < _grid.Rows; r++)
            for (int c = 0; c < _grid.Cols; c++)
            {
                SetViewSprite(r, c, _grid.Data[r, c]);

                
                int renderRow = (_grid.Rows - 1 - r);
                _views[r, c].SetSortingForRow(renderRow, _grid.Cols, c);
            }
        }

        private void SetViewSprite(int r, int c, int colorId)
        {
            Sprite sprite = null;
            if (colorId >= 0 && colorId < colorSprites.Length)
                sprite = colorSprites[colorId];

            _views[r, c].Setup(r, c, sprite);

            
            _views[r, c].SetVisible(sprite != null);
        }

        private void OnBlockClicked(int r, int c)
        {
            if (_busy) return;
            StartCoroutine(CollectFlow(r, c));
        }

        private IEnumerator CollectFlow(int r, int c)
        {
            _busy = true;

            var cluster = _grid.GetCluster(r, c);
            if (cluster.Count == 0)
            {
                _busy = false;
                yield break;
            }

            gameLoop.OnBlocksCollected(cluster.Count);

            foreach (var (rr, cc) in cluster)
                _views[rr, cc].SetVisible(false);

            _grid.Remove(cluster);

            yield return new WaitForSeconds(config.waitAfterCollectSeconds);

            _grid.Refill();
            RefreshAll();

            _busy = false;
        }

        public void ResetGrid()
        {
            _busy = false;
            BuildNewGrid();
        }
    }
}

