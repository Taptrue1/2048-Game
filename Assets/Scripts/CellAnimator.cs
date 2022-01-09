using UnityEngine;
using DG.Tweening;

public class CellAnimator : MonoBehaviour
{
    [SerializeField] private CellAnimation _cellAnimationPrefab;

    private void Awake()
    {
        DOTween.Init();
    }

    public void Move(Cell from, Cell to, bool isMerging)
    {
        Instantiate(_cellAnimationPrefab, transform, false).Move(from, to, isMerging);
    }
    public void Appear(Cell cell)
    {
        Instantiate(_cellAnimationPrefab, transform, false).Appear(cell);
    }
}
