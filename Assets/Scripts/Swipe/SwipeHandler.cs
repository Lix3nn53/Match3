using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeHandler : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] EventManager _eventManager;

    private void OnEnable()
    {
        _eventManager.OnSwipe += OnSwipeListener;
    }
    private void OnDisable()
    {
        _eventManager.OnSwipe -= OnSwipeListener;
    }
    private void OnSwipeListener(BoardItem item, SwipeType swipeType)
    {
        Board.Instance.DestroyOne(item);
    }
}
