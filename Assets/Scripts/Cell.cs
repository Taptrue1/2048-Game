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
    public bool IsMaxmimal => Value == _maxValue;
    public bool IsMerged { get; private set; }

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private ColorsConfig _colorsConfig;

    private CellAnimator _animator;
    private CellAnimation _currentAnimation;
    private Action<int> _valueChanged;
    private const int _maxValue = 11;

    public void Init(int x, int y, Action<int> onValueChanged, CellAnimator animator)
    {
        X = x;
        Y = y;
        _valueChanged = onValueChanged;
        _animator = animator;
    }
    public void MergeWithCell(Cell target)
    {
        _animator.Move(this, target, true);
        target.IncreaseValue();
        SetValue(0);
    }
    public void MoveToCell(Cell target)
    {
        _animator.Move(this, target, false);
        target.SetValue(Value, false);
        SetValue(0);
    }
    public void SetValue(int value, bool updateValue = true)
    {
        if (value < 0) 
            throw new Exception("Нельзя установить число меньше 0");

        Value = value;

        if(updateValue)
            UpdateCell();
    }
    public void ResetFlags()
    {
        IsMerged = false;
    }
    public void UpdateCell()
    {
        _pointsText.text = IsEmpty ? string.Empty : Points.ToString();
        _pointsText.color = Value < 3 ? _colorsConfig.DarkFontColor : _colorsConfig.LightFontColor;
        _image.color = _colorsConfig.CellsColors[Value];
    }
    public void SetAnimation(CellAnimation animation)
    {
        _currentAnimation = animation;
    }
    public void CancelAnimation()
    {
        if (_currentAnimation != null)
            _currentAnimation.Destroy();
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
}