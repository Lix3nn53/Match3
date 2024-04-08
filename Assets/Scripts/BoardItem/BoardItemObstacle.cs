using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardItemObstacle : BoardItem
{
    public override bool StartFalling()
    {
        return false;
    }
}
