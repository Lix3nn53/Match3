using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BoardItem : MonoBehaviour
{
    [SerializeField] private BoardItemType _type;

    public void DestroySelf()
    {
        transform.parent = null;
        Destroy(gameObject);
    }

    public BoardSlot GetCurrentSlot()
    {
        if (transform.parent == null)
        {
            throw new System.Exception("Parent is null");
        }

        return transform.parent.GetComponent<BoardSlot>();
    }

    public abstract bool StartFalling();
}
