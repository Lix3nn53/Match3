using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BoardItem : MonoBehaviour
{
    [SerializeField] private BoardItemType _itemType;
    public BoardItemType ItemType => _itemType;
    public BoardSlot CurrentSlot;

    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual bool DestroySelf()
    {
        CurrentSlot.CurrentItem = null;

        Destroy(gameObject);

        Board.Instance.OnSlotEmpty(CurrentSlot);

        CurrentSlot = null;

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

    public void Debug()
    {
        _spriteRenderer.color = Color.red;
    }
    public abstract void CancelMovement();
    public abstract bool StartFalling();
    public abstract bool MoveTo(BoardSlot slot, Action onComplete);
}
