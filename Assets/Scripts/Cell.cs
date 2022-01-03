using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Value { get; private set; }
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);
    public bool IsEmpty => Value == 0;
    public bool IsMerged { get; private set; }

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private ColorsConfig _colorsConfig;

    private Action<int> _valueChanged;
    private const int _maxValue = 11;

    public void Init(int x, int y, Action<int> valueChanged)
    {
        X = x;
        Y = y;
        _valueChanged = valueChanged;
    }
    public void MergeWithCell(Cell target)
    {
        if (target = this)
            throw new System.Exception("Нельзя совместить плитку с самой собой");

        target.IncreaseValue();
        SetValue(0);

        UpdateCell();
    }
    public void MoveToCell(Cell target)
    {
        target.SetValue(Value);
        SetValue(0);

        UpdateCell();
    }
    public void ResetFlags()
    {
        IsMerged = false;
    }
    public void SetValue(int value)
    {
        Value = value;
        UpdateCell();
    }

    private void IncreaseValue()
    {
        if (Value < _maxValue)
        {
            Value++;
            IsMerged = true;
            _valueChanged?.Invoke(Points);
            UpdateCell();
        }
    }
    private void UpdateCell()
    {
        _pointsText.text = IsEmpty ? string.Empty : Points.ToString();
        _pointsText.color = Value <= 2 ? _colorsConfig.DarkFontColor : _colorsConfig.LightFontColor;
        _image.color = _colorsConfig.CellsColors[Value];
    }
}