using UnityEngine;
using DG.Tweening;

public class CellAnimator : MonoBehaviour
{
    public static CellAnimator Instance { get; private set; }

    [SerializeField] private CellAnimation _cellAnimationPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

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
