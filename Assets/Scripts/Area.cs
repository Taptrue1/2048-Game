using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Area : MonoBehaviour
{
    [Header("Area Settings")]
    [SerializeField] private int _areaSize;
    [SerializeField] private float _cellSize;
    [SerializeField] private float _spacing;

    [Header("Other")]
    [SerializeField] private int _startCellsCount;
    [SerializeField] private Cell _cellPrefab;

    private Action _win;
    private Action _lose;
    private Action<int> _valueChanged;
    private RectTransform _rectTransform;
    private Cell[,] _area;
    private bool _isAnyCellMoved;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Init(Action onWin, Action onLose, Action<int> onValueChanged)
    {
        _win = onWin;
        _lose = onLose;
        _valueChanged = onValueChanged;
    }
    public void OnInput(Vector2 direction)
    {
        _isAnyCellMoved = false;
        ResetCellsFlags();

        Move(direction);

        if(_isAnyCellMoved)
        {
            GenerateCell();
            CheckGameResult();
        }
    }
    public void GenerateArea()
    {
        if (_area == null)
            CreateArea();

        for (int y = 0; y < _areaSize; y++)
            for (int x = 0; x < _areaSize; x++)
                _area[y, x].SetValue(0);

        for (int i = 0; i < _startCellsCount; i++)
            GenerateCell();
    }

    private void Move(Vector2 direction)
    {
        var startXY = direction.x > 0 || direction.y < 0 ? _areaSize - 1 : 0;
        var dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        for(int i = 0; i < _areaSize; i++)
        {
            for(int k = startXY; k >= 0 && k < _areaSize; k -= dir)
            {
                var cell = direction.x != 0 ? _area[i, k] : _area[k, i];

                if (cell.IsEmpty) continue;

                var cellToMerge = FindCellToMerge(cell, direction);
                if(cellToMerge != null)
                {
                    _isAnyCellMoved = true;
                    cell.MergeWithCell(cellToMerge);

                    continue;
                }

                var emptyCell = FindEmptyCell(cell, direction);
                if(emptyCell != null)
                {
                    _isAnyCellMoved = true;
                    cell.MoveToCell(emptyCell);
                }
            }
        }
    }
    private void CheckGameResult()
    {
        var lose = true;

        for (int y = 0; y < _areaSize; y++)
        {
            for (int x = 0; x < _areaSize; x++)
            {
                if (_area[y, x].IsMaxmimal)
                {
                    _win?.Invoke();
                    break;
                }

                var canMerge = FindCellToMerge(_area[y, x], Vector2.up) || FindCellToMerge(_area[y, x], Vector2.down) ||
                    FindCellToMerge(_area[y, x], Vector2.right) || FindCellToMerge(_area[y, x], Vector2.left);

                if (lose && _area[y, x].IsEmpty || canMerge)
                {
                    lose = false;
                }
            }
        }

        if (lose)
            _lose?.Invoke();
    }
    private Cell FindCellToMerge(Cell cell, Vector2 direction)
    {
        var startX = cell.X + (int)direction.x;
        var startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; x >= 0 && y >= 0 && x < _areaSize && y < _areaSize; x += (int)direction.x, y -= (int)direction.y)
        {
            if (_area[y, x].IsEmpty)
                continue;

            if (_area[y, x].Value == cell.Value && !_area[y, x].IsMerged)
                return _area[y, x];

            break;
        }

        return null;
    }
    private Cell FindEmptyCell(Cell cell, Vector2 direction)
    {
        Cell emptyCell = null;
        var startX = cell.X + (int)direction.x;
        var startY = cell.Y - (int)direction.y;

        for (int x = startX, y = startY; x >= 0 && y >= 0 && x < _areaSize && y < _areaSize; x += (int)direction.x, y -= (int)direction.y)
        {
            if (_area[y, x].IsEmpty) 
                emptyCell = _area[y, x];
        }

        return emptyCell;
    }
    private void ResetCellsFlags()
    {
        for(int y = 0; y < _areaSize; y++)
        {
            for(int x = 0; x < _areaSize; x++)
            {
                _area[y, x].ResetFlags();
            }
        }
    }
    private void GenerateCell()
    {
        var emptyCells = new List<Cell>();

        for (int y = 0; y < _areaSize; y++)
            for (int x = 0; x < _areaSize; x++)
                if (_area[y, x].IsEmpty)
                    emptyCells.Add(_area[y, x]);

        if (emptyCells.Count == 0)
            throw new Exception("Нет свободных ячеек");

        var value = UnityEngine.Random.Range(0, 10) == 0 ? 2 : 1;
        var cell = emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)];
        cell.SetValue(value, false);
        CellAnimator.Instance.Appear(cell);
    }
    private void CreateArea()
    {
        _area = new Cell[_areaSize, _areaSize];
        SetAreaSize();

        var xPosition = -(_rectTransform.sizeDelta.x / 2) + (_cellSize / 2) + _spacing;
        var yPosition = (_rectTransform.sizeDelta.y / 2) - (_cellSize / 2) - _spacing;
        var offset = _cellSize + _spacing;

        for (int y = 0; y < _areaSize; y++)
        {
            for (int x = 0; x < _areaSize; x++)
            {
                var cell = Instantiate(_cellPrefab, transform, false);
                var position = new Vector2(xPosition + offset * x, yPosition - offset * y);
                cell.transform.localPosition = position;
                cell.Init(x, y, _valueChanged);
                _area[y, x] = cell;
            }
        }
    }
    private void SetAreaSize()
    {
        var size = _areaSize * (_cellSize + _spacing) + _spacing; 
        _rectTransform.sizeDelta = new Vector2(size, size);
    }
}
