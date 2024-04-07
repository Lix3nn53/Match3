using UnityEngine;

/*
 * Swipe Input script for Unity by @fonserbc, free to use wherever
 *
 * Attack to a gameObject, check the static booleans to check if a swipe has been detected this frame
 * Eg: if (SwipeInput.swipedRight) ...
 *
 * 
 */

public class SwipeInput : MonoBehaviour
{

    // If the touch is longer than MAX_SWIPE_TIME, we dont consider it a swipe
    [SerializeField] private float MAX_SWIPE_TIME = 0.2f;

    // Factor of the screen width that we consider a swipe
    // 0.17 works well for portrait mode 16:9 phone
    [SerializeField] private float MIN_SWIPE_DISTANCE = 0.17f;

    private Vector2 _startPos;
    private float _startTime;

    private void Update()
    {
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startPos = touch.position;
                _startTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (Time.time - _startTime > MAX_SWIPE_TIME) // press too long
                    return;

                Vector2 startPosPercent = new Vector2(_startPos.x / (float)Screen.width, _startPos.y / (float)Screen.width);
                Vector2 endPosPercent = new Vector2(touch.position.x / (float)Screen.width, touch.position.y / (float)Screen.width);

                Vector2 swipe = new Vector2(endPosPercent.x - startPosPercent.x, endPosPercent.y - startPosPercent.y);

                if (swipe.magnitude < MIN_SWIPE_DISTANCE) // Too short swipe
                    return;

                SwipeType swipeType = CalculateSwipeAngle(startPosPercent, endPosPercent);

                if (swipeType != SwipeType.Invalid)
                {
                    Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(_startPos);
                    RaycastHit2D hit = Physics2D.Raycast(clickedPosition, -Vector2.up);
                    if (hit)
                    {
                        // Hit an active BoardItem hit.collider.gameObject
                        Debug.Log("Object at start position: " + hit.collider.gameObject.name, hit.collider.gameObject);
                        Debug.Log("Swipe: " + swipeType);
                    }
                }
            }
        }
    }

    private SwipeType CalculateSwipeAngle(Vector2 first, Vector2 final)
    {
        float angle = Mathf.Atan2(final.y - first.y, final.x - first.x) * Mathf.Rad2Deg;

        if (angle > -45 && angle <= 45)
        {
            return SwipeType.Right;
        }
        else if (angle > 45 && angle <= 135)
        {
            return SwipeType.Up;
        }
        else if (angle > 135 || angle <= -135)
        {
            return SwipeType.Left;
        }
        else if (angle > -135 && angle <= -45)
        {
            return SwipeType.Down;
        }

        return SwipeType.Invalid;
    }
}
public enum SwipeType
{
    Left,
    Right,
    Up,
    Down,
    Invalid
}