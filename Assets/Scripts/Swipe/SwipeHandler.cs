using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("Object at start position: " + item.gameObject.name, item.gameObject);
        Debug.Log("Swipe: " + swipeType);
        item.DestroySelf();

        bool startFall = false;

        BoardSlot slot = _board.GetBoardSlot(item.GetCurrentSlot().Position, Vector2Int.up);
        if (slot != null)
        {
            BoardItem currentItem = slot.GetCurrentItem();
            if (currentItem != null)
            {
                startFall = currentItem.StartFalling();
            }
        }
        if (!startFall)
        {
            slot = _board.GetBoardSlot(item.GetCurrentSlot().Position, Vector2Int.up + Vector2Int.left);
            if (slot != null)
            {
                BoardItem currentItem = slot.GetCurrentItem();
                if (currentItem != null)
                {
                    startFall = currentItem.StartFalling();
                }
            }
            if (!startFall)
            {
                slot = _board.GetBoardSlot(item.GetCurrentSlot().Position, Vector2Int.up + Vector2Int.right);
                if (slot != null)
                {
                    BoardItem currentItem = slot.GetCurrentItem();
                    if (currentItem != null)
                    {
                        startFall = currentItem.StartFalling();
                    }
                }
            }
        }
    }
}
