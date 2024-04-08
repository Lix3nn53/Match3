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
        Vector2Int currentPosition = item.GetCurrentSlot().Position;

        Board.Instance.DestroyOne(item);

        BoardSlot extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.down);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.GetCurrentItem());
        }

        extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.down);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.GetCurrentItem());
        }

        // Test(currentPosition).Forget();
    }

    private async UniTaskVoid Test(Vector2Int currentPosition)
    {
        await UniTask.NextFrame();

        BoardSlot extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.down);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.GetCurrentItem());
        }

        await UniTask.NextFrame();

        extraSlot = Board.Instance.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.down);
        if (extraSlot != null)
        {
            Board.Instance.DestroyOne(extraSlot.GetCurrentItem());
        }
    }
}
