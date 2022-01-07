using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CellAnimation : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _pointsText;
    [SerializeField] private ColorsConfig _colorsConfig;

    private Sequence _sequence;
    private const float _moveSpeed = .1f;
    private const float _scaleSpeed = .1f;

    public void Move(Cell from, Cell to, bool isMerging)
    {
        from.CancelAnimation();
        to.SetAnimation(this);

        Init(from);
        transform.position = from.transform.position;

        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOMove(to.transform.position, _moveSpeed).SetEase(Ease.InOutQuad));

        if(isMerging)
        {
            _sequence.AppendCallback(() =>
            {
                Init(to);
            });

            _sequence.Append(transform.DOScale(1.2f, _scaleSpeed));
            _sequence.Append(transform.DOScale(1, _scaleSpeed));
        }

        _sequence.AppendCallback(() =>
        {
            to.UpdateCell();
            Destroy();
        });

    }
    public void Appear(Cell cell)
    {
        cell.CancelAnimation();
        cell.SetAnimation(this);

        Init(cell);

        transform.position = cell.transform.position;
        transform.localScale = Vector2.zero;

        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOScale(1.2f, _scaleSpeed * 2));
        _sequence.Append(transform.DOScale(1f, _scaleSpeed * 2));

        _sequence.AppendCallback(() => 
        { 
            cell.UpdateCell();
            Destroy(); 
        });
    }
    public void Destroy()
    {
        _sequence.Kill();
        Destroy(gameObject);
    }

    private void Init(Cell cell)
    {
        _image.color = _colorsConfig.CellsColors[cell.Value];
        _pointsText.color = cell.Value < 3 ? _colorsConfig.DarkFontColor : _colorsConfig.LightFontColor;
        _pointsText.text = cell.Points.ToString();
    }
}
