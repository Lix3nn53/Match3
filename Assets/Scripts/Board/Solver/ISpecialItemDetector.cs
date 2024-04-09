using System.Collections.Generic;

public interface ISpecialItemDetector
{
    IEnumerable<BoardSlot> GetSpecialItemGridSlots(Board board, BoardSlot gridSlot);
}
