using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public abstract class BoardItem : MonoBehaviour
{
    public static float TWEEN_DURATION = .25f;
    public static float TWEEN_DURATION_MIN = .1f;
    public static float TWEEN_DURATION_PERCENT_STEP = .2f;
    public static float TWEEN_SHAKE_DISTANCE = .25f;
    [SerializeField] private BoardItemType _itemType;
    public BoardItemType ItemType => _itemType;
    public BoardSlot CurrentSlot;
    public Tween CurrentTween;

    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool DestroySelf()
    {
        if (CurrentTween != null)
        {
            return false;
        }

        if (CurrentSlot != null)
        {
            CurrentSlot.Board.OnSlotEmpty(CurrentSlot);
            CurrentSlot.CurrentItem = null;
        }

        CurrentSlot = null;

        CurrentTween?.Kill();
        CurrentTween = transform.DOScale(0, TWEEN_DURATION / 2f).OnComplete(() => Destroy(gameObject));

        return true;
    }
    public void ClearCurrentSlot()
    {
        if (CurrentSlot != null)
        {
            CurrentSlot.CurrentItem = null;
            CurrentSlot = null;
        }
    }

    public void DebugColor()
    {
        _spriteRenderer.color = Color.red;
    }
    public abstract void CancelMovement();
    public abstract bool StartFalling(int alreadyFalled = 0);
    public abstract bool MoveTo(BoardSlot slot, Action onComplete);
}
