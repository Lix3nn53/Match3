using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    // Singleton
    public static AssetManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Assets
    public GameObject SlotPrefab;
    public GameObject ItemType00;
    public GameObject ItemType01;
    public GameObject ItemType02;
    public GameObject ItemType03;
    public GameObject ItemObstacle01;
    public GameObject GetItemTypePrefab(BoardItemType type)
    {
        switch (type)
        {
            case BoardItemType.Type00:
                return ItemType00;
            case BoardItemType.Type01:
                return ItemType01;
            case BoardItemType.Type02:
                return ItemType02;
            case BoardItemType.Type03:
                return ItemType03;
            case BoardItemType.Obstacle01:
                return ItemObstacle01;
        }

        return null;
    }
}
