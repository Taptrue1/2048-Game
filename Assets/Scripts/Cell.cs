using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Cell : MonoBehaviour
{
    public int Value { get; private set; }
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value);
    public bool IsEmpty => Value == 0;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private ColorsConfig _colorsConfig;

    private const int _maxValue = 11;

    private void Awake()
    {
        UpdateCell();
    }

    public void IncreaseValue()
    {
        if (Value < _maxValue)
        {
            Value++;
            UpdateCell();
        }
    }
    public void SetValue(int value)
    {
        Value = value;
        UpdateCell();
    }

    private void UpdateCell()
    {
        _pointsText.text = IsEmpty ? string.Empty : Points.ToString();
        _pointsText.color = Value <= 2 ? _colorsConfig.DarkFontColor : _colorsConfig.LightFontColor;
        _image.color = _colorsConfig.CellsColors[Value];
    }
}