using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PLanA.Code.Grid
{
    public class BlockView:MonoBehaviour,IPointerClickHandler
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        [SerializeField] private Image image;
        private Canvas localCanvas;

        public event Action<int,int> OnClicked;

        void Awake()
        {
            if (!image) image = GetComponentInChildren<Image>(true);
            localCanvas = gameObject.GetComponent<Canvas>();
            if (!localCanvas) localCanvas = gameObject.AddComponent<Canvas>();
            localCanvas.overrideSorting = true;          
        }

        public void Setup(int row, int col, Sprite sprite)
        {
            Row = row; Col = col;
            if (image)
            {
                image.sprite = sprite;
                image.enabled = sprite != null;
            }
        }

        public void SetVisible(bool v)
        {
            if (image) image.enabled = v;
        }

        public void SetSortingForRow(int row, int cols, int cIndex)
        {
            localCanvas.sortingOrder = (row * 10) + cIndex+1;  
        }

        public void OnPointerClick(PointerEventData eventData) => OnClicked?.Invoke(Row, Col);
    }
}
