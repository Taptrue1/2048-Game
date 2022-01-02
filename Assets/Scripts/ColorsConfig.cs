using UnityEngine;

[CreateAssetMenu(fileName = "ColorsConfig")]
public class ColorsConfig : ScriptableObject
{
    public Color DarkFontColor => _darkFontColor;
    public Color LightFontColor => _lightFontColor;
    public Color[] CellsColors => _cellsColors;

    [SerializeField] private Color _darkFontColor;
    [SerializeField] private Color _lightFontColor;
    [SerializeField] private Color[] _cellsColors;
}
