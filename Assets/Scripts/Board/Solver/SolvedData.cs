using System.Collections.Generic;


public class SolvedData
{
    public SolvedData(IReadOnlyCollection<ItemSequence> solvedSequences, IReadOnlyCollection<BoardSlot> specialItemGridSlots)
    {
        SolvedSequences = solvedSequences;
        SpecialItemGridSlots = specialItemGridSlots;
    }

    public IReadOnlyCollection<BoardSlot> SpecialItemGridSlots { get; }
    public IReadOnlyCollection<ItemSequence> SolvedSequences { get; }

    public IEnumerable<BoardSlot> GetSolvedGridSlots(bool onlyMovable = false)
    {
        foreach (var sequence in SolvedSequences)
        {
            foreach (var solvedGridSlot in sequence.SolvedGridSlots)
            {
                // if (onlyMovable && solvedGridSlot.IsMovable == false)
                // {
                //     continue;
                // }

                yield return solvedGridSlot;
            }
        }
    }

    public IEnumerable<BoardSlot> GetSpecialItemGridSlots(bool excludeOccupied = false)
    {
        foreach (var specialItemGridSlot in SpecialItemGridSlots)
        {
            // if (excludeOccupied && specialItemGridSlot.HasItem)
            // {
            //     continue;
            // }

            yield return specialItemGridSlot;
        }
    }
}
