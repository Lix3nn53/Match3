using UnityEngine;
public interface ISequenceDetector
{
    ItemSequence GetSequence(Board board, Vector2Int position);
}
