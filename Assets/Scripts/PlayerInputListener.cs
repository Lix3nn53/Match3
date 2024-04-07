using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputListener : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _swipe;

    private Vector2 _swipeStart;
    private Vector2 _swipeEnd;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _swipe = _playerInput.currentActionMap.FindAction("Swipe");
    }


    private void OnEnable()
    {
        _swipe.started += OnSwipeStarted;
        _swipe.canceled += OnSwipeCanceled;
    }

    private void OnSwipeStarted(InputAction.CallbackContext ctx)
    {
        _swipeStart = ctx.ReadValue<Vector2>();
        Debug.Log("OnSwipeStarted: " + _swipeStart);
    }

    private void OnSwipeCanceled(InputAction.CallbackContext ctx)
    {
        _swipeEnd = ctx.ReadValue<Vector2>();
    }
}
