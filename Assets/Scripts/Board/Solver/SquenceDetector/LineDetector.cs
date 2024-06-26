using System.Collections.Generic;
using UnityEngine;


public abstract class LineDetector : ISequenceDetector
{
    public abstract ItemSequence GetSequence(Board board, Vector2Int Vector2Int);

    protected ItemSequence GetSequenceByDirection(Board board, Vector2Int position, IEnumerable<Vector2Int> directions)
    {
        BoardSlot gridSlot = board.GetBoardSlot(position);
        List<BoardSlot> gridSlots = new List<BoardSlot>();

        foreach (Vector2Int direction in directions)
        {
            gridSlots.AddRange(GetSequenceOfGridSlots(board, gridSlot, position, direction));
        }

        if (gridSlots.Count < 2)
        {
            return null;
        }

        gridSlots.Add(gridSlot);

        return new ItemSequence(GetType(), gridSlots);
    }

    private IEnumerable<BoardSlot> GetSequenceOfGridSlots(Board board, BoardSlot slot, Vector2Int position, Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;
        List<BoardSlot> slotsSequence = new List<BoardSlot>();

        if (slot.CurrentItem == null)
        {
            return slotsSequence;
        }

        BoardItemType positionType = slot.CurrentItem.ItemType;

        BoardSlot currentSlot = board.GetBoardSlot(newPosition, includeFactory: false);
        while (currentSlot != null)
        {
            if (currentSlot.CurrentItem == null)
            {
                break;
            }

            if (currentSlot.CurrentItem.ItemType == positionType)
            {
                slotsSequence.Add(currentSlot);

                newPosition += direction;
                currentSlot = board.GetBoardSlot(newPosition, includeFactory: false);
            }
            else
            {
                break;
            }
        }

        return slotsSequence;
    }
}
