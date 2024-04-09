using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SwipeHandler : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] EventManager _eventManager;

    private void OnEnable()
    {
        _eventManager.OnSwipe += OnSwipeListener;
    }
    private void OnDisable()
    {
        _eventManager.OnSwipe -= OnSwipeListener;
    }
    private void OnSwipeListener(BoardItem item, SwipeType swipeType)
    {
        if (item.CurrentSlot == null)
        {
            return;
        }

        Vector2Int currentPosition = item.CurrentSlot.Position;
        Vector2Int swipePosition;

        switch (swipeType)
        {
            case SwipeType.Left:
                swipePosition = currentPosition + Vector2Int.left;
                break;
            case SwipeType.Right:
                swipePosition = currentPosition + Vector2Int.right;
                break;
            case SwipeType.Up:
                swipePosition = currentPosition + Vector2Int.up;
                break;
            case SwipeType.Down:
                swipePosition = currentPosition + Vector2Int.down;
                break;
            default:
                return;
        }

        item.CurrentSlot.Board.Swap(currentPosition, swipePosition);
    }
}
