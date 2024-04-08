using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class BoardItemGeneric : BoardItem
{
    public static float TWEEN_DURATION = .5f;
    private readonly TweenChain _tweenChain = new TweenChain();
    private void OnDestroy()
    {
        _tweenChain.Destroy();
    }

    public override bool DestroySelf()
    {
        if (_tweenChain.IsRunning())
        {
            return false;
        }

        return base.DestroySelf();
    }

    public override bool StartFalling()
    {
        BoardSlot slotToFallInto = GetSlotToFallInto();

        if (slotToFallInto == null)
        {
            return false;
        }

        BoardSlot oldSlot = CurrentSlot;
        oldSlot.CurrentItem = null;
        slotToFallInto.CurrentItem = this;
        CurrentSlot = slotToFallInto;

        MoveAnimation(oldSlot).Forget();

        Board.Instance.OnSlotEmpty(oldSlot);

        // if (oldSlot is BoardSlotFactory boardSlotFactory)
        // {
        //     Debug.Log("from: " + oldSlot.Position + " to " + slotToFallInto.Position, gameObject);
        // }

        return true;
    }

    private BoardSlot GetSlotToFallInto()
    {
        Board board = Board.Instance;

        Vector2Int currentPosition = CurrentSlot.Position;

        // Middle Down
        BoardSlot slot = board.GetBoardSlot(currentPosition, Vector2Int.down);
        if (slot != null)
        {
            BoardItem currentItem = slot.CurrentItem;
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
            BoardItem currentItem = slot.CurrentItem;
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
            BoardItem currentItem = slot.CurrentItem;
            if (currentItem == null)
            {
                // if Middle Down Right is empty
                return slot;
            }
        }

        return null;
    }

    private async UniTaskVoid MoveAnimation(BoardSlot oldSlot)
    {
        Sequence mySequence = DOTween.Sequence();

        float startDelay = 0;
        if (oldSlot is BoardSlotFactory boardSlotFactory)
        {
            startDelay = boardSlotFactory.Count * TWEEN_DURATION;

            boardSlotFactory.Count++;

            mySequence.PrependInterval(startDelay);
        }

        DelayedActivation(startDelay).Forget();

        // transform.position = CurrentSlot.transform.position;
        mySequence.Append(transform.DOMove(CurrentSlot.transform.position, TWEEN_DURATION).SetEase(Ease.Linear));

        _tweenChain.AddAndPlay(mySequence);

        await UniTask.Delay(TimeSpan.FromSeconds(TWEEN_DURATION));

        if (oldSlot is BoardSlotFactory a)
        {
            a.Count--;
        }
    }

    private async UniTaskVoid DelayedActivation(float delay)
    {
        if (delay > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }

        gameObject.SetActive(true);
    }
}
