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
    private Vector2 _startTouchPosition;
    private Vector2 _endSwipePosition;

    private void Start()
    {
        _area.Init(OnWin, OnLose, AddPoints);
        StartGame();
    }
    private void Update()
    {
        GetInput();
        var swipeDirection = GetSwipeDirection();
        if(_isGameStarted)
        {
            _area.OnInput(swipeDirection);
        }
    }

    private void StartGame()
    {
        _resultText.text = "";
        _isGameStarted = true;
        SetPoints(0);
        _area.GenerateArea();
    }
    private void OnWin()
    {
        _resultText.text = _winText;
    }
    private void OnLose()
    {
        _resultText.text = _loseText;
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
    private Vector2 GetSwipeDirection()
    {
        var direction = _endSwipePosition - _startTouchPosition;

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return direction.x > 0 ? Vector2.right : Vector2.left;
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            return direction.y > 0 ? Vector2.up : Vector2.down;

        return Vector2.zero;
    }
    private void GetInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _endSwipePosition = Input.mousePosition;
            isSwiped = true;
        }
    }
}
