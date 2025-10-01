using UnityEngine;
using System.Collections.Generic;
namespace CardMatchGame.Handler
{
    public class GridSystem : MonoBehaviour
    {
        [Header("Referance Setup")]
        [SerializeField] private RectTransform parentRect;
        [SerializeField] private RectTransform centerPoint;
        [SerializeField] private GameObject cardPrefab;

        [Header("Grid Settings")]
        [SerializeField] private int rows = 4;
        [SerializeField] private int columns = 4;
        [SerializeField] private float spacing = 10f;

        [Header("Grid Margins")]
        [SerializeField] private float leftMargin = 0f;
        [SerializeField] private float rightMargin = 0f;
        [SerializeField] private float topMargin = 0f;
        [SerializeField] private float bottomMargin = 0f;
        [SerializeField] private float maxScale = 1;


        private Vector2 mLastSize;
        private List<RectTransform> mCardPool = new List<RectTransform>();


        public float MaxScale
        {
            get
            {
                return maxScale;
            }
            set
            {
                maxScale = value;                
            }
        }

        public void GetCards<T>(ref List<T> newList)
        {
            newList.Clear();
            for (int i = 0; i < mCardPool.Count; i++)
            {
                newList.Add(mCardPool[i].GetComponent<T>());
            }
        }

        //private void Start()
        //{
        //    RebuildGrid();
        //    mLastSize = parentRect.rect.size;
        //}

        public void SetMarging(float left, float right, float top, float bottom)
        {
            leftMargin = left;
            rightMargin = right;
            topMargin = top;
            bottomMargin = bottom;
        }
        public void InitGridLayout(int rows, int columns, float spacing,GameObject cardPrefab=null)
        {
            if (cardPrefab != null)
            {
                this.cardPrefab= cardPrefab;
            }
            this.rows = rows;
            this.columns = columns;
            this.spacing = spacing;
            RebuildGrid();
            mLastSize = parentRect.rect.size;
        }



        private void Update()
        {
            if (parentRect.rect.size != mLastSize)
            {
                mLastSize = parentRect.rect.size;
                RebuildGrid();
            }
        }

        void RebuildGrid()
        {
            int requiredCards = rows * columns;
            if (requiredCards == 0)
            {
                return;
            }

            while (mCardPool.Count < requiredCards)
            {
                GameObject card = Instantiate(cardPrefab, parentRect);
                mCardPool.Add(card.GetComponent<RectTransform>());
            }

            for (int i = 0; i < mCardPool.Count; i++)
            {
                mCardPool[i].gameObject.SetActive(i < requiredCards);
            }

            float availableWidth = parentRect.rect.width - leftMargin - rightMargin;
            float availableHeight = parentRect.rect.height - topMargin - bottomMargin;

            float cellWidth = (availableWidth - spacing * (columns - 1)) / columns;
            float cellHeight = (availableHeight - spacing * (rows - 1)) / rows;
            float cellSize = Mathf.Min(cellWidth, cellHeight);

            Vector2 totalGridSize = new Vector2(
                columns * cellSize + (columns - 1) * spacing,
                rows * cellSize + (rows - 1) * spacing
            );


            Vector2 offset = new Vector2(
                (leftMargin - rightMargin) / 2f,
                (bottomMargin - topMargin) / 2f
            );

            Vector2 startPos = (Vector2)centerPoint.localPosition - totalGridSize / 2f + new Vector2(cellSize, cellSize) / 2f + offset;

            int index = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    RectTransform rt = mCardPool[index++];
                    rt.sizeDelta = new Vector2(cellSize / maxScale, cellSize / maxScale);

                    float x = startPos.x + col * (cellSize + spacing);
                    float y = startPos.y + (rows - 1 - row) * (cellSize + spacing);

                    rt.localPosition = new Vector3(x, y, 0);
                }
            }
        }
    }
}