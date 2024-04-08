using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class BoardItemGeneric : BoardItem
{
    public static float TWEEN_DURATION = .5f;
    private Sequence _tweenChain;
    private void OnDestroy()
    {
        _tweenChain?.Kill();
    }

    public override bool DestroySelf()
    {
        if (_tweenChain != null)
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
            if (currentItem == null && !slot.ItemIncoming)
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
            if (currentItem == null && !slot.ItemIncoming)
            {
                // if Middle Down Left is empty
                BoardSlot sideSlot = board.GetBoardSlot(currentPosition, Vector2Int.right);
                if (sideSlot.ItemIncoming == null)
                {
                    return slot;
                }
            }
        }

        // Middle Down Left is not empty
        // Check Middle Down Right
        slot = board.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.left);
        if (slot != null)
        {
            BoardItem currentItem = slot.CurrentItem;
            if (currentItem == null && !slot.ItemIncoming)
            {
                // if Middle Down Right is empty
                BoardSlot sideSlot = board.GetBoardSlot(currentPosition, Vector2Int.left);
                if (sideSlot.ItemIncoming == null)
                {
                    sideSlot = board.GetBoardSlot(currentPosition, Vector2Int.left + Vector2Int.left);

                    if (
                        sideSlot == null
                        || (sideSlot.CurrentItem == null && sideSlot.ItemIncoming == null)
                        || sideSlot.CurrentItem != null && sideSlot.CurrentItem is BoardItemObstacle
                    )
                    {
                        return slot;
                    }
                }
            }
        }

        return null;
    }

    private void StartMoveAnimation(BoardSlot oldSlot, BoardSlot slotToFallInto)
    {
        // gameObject.SetActive(true);
        // transform.position = slotToFallInto.transform.position;
        slotToFallInto.ItemIncoming = this;

        _tweenChain = DOTween.Sequence();

        if (oldSlot is BoardSlotFactory boardSlotFactory)
        {
            float startDelay = boardSlotFactory.CountForDelay * TWEEN_DURATION;

            boardSlotFactory.CountForDelay++;

            _tweenChain.PrependInterval(startDelay);
        }

        _tweenChain.Append(
            transform.DOMove(slotToFallInto.transform.position, TWEEN_DURATION)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                gameObject.SetActive(true);
            })
            .OnComplete(() =>
            {
                if (oldSlot is BoardSlotFactory boardSlotFactory)
                {
                    boardSlotFactory.CountForDelay--;
                }

                CurrentSlot = slotToFallInto;
                slotToFallInto.CurrentItem = this;
                slotToFallInto.ItemIncoming = null;

                bool startFall = StartFalling();
                if (startFall)
                {
                    Board.Instance.StartFallingInto(slotToFallInto.Position);
                }

                _tweenChain = null;
            })
        );

        _tweenChain.Play();
    }
}
