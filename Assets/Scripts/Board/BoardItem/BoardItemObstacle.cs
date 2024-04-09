using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardItemObstacle : BoardItem
{
    public override void CancelMovement()
    {

    }
    public override bool StartFalling()
    {
        return false;
    }
    public override bool MoveTo(BoardSlot slot, Action onComplete)
    {
        return false;
    }
}
