using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public int Value 
    { 
        get { return _value; } 
        private set { _value = value; UpdateCell(); } 
    }
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);
    public bool IsEmpty => Value == 0;
    public bool IsMaxmimal => Value == _maxValue;
    public bool IsMerged { get; private set; }

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private ColorsConfig _colorsConfig;

    private int _value;
    private Action<int> _valueChanged;
    private const int _maxValue = 11;

    public void Init(int x, int y, Action<int> onValueChanged)
    {
        X = x;
        Y = y;
        _valueChanged = onValueChanged;
    }
    public void MergeWithCell(Cell target)
    {
        target.IncreaseValue();
        SetValue(0);
    }
    public void MoveToCell(Cell target)
    {
        target.SetValue(Value);
        SetValue(0);
    }
    public void ResetFlags()
    {
        IsMerged = false;
    }
    public void SetValue(int value)
    {
        if (value < 0) 
            throw new Exception("Нельзя установить число меньше 0");

        Value = value;
    }

    private void IncreaseValue()
    {
        if (Value < _maxValue)
        {
            Value++;
            IsMerged = true;
            _valueChanged?.Invoke(Points);
        }
    }
    private void UpdateCell()
    {
        _pointsText.text = IsEmpty ? string.Empty : Points.ToString();
        _pointsText.color = Value <= 2 ? _colorsConfig.DarkFontColor : _colorsConfig.LightFontColor;
        _image.color = _colorsConfig.CellsColors[Value];
    }
}