using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardItemGeneric : BoardItem
{
    public override bool StartFalling()
    {
        EventManager.Instance.OnSwipe.Invoke(this, SwipeType.Left);

        return true;
    }
}
