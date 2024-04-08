using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardItemGeneric : BoardItem
{
    public override bool StartFalling()
    {
        BoardSlot slotToFallInto = GetSlotToFallInto();

        if (slotToFallInto == null)
        {
            return false;
        }

        transform.parent = slotToFallInto.transform;
        transform.localPosition = Vector2.zero;

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
        slot = board.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.left);
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
        slot = board.GetBoardSlot(currentPosition, Vector2Int.down + Vector2Int.right);
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
}
