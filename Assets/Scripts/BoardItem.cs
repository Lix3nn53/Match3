using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BoardItem : MonoBehaviour
{
    [SerializeField] private BoardItemType _type;
    public BoardSlot CurrentSlot;

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

    public abstract bool StartFalling();
    public abstract bool MoveTo(BoardSlot slot);
}
