using UnityEngine;

public class VerticalLineDetector : LineDetector
{
    private readonly Vector2Int[] _lineDirections;

    public VerticalLineDetector()
    {
        _lineDirections = new[] { Vector2Int.up, Vector2Int.down };
    }

    public override ItemSequence GetSequence(Board board, Vector2Int position)
    {
        return GetSequenceByDirection(board, position, _lineDirections);
    }
}
