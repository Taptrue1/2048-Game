using UnityEngine;

public class Game : MonoBehaviour
{

    private Vector2 _startTouchPosition;
    private Vector2 _endSwipePosition;
    private bool isSwiped;

    private void Update()
    {
        GetInput();
        var swipeDirection = GetSwipeDirection();
    }

    private Vector2 GetSwipeDirection()
    {
        var direction = _endSwipePosition - _startTouchPosition;

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return direction.x > 0 ? Vector2.right : Vector2.left;
        else
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
