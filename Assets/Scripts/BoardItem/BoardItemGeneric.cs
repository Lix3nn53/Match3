using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class BoardItemGeneric : BoardItem
{
    private readonly TweenChain _tweenChain = new TweenChain();
    private void OnDestroy()
    {
        _tweenChain.Destroy();
    }

    public override bool StartFalling()
    {
        BoardSlot slotToFallInto = GetSlotToFallInto();

        if (slotToFallInto == null)
        {
            return false;
        }

        BoardSlot currentParent = transform.parent.GetComponent<BoardSlot>();

        transform.parent = slotToFallInto.transform;
        // transform.localPosition = Vector2.zero;
        MoveAnimation(0).Forget();

        Board.Instance.OnSlotEmpty(currentParent);

        // Debug.Log("from: " + currentParent.Position + " to " + slotToFallInto.Position, gameObject);

        return true;
    }

    private BoardSlot GetSlotToFallInto()
    {
        Board board = Board.Instance;

        Vector2Int currentPosition = GetCurrentSlot().Position;

        // Middle Down
        BoardSlot slot = board.GetBoardSlot(currentPosition, Vector2Int.down);
        if (slot != null)
        {
            BoardItem currentItem = slot.GetCurrentItem();
            if (currentItem == null)
            {
                // if Middle Down is empty 
                return slot;
            }
        }

        // Middle Down is not empty
        // Check Middle Down Left
        slot = board.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.right);
        if (slot != null)
        {
            BoardItem currentItem = slot.GetCurrentItem();
            if (currentItem == null)
            {
                // if Middle Down Left is empty
                return slot;
            }
        }

        // Middle Down Left is not empty
        // Check Middle Down Right
        slot = board.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.left);
        if (slot != null)
        {
            BoardItem currentItem = slot.GetCurrentItem();
            if (currentItem == null)
            {
                // if Middle Down Right is empty
                return slot;
            }
        }

        return null;
    }

    private async UniTaskVoid MoveAnimation(float delay)
    {
        if (delay > 0)
        {
            Debug.Log("Delay: " + delay);
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }

        _tweenChain.AddAndPlay(transform.DOMove(transform.parent.position, .5f).SetEase(Ease.Linear));
    }
}
