using UnityEngine;

public class HorizontalLineDetector : LineDetector
{
    private readonly Vector2Int[] _lineDirections;

    public HorizontalLineDetector()
    {
        _lineDirections = new[] { Vector2Int.left, Vector2Int.right };
    }

    public override ItemSequence GetSequence(Board board, Vector2Int Vector2Int)
    {
        return GetSequenceByDirection(board, Vector2Int, _lineDirections);
    }
}
