using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Area _area;

    [Header("UI Options")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private string _winText;
    [SerializeField] private string _loseText;

    private int _points;
    private bool _isGameStarted;
    private bool _isSwiped;
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;

    private void Start()
    {
        _area.Init(OnWin, OnLose, AddPoints);
        StartGame();
    }
    private void Update()
    {
        GetInput();
        if(_isGameStarted && _isSwiped)
        {
            var swipeDirection = GetSwipeDirection();
            var isSwipeEqualsZero = swipeDirection == Vector2.zero;

            if (!isSwipeEqualsZero)
                _area.OnInput(swipeDirection);

            _isSwiped = false;
        }
    }

    public void StartGame()
    {
        _resultText.text = "";
        _isGameStarted = true;
        SetPoints(0);
        _area.GenerateArea();
    }

    private void OnWin()
    {
        _resultText.text = _winText;
        _isGameStarted = false;
    }
    private void OnLose()
    {
        _resultText.text = _loseText;
        _isGameStarted = false;
    }
    private void AddPoints(int value)
    {
        if (value < 0) return;
        SetPoints(_points + value);
    }
    private void SetPoints(int value)
    {
        _points = value;
        _scoreText.text = value.ToString();
    }
    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _endTouchPosition = Input.mousePosition;
            _isSwiped = true;
        }
    }
    private Vector2 GetSwipeDirection()
    {
        var direction = _endTouchPosition - _startTouchPosition;
        var positiveX = Mathf.Abs(direction.x);
        var positiveY = Mathf.Abs(direction.y);

        if (positiveX > positiveY)
            return direction.x > 0 ? Vector2.right : Vector2.left;
        else if (positiveY > positiveX)
            return direction.y > 0 ? Vector2.up : Vector2.down;

        return Vector2.zero;
    }
}