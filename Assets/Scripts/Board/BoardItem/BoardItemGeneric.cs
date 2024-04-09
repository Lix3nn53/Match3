using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class BoardItemGeneric : BoardItem
{
    public static float TWEEN_DURATION = .5f;
    private Sequence _tweenSquence;
    private void OnDestroy()
    {
        _tweenSquence?.Kill();
    }

    public override bool DestroySelf()
    {
        if (_tweenSquence != null)
        {
            return false;
        }

        return base.DestroySelf();
    }

    public override void CancelMovement()
    {
        _tweenSquence?.Kill();
        _tweenSquence = null;
    }

    public override bool StartFalling()
    {
        BoardSlot slotToFallInto = GetSlotToFallInto();

        if (slotToFallInto == null)
        {
            return false;
        }

        BoardSlot oldSlot = CurrentSlot;

        CurrentSlot.CurrentItem = null;
        CurrentSlot = null;

        Board.Instance.OnSlotEmpty(oldSlot);

        StartFallAnimation(oldSlot, slotToFallInto);

        return true;
    }

    public override bool MoveTo(BoardSlot slot, Action onComplete)
    {
        ClearCurrentSlot();

        StartMoveAnimation(slot, onComplete);

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

    private void StartFallAnimation(BoardSlot oldSlot, BoardSlot slotToFallInto)
    {
        // gameObject.SetActive(true);
        // transform.position = slotToFallInto.transform.position;
        slotToFallInto.ItemIncoming = this;

        _tweenSquence = DOTween.Sequence();

        if (oldSlot is BoardSlotFactory boardSlotFactory)
        {
            float startDelay = boardSlotFactory.CountForDelay * TWEEN_DURATION;

            boardSlotFactory.CountForDelay++;

            _tweenSquence.PrependInterval(startDelay);
        }

        _tweenSquence.Append(
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

                _tweenSquence.Kill();
                _tweenSquence = null;
            })
        );

        _tweenSquence.Play();
    }

    private void StartMoveAnimation(BoardSlot slotToMoveInto, Action onComplete)
    {
        // gameObject.SetActive(true);
        // transform.position = slotToFallInto.transform.position;
        slotToMoveInto.ItemIncoming = this;

        _tweenSquence?.Kill();
        _tweenSquence = DOTween.Sequence();

        _tweenSquence.Append(
            transform.DOMove(slotToMoveInto.transform.position, TWEEN_DURATION)
            .SetEase(Ease.Linear)
            .OnStart(() =>
            {
                gameObject.SetActive(true);
            })
            .OnComplete(() =>
            {
                CurrentSlot = slotToMoveInto;
                slotToMoveInto.CurrentItem = this;
                slotToMoveInto.ItemIncoming = null;

                _tweenSquence.Kill();
                _tweenSquence = null;

                onComplete?.Invoke();
            })
        );

        _tweenSquence.Play();
    }
}
