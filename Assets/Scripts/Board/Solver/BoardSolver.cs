using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BoardSolver
{
    private readonly ISpecialItemDetector[] _specialItemDetectors;
    private readonly ISequenceDetector[] _sequenceDetectors;

    public BoardSolver(ISequenceDetector[] sequenceDetectors, ISpecialItemDetector[] specialItemDetectors)
    {
        _sequenceDetectors = sequenceDetectors;
        _specialItemDetectors = specialItemDetectors;
    }

    public SolvedData Solve(Board gameBoard, params Vector2Int[] Vector2Ints)
    {
        var resultSequences = new Collection<ItemSequence>();
        var specialItemGridSlots = new HashSet<BoardSlot>();

        foreach (var Vector2Int in Vector2Ints)
        {
            foreach (var sequenceDetector in _sequenceDetectors)
            {
                var sequence = sequenceDetector.GetSequence(gameBoard, Vector2Int);
                if (sequence == null)
                {
                    continue;
                }

                if (IsNewSequence(sequence, resultSequences) == false)
                {
                    continue;
                }

                foreach (var specialItemGridSlot in GetSpecialItemGridSlots(gameBoard, sequence))
                {
                    specialItemGridSlots.Add(specialItemGridSlot);
                }

                resultSequences.Add(sequence);
            }
        }

        return new SolvedData(resultSequences, specialItemGridSlots);
    }

    private bool IsNewSequence(ItemSequence newSequence, IEnumerable<ItemSequence> sequences)
    {
        var sequencesByType = sequences.Where(sequence => sequence.SequenceDetectorType == newSequence.SequenceDetectorType);
        var newSequenceGridSlot = newSequence.SolvedGridSlots[0];

        return sequencesByType.All(sequence => sequence.SolvedGridSlots.Contains(newSequenceGridSlot) == false);
    }

    private IEnumerable<BoardSlot> GetSpecialItemGridSlots(Board gameBoard, ItemSequence sequence)
    {
        foreach (var itemDetector in _specialItemDetectors)
        {
            foreach (var solvedGridSlot in sequence.SolvedGridSlots)
            {
                foreach (var specialItemGridSlot in itemDetector.GetSpecialItemGridSlots(gameBoard, solvedGridSlot))
                {
                    // var hasNextState = ((IStatefulSlot)specialItemGridSlot.State).NextState();
                    // if (hasNextState)
                    // {
                    //     continue;
                    // }

                    yield return specialItemGridSlot;
                }
            }
        }
    }
}
