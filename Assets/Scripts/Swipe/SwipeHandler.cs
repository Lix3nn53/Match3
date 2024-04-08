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

        Board.Instance.DestroyOne(item);

        BoardSlot extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.down);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.CurrentItem);
        }

        extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.right);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.CurrentItem);
        }

        extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.right);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.CurrentItem);
        }

        extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.left);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.CurrentItem);
        }

        extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.left);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.CurrentItem);
        }
    }
}
