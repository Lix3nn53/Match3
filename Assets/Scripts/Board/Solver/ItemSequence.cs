using System;
using System.Collections.Generic;

public class ItemSequence
{
    public ItemSequence(Type sequenceDetectorType, IReadOnlyList<BoardSlot> solvedGridSlots)
    {
        SequenceDetectorType = sequenceDetectorType;
        SolvedGridSlots = solvedGridSlots;
    }

    public Type SequenceDetectorType { get; }
    public IReadOnlyList<BoardSlot> SolvedGridSlots { get; }
}