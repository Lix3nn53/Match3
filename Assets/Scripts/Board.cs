using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int ItemCount = 0;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _halfDistance = .5f;

    private BoardSlot[,] _grid;

    private GameObject _slotPrefab;
    private GameObject _slotFactoryPrefab;
    // Singleton
    public static Board Instance { get; private set; }

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

    // Start is called before the first frame update
    private void Start()
    {
        _slotPrefab = AssetManager.Instance.SlotPrefab;
        _slotFactoryPrefab = AssetManager.Instance.SlotFactoryPrefab;

        _grid = new BoardSlot[_width, _height + 1];

        SetUp();
    }
    private void SetUp()
    {
        float startX = (-_width * _halfDistance) + transform.position.x;
        float startY = (-_height * _halfDistance) + transform.position.y;

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height + 1; j++)
            {
                Vector2 pos = new Vector2(startX + (i * _halfDistance * 2) + _halfDistance, startY + (j * _halfDistance * 2) + _halfDistance);

                GameObject prefab = j == _height ? _slotFactoryPrefab : _slotPrefab;
                GameObject slotGameObject = Instantiate(prefab, pos, Quaternion.identity, transform);

                BoardSlot boardSlot = slotGameObject.GetComponent<BoardSlot>();
                boardSlot.FillRandom();

                boardSlot.Position = new Vector2Int(i, j);
                _grid[i, j] = boardSlot;

                slotGameObject.name = "GridSlot(" + i + ", " + j + ")";
            }
        }
    }

    public BoardSlot GetBoardSlot(Vector2Int center, Vector2Int offset = new Vector2Int())
    {
        int x = center.x + offset.x;
        int y = center.y + offset.y;

        // Check if the coordinates are within the bounds of the grid
        if (x >= 0 && x < _width && y >= 0 && y < _height + 1)
        {
            return _grid[x, y];
        }
        else
        {
            // Handle out-of-bounds case
            return null;
        }
    }

    public void DestroyOne(BoardItem item)
    {
        if (item == null)
        {
            return;
        }
        if (item.CurrentSlot == null)
        {
            return;
        }

        Vector2Int currentPosition = item.CurrentSlot.Position;

        bool destroy = item.DestroySelf();

        if (!destroy)
        {
            return;
        }

        StartFallingInto(currentPosition);
        // StartFalling(GetBoardSlot(currentPosition));
    }

    public bool StartFalling(BoardSlot slot)
    {
        if (slot != null)
        {
            BoardItem currentItem = slot.CurrentItem;
            if (currentItem == null)
            {
                // if empty do nothing and return
                return true;
            }
            bool startFall = currentItem.StartFalling();
            if (startFall)
            {
                StartFallingInto(slot.Position);
                return true;
            }
        }

        return false;
    }

    public void StartFallingInto(Vector2Int emptyPosition)
    {
        // Middle Up
        BoardSlot slot = GetBoardSlot(emptyPosition, Vector2Int.up);
        bool startFall = StartFalling(slot);
        if (startFall)
        {
            return;
        }

        // Middle Up is obstacle
        // Check Middle Up Left
        slot = GetBoardSlot(emptyPosition, Vector2Int.up + Vector2Int.left);
        startFall = StartFalling(slot);
        if (startFall)
        {
            return;
        }

        // Middle Up Left is obstacle
        // Check Middle Up Right
        slot = GetBoardSlot(emptyPosition, Vector2Int.up + Vector2Int.right);
        startFall = StartFalling(slot);
        if (startFall)
        {
            return;
        }
    }

    public void OnSlotEmpty(BoardSlot slot)
    {
        if (slot.Position.y == _height)
        {
            slot.FillRandom();
        }
    }
}
