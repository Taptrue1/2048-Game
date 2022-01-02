using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Area : MonoBehaviour
{
    [Header("Area Settings")]
    [SerializeField] private int _columns;
    [SerializeField] private int _rows;
    [SerializeField] private float _cellSize;
    [SerializeField] private float _spacing;
    [Header("Other")]
    [SerializeField] private int _startCellsCount;
    [SerializeField] private Cell _cellPrefab;

    private RectTransform _rectTransform;
    private Cell[,] _area;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        GenerateArea();
    }

    public void GenerateArea()
    {
        if (_area == null)
            CreateArea();

        for (int i = 0; i < _columns; i++)
            for (int j = 0; j < _rows; j++)
                _area[i, j].SetValue(0);

        for (int i = 0; i < _startCellsCount; i++)
            GenerateCell();
    }

    private void GenerateCell()
    {
        var emptyCells = new List<Cell>();

        for (int i = 0; i < _columns; i++)
            for (int j = 0; j < _rows; j++)
                if (_area[i, j].IsEmpty)
                    emptyCells.Add(_area[i, j]);

        if (emptyCells.Count == 0)
            throw new System.Exception("Нет свободных ячеек");

        var value = Random.Range(0, 10) == 0 ? 2 : 1;
        emptyCells[Random.Range(0, emptyCells.Count)].SetValue(value);
    }
    private void CreateArea()
    {
        _area = new Cell[_columns, _rows];
        SetAreaSize();

        var xPosition = -(_rectTransform.sizeDelta.x / 2) + (_cellSize / 2) + _spacing;
        var yPosition = (_rectTransform.sizeDelta.y / 2) - (_cellSize / 2) - _spacing;
        var offset = _cellSize + _spacing;
        for (int y = 0; y < _columns; y++)
        {
            for (int x = 0; x < _rows; x++)
            {
                var cell = Instantiate(_cellPrefab, transform, false);
                var position = new Vector2(xPosition + offset * x, yPosition - offset * y);
                cell.transform.localPosition = position;
                _area[y, x] = cell;
            }
        }
    }
    private void SetAreaSize()
    {
        var size = new Vector2(_columns * (_cellSize + _spacing) + _spacing, _rows * (_cellSize + _spacing) + _spacing);
        _rectTransform.sizeDelta = size;
    }
}
