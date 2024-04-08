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
        CurrentSlot = null;
        Board.Instance.OnSlotEmpty(oldSlot);

        StartMoveAnimation(oldSlot, slotToFallInto);

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

    private void StartMoveAnimation(BoardSlot oldSlot, BoardSlot slotToFallInto)
    {
        // gameObject.SetActive(true);
        // transform.position = slotToFallInto.transform.position;

        Sequence mySequence = DOTween.Sequence();

        if (oldSlot is BoardSlotFactory boardSlotFactory)
        {
            float startDelay = boardSlotFactory.Count * TWEEN_DURATION;

            boardSlotFactory.Count++;

            mySequence.PrependInterval(startDelay);
        }

        mySequence.Append(
            transform.DOMove(slotToFallInto.transform.position, TWEEN_DURATION)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                gameObject.SetActive(true);
            })
            .OnComplete(() =>
            {
                if (oldSlot is BoardSlotFactory a)
                {
                    a.Count--;
                }

                CurrentSlot = slotToFallInto;
                slotToFallInto.CurrentItem = this;

                StartFalling();
            })
        );

        _tweenChain.AddAndPlay(mySequence);
    }
}
